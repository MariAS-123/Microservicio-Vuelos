using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Escala;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/booking/vuelos/{id_vuelo:int}/escalas")] // ? ruta corregida
[Produces("application/json")]
[Authorize]
public class EscalaController : ControllerBase
{
    private readonly IEscalaService _escalaService;

    public EscalaController(IEscalaService escalaService)
    {
        _escalaService = escalaService;
    }

    // GET � Listar escalas de un vuelo
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetByVuelo(int id_vuelo)
    {
        var filter = new EscalaFilterDto { IdVuelo = id_vuelo };
        var result = await _escalaService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Escalas del vuelo obtenidas correctamente."));
    }

    // GET BY ID � Obtener escala espec�fica
    [HttpGet("{id_escala:int}")]
    [ProducesResponseType(typeof(ApiResponse<EscalaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<EscalaResponseDto>>> GetById(int id_vuelo, int id_escala)
    {
        var result = await _escalaService.GetByIdAsync(id_escala);

        if (result is null)
            return NotFound(ApiResponse<EscalaResponseDto>.Fail("Escala no encontrada."));

        return Ok(ApiResponse<EscalaResponseDto>.Ok(result));
    }
}
