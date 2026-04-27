using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/reservas")]
[Produces("application/json")]
[Authorize]
public class ReservaAdminController : ControllerBase
{
    private readonly IReservaService _reservaService;

    public ReservaAdminController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    // GET PAGINADO ? Solo ADMINISTRADOR y AEROLINEA pueden ver el listado completo
    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] ReservaFilterDto filter)
    {
        var result = await _reservaService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de reservas realizada correctamente."));
    }

    // GET BY ID ? ADMINISTRADOR, AEROLINEA y CLIENTE (solo la propia en el service)
    [HttpGet("{id_reserva:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> GetById(int id_reserva)
    {
        var result = await _reservaService.GetByIdAsync(id_reserva, GetIdCliente(), GetRol()); // ?

        if (result is null)
            return NotFound(ApiResponse<ReservaResponseDto>.Fail("Reserva no encontrada."));

        return Ok(ApiResponse<ReservaResponseDto>.Ok(result));
    }

    // POST ? Crear reserva (internal) segun contrato
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> Create([FromBody] ReservaRequestDto request)
    {
        var result = await _reservaService.CreateAsync(request, GetUsuario());

        return CreatedAtAction(
            nameof(GetById),
            new { id_reserva = result.IdReserva, version = "1" },
            ApiResponse<ReservaResponseDto>.Ok(result, "Reserva creada correctamente."));
    }

    // PATCH /estado ? ADMINISTRADOR, AEROLINEA y CLIENTE (propio) cambian estado
    [HttpPatch("{id_reserva:int}/estado")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> UpdateEstado(
        int id_reserva,
        [FromBody] ReservaUpdateRequestDto request)
    {
        var result = await _reservaService.UpdateEstadoAsync(id_reserva, request, GetUsuario(), GetIdCliente(), GetRol());

        if (result is null)
            return NotFound(ApiResponse<ReservaResponseDto>.Fail("Reserva no encontrada."));

        return Ok(ApiResponse<ReservaResponseDto>.Ok(result, "Estado de reserva actualizado correctamente."));
    }

    private string GetUsuario() =>
        User?.Identity?.Name ?? "SYSTEM";

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string GetRol() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
