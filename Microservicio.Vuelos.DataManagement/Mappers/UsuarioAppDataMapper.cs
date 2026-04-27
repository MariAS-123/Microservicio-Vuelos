using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class UsuarioAppDataMapper
{
    public static UsuarioAppDataModel ToDataModel(UsuarioAppEntity e) => new()
    {
        IdUsuario = e.IdUsuario,
        UsuarioGuid = e.UsuarioGuid,
        IdCliente = e.IdCliente,
        Username = e.Username,
        Correo = e.Correo,
        PasswordHash = e.PasswordHash,
        PasswordSalt = e.PasswordSalt,
        FechaUltimoLogin = e.FechaUltimoLogin,
        EstadoUsuario = e.EstadoUsuario,
        EsEliminado = e.EsEliminado,
        Activo = e.Activo,
        CreadoPorUsuario = e.CreadoPorUsuario,
        FechaRegistroUtc = e.FechaRegistroUtc,
        ModificadoPorUsuario = e.ModificadoPorUsuario,
        FechaModificacionUtc = e.FechaModificacionUtc,
        ModificacionIp = e.ModificacionIp,
        RowVersion = e.RowVersion,
        Roles = e.UsuariosRoles?
            .Where(ur => ur.Activo &&
                         !ur.EsEliminado &&
                         ur.EstadoUsuarioRol == "ACT" &&
                         ur.Rol != null &&
                         ur.Rol.Activo &&
                         !ur.Rol.EsEliminado &&
                         ur.Rol.EstadoRol == "ACT")
            .Select(ur => ur.Rol.NombreRol)
            .Distinct()
            .ToList() ?? new List<string>()
    };

    public static UsuarioAppEntity ToEntity(UsuarioAppDataModel m) => new()
    {
        IdUsuario = m.IdUsuario,

        UsuarioGuid = m.UsuarioGuid == Guid.Empty
            ? Guid.NewGuid()
            : m.UsuarioGuid,

        IdCliente = m.IdCliente,

        Username = m.Username.Trim(),
        Correo = m.Correo.Trim().ToUpperInvariant(),

        PasswordHash = m.PasswordHash,
        PasswordSalt = m.PasswordSalt,

        FechaUltimoLogin = m.FechaUltimoLogin,

        EstadoUsuario = string.IsNullOrWhiteSpace(m.EstadoUsuario)
            ? "ACT"
            : m.EstadoUsuario.Trim().ToUpperInvariant(),

        EsEliminado = m.EsEliminado,
        Activo = m.Activo,

        CreadoPorUsuario = m.CreadoPorUsuario.Trim(),

        FechaRegistroUtc = m.FechaRegistroUtc == default
            ? DateTime.UtcNow
            : m.FechaRegistroUtc,

        ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = m.FechaModificacionUtc,
        ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp) ? null : m.ModificacionIp.Trim(),

        RowVersion = m.RowVersion
    };

    public static void UpdateEntity(UsuarioAppEntity e, UsuarioAppDataModel m)
    {
        e.IdCliente = m.IdCliente;

        e.Username = m.Username.Trim();
        e.Correo = m.Correo.Trim().ToUpperInvariant();

        if (!string.IsNullOrWhiteSpace(m.PasswordHash))
            e.PasswordHash = m.PasswordHash;

        if (!string.IsNullOrWhiteSpace(m.PasswordSalt))
            e.PasswordSalt = m.PasswordSalt;

        e.FechaUltimoLogin = m.FechaUltimoLogin;

        e.EstadoUsuario = m.EstadoUsuario.Trim().ToUpperInvariant();
        e.Activo = m.Activo;

        e.ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim();
        e.FechaModificacionUtc = DateTime.UtcNow;
        e.ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp) ? null : m.ModificacionIp.Trim();
    }
}