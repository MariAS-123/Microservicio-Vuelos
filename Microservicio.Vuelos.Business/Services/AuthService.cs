using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microservicio.Vuelos.Business.DTOs.Auth;
using Microservicio.Vuelos.Business.DTOs.Cliente;
using Microservicio.Vuelos.Business.DTOs.UsuarioApp;
using Microservicio.Vuelos.Business.DTOs.UsuarioRol;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioAppDataService _usuarioDataService;
    private readonly IClienteService _clienteService;
    private readonly IUsuarioAppService _usuarioAppService;
    private readonly IUsuarioRolService _usuarioRolService;
    private readonly IRolDataService _rolDataService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthValidator _validator;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUsuarioAppDataService usuarioDataService,
        IClienteService clienteService,
        IUsuarioAppService usuarioAppService,
        IUsuarioRolService usuarioRolService,
        IRolDataService rolDataService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _usuarioDataService = usuarioDataService;
        _clienteService = clienteService;
        _usuarioAppService = usuarioAppService;
        _usuarioRolService = usuarioRolService;
        _rolDataService = rolDataService;
        _unitOfWork = unitOfWork;
        _validator = new AuthValidator();
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        _validator.ValidateLogin(request);

        var user = await _usuarioDataService.GetByUsernameAsync(request.Username);

        if (user == null)
            throw new UnauthorizedBusinessException("Usuario o contrasena incorrectos.");

        if (!user.Activo || user.EsEliminado || user.EstadoUsuario != "ACT")
            throw new UnauthorizedBusinessException("El usuario no esta activo.");

        if (!IsPasswordValid(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new UnauthorizedBusinessException("Usuario o contrasena incorrectos.");

        var expirationMinutes = _configuration.GetValue<int>("JwtSettings:ExpirationMinutes");
        var expiracion = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = GenerateJwtToken(user.Username, user.Roles, user.IdCliente, expiracion);

        return AuthBusinessMapper.ToLoginResponse(user, token, expiracion);
    }

    public async Task<RegisterClienteResponse> RegisterClienteAsync(RegisterClienteRequest request)
    {
        _validator.ValidateRegisterCliente(request);

        var rolCliente = await _rolDataService.GetByNombreAsync("CLIENTE");
        if (rolCliente == null || rolCliente.EsEliminado || !rolCliente.Activo || rolCliente.EstadoRol != "ACT")
            throw new BusinessException("No se encontro el rol CLIENTE activo para registrar la cuenta.");

        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var cliente = await _clienteService.CreateAsync(new ClienteRequestDto
            {
                TipoIdentificacion = request.TipoIdentificacion,
                NumeroIdentificacion = request.NumeroIdentificacion,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                RazonSocial = request.RazonSocial,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                IdCiudadResidencia = request.IdCiudadResidencia,
                IdPaisNacionalidad = request.IdPaisNacionalidad,
                FechaNacimiento = request.FechaNacimiento,
                Genero = request.Genero
            }, "SELF_REGISTER");

            var usuario = await _usuarioAppService.CreateAsync(new UsuarioAppRequestDto
            {
                IdCliente = cliente.IdCliente,
                Username = request.Username,
                Correo = request.Correo,
                Password = request.Password
            }, "SELF_REGISTER");

            await _usuarioRolService.CreateAsync(new UsuarioRolRequestDto
            {
                IdUsuario = usuario.IdUsuario,
                IdRol = rolCliente.IdRol
            }, "SELF_REGISTER");

            return new RegisterClienteResponse
            {
                IdCliente = cliente.IdCliente,
                IdUsuario = usuario.IdUsuario,
                Username = usuario.Username,
                RolAsignado = "CLIENTE"
            };
        });
    }

    public Task LogoutAsync(string token, string ejecutadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new UnauthorizedBusinessException("No se recibio un token valido para cerrar sesion.");

        if (string.IsNullOrWhiteSpace(ejecutadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar al usuario autenticado.");

        return Task.CompletedTask;
    }

    private string GenerateJwtToken(string username, List<string> roles, int? idCliente, DateTime expiracion)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new BusinessException("No se encontro JwtSettings:SecretKey en la configuracion.");

        if (string.IsNullOrWhiteSpace(issuer))
            throw new BusinessException("No se encontro JwtSettings:Issuer en la configuracion.");

        if (string.IsNullOrWhiteSpace(audience))
            throw new BusinessException("No se encontro JwtSettings:Audience en la configuracion.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new("username", username)
        };

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

    private static bool IsPasswordValid(string password, string storedHash, string? storedSalt)
    {
        if (string.Equals(storedHash, password, StringComparison.Ordinal))
            return true;

        if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
            return false;

        try
        {
            var hashBytes = Convert.FromBase64String(storedHash);
            var saltBytes = Convert.FromBase64String(storedSalt);

            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combined = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combined, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combined, passwordBytes.Length, saltBytes.Length);
            var computed = sha256.ComputeHash(combined);

            return CryptographicOperations.FixedTimeEquals(computed, hashBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
