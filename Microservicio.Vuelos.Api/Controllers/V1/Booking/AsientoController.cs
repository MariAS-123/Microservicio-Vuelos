using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Asiento;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/booking/vuelos/{id_vuelo:int}/asientos")] // ? ruta corregida
[Produces("application/json")]
[Authorize]
public class AsientoController : ControllerBase
{
    private readonly IAsientoService _asientoService;

    public AsientoController(IAsientoService asientoService)
    {
        _asientoService = asientoService;
    }

    // GET � Listar asientos de un vuelo
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetByVuelo(int id_vuelo, [FromQuery] AsientoFilterDto filter)
    {
        filter.IdVuelo = id_vuelo; // ? forzar filtro por vuelo
        var result = await _asientoService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Asientos del vuelo obtenidos correctamente."));
    }

    // GET BY ID � Obtener asiento espec�fico
    [HttpGet("{id_asiento:int}")]
    [ProducesResponseType(typeof(ApiResponse<AsientoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AsientoResponseDto>>> GetById(int id_vuelo, int id_asiento)
    {
        var result = await _asientoService.GetByIdAsync(id_asiento);

        if (result is null)
            return NotFound(ApiResponse<AsientoResponseDto>.Fail("Asiento no encontrado."));

        return Ok(ApiResponse<AsientoResponseDto>.Ok(result));
    }
}
