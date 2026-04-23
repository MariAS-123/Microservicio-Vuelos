using System.Linq;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.UsuarioRol;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/usuarios/{id_usuario:int}/roles")]
[Produces("application/json")]
[Authorize(Roles = "ADMINISTRADOR")]
public class UsuarioRolController : ControllerBase
{
    private readonly IUsuarioRolService _service;

    public UsuarioRolController(IUsuarioRolService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetByUsuario(
        int id_usuario,
        [FromQuery] UsuarioRolFilterDto filter)
    {
        filter.IdUsuario = id_usuario;
        var result = await _service.GetPagedAsync(filter);
        return Ok(ApiResponse<object>.Ok(result, "Consulta de usuario-rol realizada correctamente."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UsuarioRolResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<UsuarioRolResponseDto>>> Create(int id_usuario, [FromBody] UsuarioRolRequestDto request)
    {
        request.IdUsuario = id_usuario;
        var usuario = GetUsuario();
        var result = await _service.CreateAsync(request, usuario);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<UsuarioRolResponseDto>.Ok(result, "Rol asignado al usuario correctamente."));
    }

    [HttpDelete("{id_rol:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id_usuario, int id_rol)
    {
        var usuario = GetUsuario();
        var asignaciones = await _service.GetPagedAsync(new UsuarioRolFilterDto
        {
            IdUsuario = id_usuario,
            IdRol = id_rol,
            Page = 1,
            PageSize = 1
        });
        var asignacion = asignaciones.Items.FirstOrDefault();
        if (asignacion is null)
            return NotFound(ApiResponse<bool>.Fail("Asignaci?n usuario-rol no encontrada."));

        var result = await _service.DeleteAsync(asignacion.IdUsuarioRol, usuario);

        return Ok(ApiResponse<bool>.Ok(result, "Asignaci?n usuario-rol eliminada correctamente."));
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
