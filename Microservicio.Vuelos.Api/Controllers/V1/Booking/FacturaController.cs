using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Factura;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/booking/facturas")]
[Produces("application/json")]
[Authorize] // ? M�nimo: estar autenticado
public class FacturaController : ControllerBase
{
    private readonly IFacturaService _facturaService;

    public FacturaController(IFacturaService facturaService)
    {
        _facturaService = facturaService;
    }

    // POST � Solo ADMINISTRADOR y AEROLINEA pueden emitir facturas
    // El CLIENTE no puede generar facturas directamente, las genera el sistema tras el pago
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Create([FromBody] FacturaRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _facturaService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_factura = result.IdFactura, version = "1" },
            ApiResponse<FacturaResponseDto>.Ok(result, "Factura generada correctamente."));
    }

    // GET BY ID � ADMINISTRADOR y AEROLINEA ven cualquiera; CLIENTE solo la propia (service)
    [HttpGet("{id_factura:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")] // ?
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> GetById(int id_factura)
    {
        var result = await _facturaService.GetByIdAsync(id_factura, GetIdCliente(), GetRol());

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(result));
    }

    private string GetUsuario()
    {
        return User?.Identity?.Name ?? "BOOKING";
    }

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string GetRol() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
