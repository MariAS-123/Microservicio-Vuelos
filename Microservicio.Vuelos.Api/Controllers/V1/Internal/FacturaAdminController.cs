using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Factura;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/facturas")]
[Produces("application/json")]
[Authorize] // ? M?nimo: estar autenticado
public class FacturaAdminController : ControllerBase
{
    private readonly IFacturaService _facturaService;

    public FacturaAdminController(IFacturaService facturaService)
    {
        _facturaService = facturaService;
    }

    // GET PAGINADO ? Solo ADMINISTRADOR y AEROLINEA pueden ver el listado completo
    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] FacturaFilterDto filter)
    {
        var result = await _facturaService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de facturas realizada correctamente."));
    }

    // GET BY ID ? ADMINISTRADOR y AEROLINEA ven cualquiera; CLIENTE solo la propia (service)
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

    // POST ? Solo ADMINISTRADOR y AEROLINEA emiten facturas
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
            ApiResponse<FacturaResponseDto>.Ok(result, "Factura creada correctamente."));
    }

    // PATCH /anular ? Solo ADMINISTRADOR puede anular facturas
    [HttpPatch("{id_factura:int}/anular")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Anular(
        int id_factura,
        [FromBody] FacturaUpdateRequestDto request)
    {
        request ??= new FacturaUpdateRequestDto();
        request.Estado = "INA";
        var usuario = GetUsuario();
        var result = await _facturaService.UpdateEstadoAsync(id_factura, request, usuario);

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(result, "Estado de factura actualizado correctamente."));
    }

    // PATCH /aprobar ? ADMINISTRADOR y AEROLINEA pueden aprobar facturas
    [HttpPatch("{id_factura:int}/aprobar")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Aprobar(int id_factura)
    {
        var usuario = GetUsuario();
        var result = await _facturaService.AprobarAsync(id_factura, usuario);

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(result, "Factura aprobada correctamente."));
    }

    // POST /pagar queda solo como operación administrativa legacy.
    [HttpPost("{id_factura:int}/pagar")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Pagar(int id_factura)
    {
        var usuario = GetUsuario();
        var result = await _facturaService.PagarAsync(id_factura, GetIdCliente(), GetRol(), usuario);

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(result, "Pago simulado correctamente. Factura aprobada."));
    }

    private string GetUsuario()
    {
        var name = User?.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name.Trim();
        var username = User?.FindFirst("username")?.Value;
        if (!string.IsNullOrWhiteSpace(username))
            return username.Trim();
        return "SYSTEM";
    }

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string GetRol() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
