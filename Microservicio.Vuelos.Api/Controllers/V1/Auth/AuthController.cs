using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Api.Security;
using Microservicio.Vuelos.Business.DTOs.Auth;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenBlacklistService _tokenBlacklistService;

    public AuthController(IAuthService authService, ITokenBlacklistService tokenBlacklistService)
    {
        _authService = authService;
        _tokenBlacklistService = tokenBlacklistService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(ApiResponse<LoginResponse>.Ok(result, "Login exitoso."));
    }

    [HttpPost("register-cliente")]
    [AllowAnonymous]
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

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        var authorization = Request.Headers.Authorization.ToString();
        var token = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? authorization["Bearer ".Length..].Trim()
            : string.Empty;

        var usuario = User?.Identity?.Name ?? User?.FindFirst("username")?.Value ?? "SYSTEM";
        await _authService.LogoutAsync(token, usuario);
        _tokenBlacklistService.Blacklist(token, GetExpirationUtc(token));

        return Ok(ApiResponse<object>.Ok(null, "Sesion cerrada correctamente."));
    }

    private static DateTimeOffset GetExpirationUtc(string token)
    {
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            if (jwt.ValidTo != DateTime.MinValue)
                return new DateTimeOffset(DateTime.SpecifyKind(jwt.ValidTo, DateTimeKind.Utc));
        }
        catch
        {
        }

        return DateTimeOffset.UtcNow.AddDays(1);
    }
}
