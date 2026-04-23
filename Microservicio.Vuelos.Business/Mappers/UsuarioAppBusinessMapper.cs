using Microservicio.Vuelos.Business.DTOs.UsuarioApp;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class UsuarioAppBusinessMapper
{
    public static UsuarioAppFiltroDataModel ToFiltroDataModel(UsuarioAppFilterDto dto)
    {
        return new UsuarioAppFiltroDataModel
        {
            Username = dto.Username,
            Correo = dto.Correo,
            Activo = dto.Activo,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static UsuarioAppDataModel ToDataModel(
        UsuarioAppRequestDto dto,
        string creadoPorUsuario,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        return new UsuarioAppDataModel
        {
            IdCliente = dto.IdCliente,
            Username = dto.Username,
            Correo = dto.Correo,
            PasswordHash = Convert.ToBase64String(passwordHash),
            PasswordSalt = Convert.ToBase64String(passwordSalt),
            EstadoUsuario = "ACT",
            Activo = true,
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario
        };
    }

    public static UsuarioAppDataModel ToDataModel(
        int idUsuario,
        UsuarioAppUpdateRequestDto dto,
        string modificadoPorUsuario,
        byte[]? passwordHash = null,
        byte[]? passwordSalt = null)
    {
        return new UsuarioAppDataModel
        {
            IdUsuario = idUsuario,
            IdCliente = dto.IdCliente,
            Username = dto.Username,
            Correo = dto.Correo,
            PasswordHash = passwordHash != null ? Convert.ToBase64String(passwordHash) : string.Empty,
            PasswordSalt = passwordSalt != null ? Convert.ToBase64String(passwordSalt) : string.Empty,
            EstadoUsuario = "ACT",
            Activo = true,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static UsuarioAppResponseDto ToResponseDto(UsuarioAppDataModel model)
    {
        return new UsuarioAppResponseDto
        {
            IdUsuario = model.IdUsuario,
            UsuarioGuid = model.UsuarioGuid,
            IdCliente = model.IdCliente,
            Username = model.Username,
            Correo = model.Correo,
            FechaUltimoLogin = model.FechaUltimoLogin,
            EstadoUsuario = model.EstadoUsuario,
            Activo = model.Activo
        };
    }

    public static List<UsuarioAppResponseDto> ToResponseDtoList(IEnumerable<UsuarioAppDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}