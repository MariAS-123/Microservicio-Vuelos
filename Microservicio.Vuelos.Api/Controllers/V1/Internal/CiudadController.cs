using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Ciudad;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/ciudades")]
[Produces("application/json")]
[Authorize] // ? M�nimo: estar autenticado
public class CiudadController : ControllerBase
{
    private readonly ICiudadService _ciudadService;

    public CiudadController(ICiudadService ciudadService)
    {
        _ciudadService = ciudadService;
    }

    // GET PAGINADO ? Todos los roles autenticados
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] CiudadFilterDto filter)
    {
        NormalizeFilter(filter);
        var result = await _ciudadService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de ciudades realizada correctamente."));
    }

    // GET BY ID ? Todos los roles autenticados
    [HttpGet("{id_ciudad:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<CiudadResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CiudadResponseDto>>> GetById(int id_ciudad)
    {
        var result = await _ciudadService.GetByIdAsync(id_ciudad);

        if (result is null)
            return NotFound(ApiResponse<CiudadResponseDto>.Fail("Ciudad no encontrada."));

        return Ok(ApiResponse<CiudadResponseDto>.Ok(result, "Ciudad obtenida correctamente."));
    }

    // POST ? Solo ADMINISTRADOR gestiona el cat�logo de ciudades
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR")] // ?
    [ProducesResponseType(typeof(ApiResponse<CiudadResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<CiudadResponseDto>>> Create([FromBody] CiudadRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _ciudadService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_ciudad = result.IdCiudad, version = "1" },
            ApiResponse<CiudadResponseDto>.Ok(result, "Ciudad creada correctamente."));
    }

    // PUT ? Solo ADMINISTRADOR
    [HttpPut("{id_ciudad:int}")]
    [Authorize(Roles = "ADMINISTRADOR")] // ?
    [ProducesResponseType(typeof(ApiResponse<CiudadResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<CiudadResponseDto>>> Update(int id_ciudad, [FromBody] CiudadUpdateRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _ciudadService.UpdateAsync(id_ciudad, request, usuario);

        if (result is null)
            return NotFound(ApiResponse<CiudadResponseDto>.Fail("Ciudad no encontrada."));

        return Ok(ApiResponse<CiudadResponseDto>.Ok(result, "Ciudad actualizada correctamente."));
    }

    // DELETE ? Solo ADMINISTRADOR
    [HttpDelete("{id_ciudad:int}")]
    [Authorize(Roles = "ADMINISTRADOR")] // ?
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id_ciudad)
    {
        var usuario = GetUsuario();
        var result = await _ciudadService.DeleteAsync(id_ciudad, usuario);

        return Ok(ApiResponse<bool>.Ok(result, "Ciudad eliminada correctamente."));
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

    private static void NormalizeFilter(CiudadFilterDto filter)
    {
        if (filter.IdPais.HasValue && filter.IdPais.Value <= 0)
            filter.IdPais = null;

        if (filter.Page <= 0)
            filter.Page = 1;

        if (filter.PageSize <= 0)
            filter.PageSize = 20;
        else if (filter.PageSize > 200)
            filter.PageSize = 200;
    }
}
