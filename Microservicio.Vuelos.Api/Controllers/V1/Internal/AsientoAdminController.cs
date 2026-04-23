using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Asiento;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/vuelos/{id_vuelo:int}/asientos")]
[Produces("application/json")]
[Authorize]
public class AsientoAdminController : ControllerBase
{
    private readonly IAsientoService _asientoService;

    public AsientoAdminController(IAsientoService asientoService)
    {
        _asientoService = asientoService;
    }

    // GET /vuelos/{id_vuelo}/asientos
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged(
        int id_vuelo,
        [FromQuery] bool? disponible,
        [FromQuery] string? clase,
        [FromQuery] string? numero_asiento,
        [FromQuery] string? posicion,
        [FromQuery] string? estado,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filter = new AsientoFilterDto
        {
            IdVuelo = id_vuelo,
            Disponible = disponible,
            Clase = clase,
            NumeroAsiento = numero_asiento,
            Posicion = posicion,
            Estado = estado,
            Page = page,
            PageSize = pageSize
        };

        var result = await _asientoService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de asientos realizada correctamente."));
    }

    // GET /vuelos/{id_vuelo}/asientos/{id_asiento}
    [HttpGet("{id_asiento:int}")]
    [ProducesResponseType(typeof(ApiResponse<AsientoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AsientoResponseDto>>> GetById(int id_vuelo, int id_asiento)
    {
        var result = await _asientoService.GetByIdAsync(id_asiento);
        if (result is null || result.IdVuelo != id_vuelo)
            return NotFound(ApiResponse<AsientoResponseDto>.Fail("Asiento no encontrado."));

        return Ok(ApiResponse<AsientoResponseDto>.Ok(result));
    }

    // POST /vuelos/{id_vuelo}/asientos
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<AsientoResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AsientoResponseDto>>> Create(int id_vuelo, [FromBody] AsientoRequestDto request)
    {
        request.IdVuelo = id_vuelo;
        var usuario = GetUsuario();
        var result = await _asientoService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_vuelo = result.IdVuelo, id_asiento = result.IdAsiento, version = "1" },
            ApiResponse<AsientoResponseDto>.Ok(result, "Asiento creado correctamente."));
    }

    // PATCH /vuelos/{id_vuelo}/asientos/{id_asiento} ? actualiza disponibilidad
    [HttpPatch("{id_asiento:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<AsientoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AsientoResponseDto>>> UpdateDisponibilidad(
        int id_vuelo,
        int id_asiento,
        [FromBody] AsientoDisponibilidadPatchDto request)
    {
        var actual = await _asientoService.GetByIdAsync(id_asiento);
        if (actual is null || actual.IdVuelo != id_vuelo)
            return NotFound(ApiResponse<AsientoResponseDto>.Fail("Asiento no encontrado."));

        var dto = new AsientoUpdateRequestDto
        {
            IdVuelo = actual.IdVuelo,
            NumeroAsiento = actual.NumeroAsiento,
            Clase = actual.Clase,
            Disponible = request.Disponible,
            PrecioExtra = actual.PrecioExtra,
            Posicion = actual.Posicion
        };

        var usuario = GetUsuario();
        var result = await _asientoService.UpdateAsync(id_asiento, dto, usuario);

        if (result is null)
            return NotFound(ApiResponse<AsientoResponseDto>.Fail("Asiento no encontrado."));

        return Ok(ApiResponse<AsientoResponseDto>.Ok(result, "Disponibilidad de asiento actualizada correctamente."));
    }

    private string GetUsuario()
    {
        return User?.Identity?.Name ?? "SYSTEM";
    }

    public class AsientoDisponibilidadPatchDto
    {
        public bool Disponible { get; set; }
    }
}
