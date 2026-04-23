using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/vuelos")]
[Produces("application/json")]
[Authorize] // ? M�nimo: estar autenticado
public class VueloAdminController : ControllerBase
{
    private readonly IVueloService _vueloService;

    public VueloAdminController(IVueloService vueloService)
    {
        _vueloService = vueloService;
    }

    // GET PAGINADO � Todos los roles autenticados
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] VueloFilterDto filter)
    {
        var result = await _vueloService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de vuelos realizada correctamente."));
    }

    // GET BY ID � Todos los roles autenticados
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

    // POST � Solo ADMINISTRADOR y AEROLINEA pueden crear vuelos
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<VueloResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VueloResponseDto>>> Create([FromBody] VueloRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _vueloService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_vuelo = result.IdVuelo, version = "1" },
            ApiResponse<VueloResponseDto>.Ok(result, "Vuelo creado correctamente."));
    }

    // PUT � Solo ADMINISTRADOR y AEROLINEA pueden actualizar vuelos
    [HttpPut("{id_vuelo:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<VueloResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VueloResponseDto>>> Update(int id_vuelo, [FromBody] VueloUpdateRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _vueloService.UpdateAsync(id_vuelo, request, usuario);

        if (result is null)
            return NotFound(ApiResponse<VueloResponseDto>.Fail("Vuelo no encontrado."));

        return Ok(ApiResponse<VueloResponseDto>.Ok(result, "Vuelo actualizado correctamente."));
    }

    // PATCH /estado � ADMINISTRADOR y AEROLINEA cambian estado operativo del vuelo
    // Estados v�lidos: PROGRAMADO | EN_VUELO | ATERRIZADO | CANCELADO | DEMORADO
    [HttpPatch("{id_vuelo:int}/estado")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")] // ?
    [ProducesResponseType(typeof(ApiResponse<VueloResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VueloResponseDto>>> UpdateEstado(
        int id_vuelo,
        [FromBody] VueloEstadoRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _vueloService.UpdateEstadoAsync(id_vuelo, request, usuario);

        if (result is null)
            return NotFound(ApiResponse<VueloResponseDto>.Fail("Vuelo no encontrado."));

        return Ok(ApiResponse<VueloResponseDto>.Ok(result, "Estado del vuelo actualizado correctamente."));
    }

    // DELETE � Solo ADMINISTRADOR puede eliminar vuelos
    [HttpDelete("{id_vuelo:int}")]
    [Authorize(Roles = "ADMINISTRADOR")] // ?
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id_vuelo)
    {
        var usuario = GetUsuario();
        var result = await _vueloService.DeleteAsync(id_vuelo, usuario);

        return Ok(ApiResponse<bool>.Ok(result, "Vuelo eliminado correctamente."));
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
}
