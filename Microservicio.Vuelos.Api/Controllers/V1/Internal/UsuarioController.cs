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
[Authorize(Roles = "ADMINISTRADOR")] // ? M�dulo de seguridad: acceso exclusivo al ADMINISTRADOR
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioAppService _service;

    public UsuarioController(IUsuarioAppService service)
    {
        _service = service;
    }

    // GET PAGINADO ? Solo ADMINISTRADOR
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] UsuarioAppFilterDto filter)
    {
        var result = await _service.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de usuarios realizada correctamente."));
    }

    // POST ? Solo ADMINISTRADOR puede crear usuarios del sistema
    [HttpPost]
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
