using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Rol;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/roles")]
[Produces("application/json")]
[Authorize(Roles = "ADMINISTRADOR")] // ? M�dulo de seguridad: acceso exclusivo al ADMINISTRADOR
public class RolController : ControllerBase
{
    private readonly IRolService _service;

    public RolController(IRolService service)
    {
        _service = service;
    }

    // GET PAGINADO ? Solo ADMINISTRADOR
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] RolFilterDto filter)
    {
        var result = await _service.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de roles realizada correctamente."));
    }

    // GET BY ID ? Solo ADMINISTRADOR
    [HttpGet("{id_rol:int}")]
    [ProducesResponseType(typeof(ApiResponse<RolResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<RolResponseDto>>> GetById(int id_rol)
    {
        var result = await _service.GetByIdAsync(id_rol);

        if (result is null)
            return NotFound(ApiResponse<RolResponseDto>.Fail("Rol no encontrado."));

        return Ok(ApiResponse<RolResponseDto>.Ok(result));
    }

    // POST ? Solo ADMINISTRADOR puede crear roles
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RolResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<RolResponseDto>>> Create([FromBody] RolRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _service.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_rol = result.IdRol, version = "1" },
            ApiResponse<RolResponseDto>.Ok(result, "Rol creado correctamente."));
    }

    // PUT ? Solo ADMINISTRADOR puede modificar roles
    [HttpPut("{id_rol:int}")]
    [ProducesResponseType(typeof(ApiResponse<RolResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<RolResponseDto>>> Update(
        int id_rol,
        [FromBody] RolUpdateRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _service.UpdateAsync(id_rol, request, usuario);

        if (result is null)
            return NotFound(ApiResponse<RolResponseDto>.Fail("Rol no encontrado."));

        return Ok(ApiResponse<RolResponseDto>.Ok(result, "Rol actualizado correctamente."));
    }

    // DELETE ? Solo ADMINISTRADOR puede eliminar roles
    [HttpDelete("{id_rol:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id_rol)
    {
        var usuario = GetUsuario();
        var result = await _service.DeleteAsync(id_rol, usuario);

        return Ok(ApiResponse<bool>.Ok(result, "Rol eliminado correctamente."));
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
