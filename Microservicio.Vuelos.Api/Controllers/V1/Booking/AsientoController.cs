using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Asiento;
using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

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
    private readonly IReservaService _reservaService;

    public AsientoController(
        IAsientoService asientoService,
        IReservaService reservaService)
    {
        _asientoService = asientoService;
        _reservaService = reservaService;
    }

    // GET � Listar asientos de un vuelo
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetByVuelo(int id_vuelo, [FromQuery] AsientoFilterDto filter)
    {
        filter.IdVuelo = id_vuelo; // ? forzar filtro por vuelo
        var result = await _asientoService.GetPagedAsync(filter);

        if (GetRol() == "CLIENTE")
        {
            var reservasVuelo = await _reservaService.GetPagedAsync(new ReservaFilterDto
            {
                IdVuelo = id_vuelo,
                Page = 1,
                PageSize = 200
            });

            var asientosReservadosActivos = reservasVuelo.Items
                .Where(r => r.EstadoReserva is "PEN" or "CON" or "EMI")
                .Select(r => r.IdAsiento)
                .ToHashSet();

            var itemsFiltrados = result.Items
                .Where(a => !asientosReservadosActivos.Contains(a.IdAsiento))
                .ToList();

            result = new DataPagedResult<AsientoResponseDto>
            {
                Items = itemsFiltrados,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = itemsFiltrados.Count
            };
        }

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

    private string GetRol() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
