using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.UsuarioApp;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/usuarios")]
[Produces("application/json")]
[Authorize(Roles = "ADMINISTRADOR,CLIENTE")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioAppService _service;

    public UsuarioController(IUsuarioAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] UsuarioAppFilterDto filter)
    {
        var result = await _service.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de usuarios realizada correctamente."));
    }

    [HttpGet("{id_usuario:int}")]
    [Authorize(Roles = "ADMINISTRADOR,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioAppResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UsuarioAppResponseDto>>> GetById(int id_usuario)
    {
        var result = await _service.GetByIdAsync(id_usuario);
        if (result is null)
            return NotFound(ApiResponse<UsuarioAppResponseDto>.Fail("Usuario no encontrado."));

        if (!ClientePuedeAcceder(result.IdCliente))
            return Forbid();

        return Ok(ApiResponse<UsuarioAppResponseDto>.Ok(result, "Usuario obtenido correctamente."));
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioAppResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<UsuarioAppResponseDto>>> Create([FromBody] UsuarioAppRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _service.CreateAsync(request, usuario);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<UsuarioAppResponseDto>.Ok(result, "Usuario creado correctamente."));
    }

    [HttpPut("{id_usuario:int}")]
    [Authorize(Roles = "ADMINISTRADOR,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioAppResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UsuarioAppResponseDto>>> Update(int id_usuario, [FromBody] UsuarioAppUpdateRequestDto request)
    {
        var actual = await _service.GetByIdAsync(id_usuario);
        if (actual is null)
            return NotFound(ApiResponse<UsuarioAppResponseDto>.Fail("Usuario no encontrado."));

        if (!ClientePuedeAcceder(actual.IdCliente))
            return Forbid();

        var result = await _service.UpdateAsync(id_usuario, request, GetUsuario());
        if (result is null)
            return NotFound(ApiResponse<UsuarioAppResponseDto>.Fail("Usuario no encontrado."));

        return Ok(ApiResponse<UsuarioAppResponseDto>.Ok(result, "Usuario actualizado correctamente."));
    }

    [HttpDelete("{id_usuario:int}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id_usuario)
    {
        var usuario = GetUsuario();
        var result = await _service.DeleteAsync(id_usuario, usuario);
        return Ok(ApiResponse<bool>.Ok(result, "Usuario eliminado correctamente."));
    }

    private bool ClientePuedeAcceder(int? idClienteRecurso)
    {
        var rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
        if (rol != "CLIENTE")
            return true;

        var idClienteClaim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(idClienteClaim, out var idClienteToken) && idClienteRecurso == idClienteToken;
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
