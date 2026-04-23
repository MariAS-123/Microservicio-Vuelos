using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.UsuarioApp;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class UsuarioAppService : IUsuarioAppService
{
    private readonly IUsuarioAppDataService _usuarioAppDataService;
    private readonly IClienteDataService _clienteDataService;
    private readonly UsuarioAppValidator _validator;

    public UsuarioAppService(
        IUsuarioAppDataService usuarioAppDataService,
        IClienteDataService clienteDataService)
    {
        _usuarioAppDataService = usuarioAppDataService;
        _clienteDataService = clienteDataService;
        _validator = new UsuarioAppValidator();
    }

    public async Task<DataPagedResult<UsuarioAppResponseDto>> GetPagedAsync(UsuarioAppFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = UsuarioAppBusinessMapper.ToFiltroDataModel(filter);
        var result = await _usuarioAppDataService.GetPagedAsync(filtro);

        return new DataPagedResult<UsuarioAppResponseDto>
        {
            Items = UsuarioAppBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<UsuarioAppResponseDto?> GetByIdAsync(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new ValidationException("El id del usuario debe ser mayor que 0.");

        var data = await _usuarioAppDataService.GetByIdAsync(idUsuario);

        return data == null ? null : UsuarioAppBusinessMapper.ToResponseDto(data);
    }

    public async Task<UsuarioAppResponseDto> CreateAsync(UsuarioAppRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        if (request.IdCliente.HasValue)
        {
            var cliente = await _clienteDataService.GetByIdAsync(request.IdCliente.Value);
            if (cliente == null)
                throw new NotFoundException("El cliente indicado no existe.");
        }

        var existentes = await _usuarioAppDataService.GetPagedAsync(new UsuarioAppFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var username = request.Username.Trim().ToUpperInvariant();
        var correo = request.Correo.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.Username.Trim().ToUpperInvariant() == username))
            throw new BusinessException("Ya existe un usuario con el mismo username.");

        if (existentes.Items.Any(x => x.Correo.Trim().ToUpperInvariant() == correo))
            throw new BusinessException("Ya existe un usuario con el mismo correo.");

        var passwordSalt = GenerateSalt();
        var passwordHash = GenerateHash(request.Password, passwordSalt);

        var dataModel = UsuarioAppBusinessMapper.ToDataModel(
            request,
            creadoPorUsuario,
            passwordHash,
            passwordSalt);

        var creado = await _usuarioAppDataService.CreateAsync(dataModel);

        return UsuarioAppBusinessMapper.ToResponseDto(creado);
    }

    public async Task<UsuarioAppResponseDto?> UpdateAsync(int idUsuario, UsuarioAppUpdateRequestDto request, string modificadoPorUsuario)
    {
        if (idUsuario <= 0)
            throw new ValidationException("El id del usuario debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _usuarioAppDataService.GetByIdAsync(idUsuario);
        if (actual == null)
            throw new NotFoundException("Usuario no encontrado.");

        if (request.IdCliente.HasValue)
        {
            var cliente = await _clienteDataService.GetByIdAsync(request.IdCliente.Value);
            if (cliente == null)
                throw new NotFoundException("El cliente indicado no existe.");
        }

        var existentes = await _usuarioAppDataService.GetPagedAsync(new UsuarioAppFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var username = request.Username.Trim().ToUpperInvariant();
        var correo = request.Correo.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.IdUsuario != idUsuario &&
                                      x.Username.Trim().ToUpperInvariant() == username))
        {
            throw new BusinessException("Ya existe otro usuario con el mismo username.");
        }

        if (existentes.Items.Any(x => x.IdUsuario != idUsuario &&
                                      x.Correo.Trim().ToUpperInvariant() == correo))
        {
            throw new BusinessException("Ya existe otro usuario con el mismo correo.");
        }

        byte[]? passwordHash = null;
        byte[]? passwordSalt = null;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            passwordSalt = GenerateSalt();
            passwordHash = GenerateHash(request.Password, passwordSalt);
        }

        var dataModel = UsuarioAppBusinessMapper.ToDataModel(
            idUsuario,
            request,
            modificadoPorUsuario,
            passwordHash,
            passwordSalt);

        if (passwordHash == null)
            dataModel.PasswordHash = actual.PasswordHash;

        if (passwordSalt == null)
            dataModel.PasswordSalt = actual.PasswordSalt;

        var actualizado = await _usuarioAppDataService.UpdateAsync(dataModel);

        return actualizado == null ? null : UsuarioAppBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idUsuario, string modificadoPorUsuario)
    {
        if (idUsuario <= 0)
            throw new ValidationException("El id del usuario debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _usuarioAppDataService.GetByIdAsync(idUsuario);
        if (actual == null)
            throw new NotFoundException("Usuario no encontrado.");

        return await _usuarioAppDataService.DeleteAsync(idUsuario, modificadoPorUsuario);
    }

    private static byte[] GenerateSalt()
    {
        var salt = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static byte[] GenerateHash(string password, byte[] salt)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var combined = new byte[passwordBytes.Length + salt.Length];

        Buffer.BlockCopy(passwordBytes, 0, combined, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, combined, passwordBytes.Length, salt.Length);

        return sha256.ComputeHash(combined);
    }
}