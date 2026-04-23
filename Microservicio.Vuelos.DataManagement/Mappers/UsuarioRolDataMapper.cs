using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class UsuarioRolDataMapper
{
    public static UsuarioRolDataModel ToDataModel(UsuarioRolEntity e) => new()
    {
        IdUsuarioRol = e.IdUsuarioRol,
        IdUsuario = e.IdUsuario,
        IdRol = e.IdRol,
        EstadoUsuarioRol = e.EstadoUsuarioRol,
        EsEliminado = e.EsEliminado,
        Activo = e.Activo,
        CreadoPorUsuario = e.CreadoPorUsuario,
        FechaRegistroUtc = e.FechaRegistroUtc,
        ModificadoPorUsuario = e.ModificadoPorUsuario,
        FechaModificacionUtc = e.FechaModificacionUtc,
        RowVersion = e.RowVersion
    };

    public static UsuarioRolEntity ToEntity(UsuarioRolDataModel m) => new()
    {
        IdUsuarioRol = m.IdUsuarioRol,
        IdUsuario = m.IdUsuario,
        IdRol = m.IdRol,

        EstadoUsuarioRol = string.IsNullOrWhiteSpace(m.EstadoUsuarioRol)
            ? "ACTIVO"
            : m.EstadoUsuarioRol.Trim().ToUpperInvariant(),

        EsEliminado = m.EsEliminado,
        Activo = m.Activo,

        CreadoPorUsuario = m.CreadoPorUsuario.Trim(),

        FechaRegistroUtc = m.FechaRegistroUtc == default
            ? DateTime.UtcNow
            : m.FechaRegistroUtc,

        ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = m.FechaModificacionUtc,

        RowVersion = m.RowVersion
    };

    public static void UpdateEntity(UsuarioRolEntity e, UsuarioRolDataModel m)
    {
        e.EstadoUsuarioRol = m.EstadoUsuarioRol.Trim().ToUpperInvariant();
        e.Activo = m.Activo;

        e.ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim();
        e.FechaModificacionUtc = DateTime.UtcNow;
    }
}