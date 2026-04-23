using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microservicio.Vuelos.Business.DTOs.Auth;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioAppDataService _usuarioDataService;
    private readonly AuthValidator _validator;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUsuarioAppDataService usuarioDataService,
        IConfiguration configuration)
    {
        _usuarioDataService = usuarioDataService;
        _validator = new AuthValidator();
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        _validator.ValidateLogin(request);

        var user = await _usuarioDataService.GetByUsernameAsync(request.Usuario);

        if (user == null)
            throw new UnauthorizedBusinessException("Usuario o contraseña incorrectos.");

        if (!user.Activo || user.EsEliminado || user.EstadoUsuario != "ACT")
            throw new UnauthorizedBusinessException("El usuario no está activo.");

        if (!string.Equals(user.PasswordHash, request.Password, StringComparison.Ordinal))
            throw new UnauthorizedBusinessException("Usuario o contraseña incorrectos.");

        var expirationMinutes = _configuration.GetValue<int>("JwtSettings:ExpirationMinutes");
        var expiracion = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = GenerateJwtToken(user.Username, user.Roles, user.IdCliente, expiracion); // ✅

        return AuthBusinessMapper.ToLoginResponse(user, token, expiracion);
    }

    private string GenerateJwtToken(string username, List<string> roles, int? idCliente, DateTime expiracion) // ✅
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new BusinessException("No se encontró JwtSettings:SecretKey en la configuración.");

        if (string.IsNullOrWhiteSpace(issuer))
            throw new BusinessException("No se encontró JwtSettings:Issuer en la configuración.");

        if (string.IsNullOrWhiteSpace(audience))
            throw new BusinessException("No se encontró JwtSettings:Audience en la configuración.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new("username", username)
        };

        // ✅ Incluir id_cliente en el token si el usuario tiene cliente asociado
        if (idCliente.HasValue)
            claims.Add(new Claim("id_cliente", idCliente.Value.ToString()));

        foreach (var role in roles.Distinct())
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiracion,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}