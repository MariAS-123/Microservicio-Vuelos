using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking; // ? corregido

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/booking/aeropuertos")]
[Produces("application/json")]
[Authorize]
public class AeropuertoController : ControllerBase
{
    private readonly IAeropuertoService _aeropuertoService;

    public AeropuertoController(IAeropuertoService aeropuertoService)
    {
        _aeropuertoService = aeropuertoService;
    }

    // GET PAGINADO � Todos los roles autenticados
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] AeropuertoFilterDto filter)
    {
        var result = await _aeropuertoService.GetPagedAsync(filter);
        return Ok(ApiResponse<object>.Ok(result, "Consulta de aeropuertos realizada correctamente."));
    }

    // GET BY ID � Todos los roles autenticados
    [HttpGet("{id_aeropuerto:int}")]
    [ProducesResponseType(typeof(ApiResponse<AeropuertoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AeropuertoResponseDto>>> GetById(int id_aeropuerto)
    {
        var result = await _aeropuertoService.GetByIdAsync(id_aeropuerto);

        if (result is null)
            return NotFound(ApiResponse<AeropuertoResponseDto>.Fail("Aeropuerto no encontrado."));

        return Ok(ApiResponse<AeropuertoResponseDto>.Ok(result, "Aeropuerto obtenido correctamente."));
    }

    // ? POST, PUT, DELETE eliminados � no existen en el contrato Booking
}
