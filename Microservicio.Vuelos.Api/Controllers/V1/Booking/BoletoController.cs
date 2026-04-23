using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Boleto;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/booking/boletos")]
[Produces("application/json")]
[Authorize] // ? M�nimo: estar autenticado
public class BoletoController : ControllerBase
{
    private readonly IBoletoService _boletoService;

    public BoletoController(IBoletoService boletoService)
    {
        _boletoService = boletoService;
    }

    // POST � Solo ADMINISTRADOR y AEROLINEA pueden emitir boletos
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<BoletoResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BoletoResponseDto>>> Create([FromBody] BoletoRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _boletoService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_boleto = result.IdBoleto, version = "1" },
            ApiResponse<BoletoResponseDto>.Ok(result, "Boleto emitido correctamente."));
    }

    // GET BY ID � ADMINISTRADOR, AEROLINEA y CLIENTE pueden consultar su boleto
    [HttpGet("{id_boleto:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")] // ?
    [ProducesResponseType(typeof(ApiResponse<BoletoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BoletoResponseDto>>> GetById(int id_boleto)
    {
        var result = await _boletoService.GetByIdAsync(id_boleto, GetIdCliente(), GetRol());

        if (result is null)
            return NotFound(ApiResponse<BoletoResponseDto>.Fail("Boleto no encontrado."));

        return Ok(ApiResponse<BoletoResponseDto>.Ok(result));
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
