using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Auth;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Produces("application/json")]
[AllowAnonymous] // ✅ Intencional: este controller genera el token, no requiere autenticación previa
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(ApiResponse<LoginResponse>.Ok(result, "Login exitoso."));
    }

    [HttpPost("register-cliente")]
    [ProducesResponseType(typeof(ApiResponse<RegisterClienteResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<RegisterClienteResponse>>> RegisterCliente([FromBody] RegisterClienteRequest request)
    {
        var result = await _authService.RegisterClienteAsync(request);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<RegisterClienteResponse>.Ok(result, "Cuenta de cliente creada correctamente."));
    }
}