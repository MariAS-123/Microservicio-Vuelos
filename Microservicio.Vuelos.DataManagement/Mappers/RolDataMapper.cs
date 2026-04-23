using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class RolDataMapper
{
    public static RolDataModel ToDataModel(RolEntity e) => new()
    {
        IdRol = e.IdRol,
        RolGuid = e.RolGuid,
        NombreRol = e.NombreRol,
        DescripcionRol = e.DescripcionRol,
        EstadoRol = e.EstadoRol,
        EsEliminado = e.EsEliminado,
        Activo = e.Activo,
        CreadoPorUsuario = e.CreadoPorUsuario,
        FechaRegistroUtc = e.FechaRegistroUtc,
        ModificadoPorUsuario = e.ModificadoPorUsuario,
        FechaModificacionUtc = e.FechaModificacionUtc,
        RowVersion = e.RowVersion
    };

    public static RolEntity ToEntity(RolDataModel m) => new()
    {
        IdRol = m.IdRol,
        RolGuid = m.RolGuid == Guid.Empty ? Guid.NewGuid() : m.RolGuid,
        NombreRol = m.NombreRol.Trim().ToUpperInvariant(),
        DescripcionRol = string.IsNullOrWhiteSpace(m.DescripcionRol) ? null : m.DescripcionRol.Trim(),
        EstadoRol = string.IsNullOrWhiteSpace(m.EstadoRol) ? "ACT" : m.EstadoRol.Trim().ToUpperInvariant(),
        EsEliminado = m.EsEliminado,
        Activo = m.Activo,
        CreadoPorUsuario = m.CreadoPorUsuario.Trim(),
        FechaRegistroUtc = m.FechaRegistroUtc == default ? DateTime.UtcNow : m.FechaRegistroUtc,
        ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = m.FechaModificacionUtc,
        RowVersion = m.RowVersion
    };

    public static void UpdateEntity(RolEntity e, RolDataModel m)
    {
        e.NombreRol = m.NombreRol.Trim().ToUpperInvariant();
        e.DescripcionRol = string.IsNullOrWhiteSpace(m.DescripcionRol) ? null : m.DescripcionRol.Trim();
        e.EstadoRol = m.EstadoRol.Trim().ToUpperInvariant();
        e.Activo = m.Activo;
        e.ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim();
        e.FechaModificacionUtc = DateTime.UtcNow;
    }
}