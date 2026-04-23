using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Pasajero;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pasajeros")]
[Produces("application/json")]
[Authorize] // ? M?nimo: estar autenticado
public class PasajeroAdminController : ControllerBase
{
    private readonly IPasajeroService _pasajeroService;

    public PasajeroAdminController(IPasajeroService pasajeroService)
    {
        _pasajeroService = pasajeroService;
    }

    // GET PAGINADO ? Solo ADMINISTRADOR y AEROLINEA pueden ver el listado completo
    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] PasajeroFilterDto filter)
    {
        var result = await _pasajeroService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de pasajeros realizada correctamente."));
    }

    // GET BY ID ? ADMINISTRADOR, AEROLINEA y CLIENTE pueden consultar un pasajero
    [HttpGet("{id_pasajero:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")] // ?
    [ProducesResponseType(typeof(ApiResponse<PasajeroResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PasajeroResponseDto>>> GetById(int id_pasajero)
    {
        var result = await _pasajeroService.GetByIdAsync(id_pasajero, GetIdCliente(), GetRol());

        if (result is null)
            return NotFound(ApiResponse<PasajeroResponseDto>.Fail("Pasajero no encontrado."));

        return Ok(ApiResponse<PasajeroResponseDto>.Ok(result));
    }

    // POST ? ADMINISTRADOR, AEROLINEA y CLIENTE pueden registrar pasajeros
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")] // ?
    [ProducesResponseType(typeof(ApiResponse<PasajeroResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PasajeroResponseDto>>> Create([FromBody] PasajeroRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _pasajeroService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_pasajero = result.IdPasajero, version = "1" },
            ApiResponse<PasajeroResponseDto>.Ok(result, "Pasajero creado correctamente."));
    }

    // PUT ? Solo ADMINISTRADOR y AEROLINEA pueden modificar datos de pasajeros
    // CLIENTE no puede editar pasajeros una vez registrados
    [HttpPut("{id_pasajero:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<PasajeroResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PasajeroResponseDto>>> Update(int id_pasajero, [FromBody] PasajeroUpdateRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _pasajeroService.UpdateAsync(id_pasajero, request, usuario);

        if (result is null)
            return NotFound(ApiResponse<PasajeroResponseDto>.Fail("Pasajero no encontrado."));

        return Ok(ApiResponse<PasajeroResponseDto>.Ok(result, "Pasajero actualizado correctamente."));
    }

    private string GetUsuario()
    {
        return User?.Identity?.Name ?? "SYSTEM";
    }

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string GetRol() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
