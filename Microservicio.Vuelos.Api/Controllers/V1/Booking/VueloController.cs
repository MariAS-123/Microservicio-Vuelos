using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/booking/vuelos")]
[Produces("application/json")]
[Authorize] // ? Solo lectura: cualquier usuario autenticado puede consultar vuelos
public class VueloController : ControllerBase
{
    private readonly IVueloService _vueloService;

    public VueloController(IVueloService vueloService)
    {
        _vueloService = vueloService;
    }

    // GET PAGINADO � Todos los roles autenticados
    // Booking lo usa para el buscador de vuelos disponibles (filtrar por estado_vuelo=PROGRAMADO)
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] VueloFilterDto filter)
    {
        NormalizeFilter(filter);

        var result = await _vueloService.GetPagedBookingAsync(filter); // ?

        return Ok(ApiResponse<object>.Ok(result, "Consulta de vuelos realizada correctamente."));
    }

    // GET BY ID � Todos los roles autenticados
    // Booking lo usa en la pantalla de detalle antes de confirmar la selecci�n
    [HttpGet("{id_vuelo:int}")]
    [ProducesResponseType(typeof(ApiResponse<VueloResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<VueloResponseDto>>> GetById(int id_vuelo)
    {
        var result = await _vueloService.GetByIdAsync(id_vuelo);

        if (result is null)
            return NotFound(ApiResponse<VueloResponseDto>.Fail("Vuelo no encontrado."));

        return Ok(ApiResponse<VueloResponseDto>.Ok(result, "Vuelo obtenido correctamente."));
    }


    private static void NormalizeFilter(VueloFilterDto filter)
    {
        if (filter.IdAeropuertoOrigen.HasValue && filter.IdAeropuertoOrigen.Value <= 0)
            filter.IdAeropuertoOrigen = null;

        if (filter.IdAeropuertoDestino.HasValue && filter.IdAeropuertoDestino.Value <= 0)
            filter.IdAeropuertoDestino = null;

        if (filter.FechaSalida.HasValue && filter.FechaSalida.Value <= DateTime.MinValue.AddDays(1))
            filter.FechaSalida = null;
    }
}
