using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class EquipajeDataMapper
{
    public static EquipajeDataModel ToDataModel(EquipajeEntity e) => new()
    {
        IdEquipaje = e.IdEquipaje,
        RowVersion = e.RowVersion,
        IdBoleto = e.IdBoleto,
        Tipo = e.Tipo,
        PesoKg = e.PesoKg,
        DescripcionEquipaje = e.DescripcionEquipaje,
        PrecioExtra = e.PrecioExtra,
        DimensionesCm = e.DimensionesCm,
        NumeroEtiqueta = e.NumeroEtiqueta,
        EstadoEquipaje = e.EstadoEquipaje,
        EsEliminado = e.EsEliminado,
        Estado = e.Estado,
        CreadoPorUsuario = e.CreadoPorUsuario,
        FechaRegistroUtc = e.FechaRegistroUtc,
        ModificadoPorUsuario = e.ModificadoPorUsuario,
        FechaModificacionUtc = e.FechaModificacionUtc,
        ModificacionIp = e.ModificacionIp
    };

    public static EquipajeEntity ToEntity(EquipajeDataModel m) => new()
    {
        IdEquipaje = m.IdEquipaje,
        RowVersion = m.RowVersion,
        IdBoleto = m.IdBoleto,
        Tipo = m.Tipo.Trim().ToUpperInvariant(),
        PesoKg = m.PesoKg,
        DescripcionEquipaje = string.IsNullOrWhiteSpace(m.DescripcionEquipaje) ? null : m.DescripcionEquipaje.Trim(),
        PrecioExtra = m.PrecioExtra,
        DimensionesCm = string.IsNullOrWhiteSpace(m.DimensionesCm) ? null : m.DimensionesCm.Trim(),
        NumeroEtiqueta = string.IsNullOrWhiteSpace(m.NumeroEtiqueta)
            ? $"EQ-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}"
            : m.NumeroEtiqueta.Trim().ToUpperInvariant(),
        EstadoEquipaje = string.IsNullOrWhiteSpace(m.EstadoEquipaje)
            ? "REGISTRADO"
            : m.EstadoEquipaje.Trim().ToUpperInvariant(),
        EsEliminado = m.EsEliminado,
        Estado = string.IsNullOrWhiteSpace(m.Estado)
            ? "ACTIVO"
            : m.Estado.Trim().ToUpperInvariant(),
        CreadoPorUsuario = m.CreadoPorUsuario.Trim(),
        FechaRegistroUtc = m.FechaRegistroUtc == default ? DateTime.UtcNow : m.FechaRegistroUtc,
        ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = m.FechaModificacionUtc,
        ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp) ? null : m.ModificacionIp.Trim()
    };

    public static void UpdateEntity(EquipajeEntity e, EquipajeDataModel m)
    {
        e.IdBoleto = m.IdBoleto;
        e.Tipo = m.Tipo.Trim().ToUpperInvariant();
        e.PesoKg = m.PesoKg;
        e.DescripcionEquipaje = string.IsNullOrWhiteSpace(m.DescripcionEquipaje) ? null : m.DescripcionEquipaje.Trim();
        e.PrecioExtra = m.PrecioExtra;
        e.DimensionesCm = string.IsNullOrWhiteSpace(m.DimensionesCm) ? null : m.DimensionesCm.Trim();
        e.NumeroEtiqueta = string.IsNullOrWhiteSpace(m.NumeroEtiqueta)
            ? e.NumeroEtiqueta
            : m.NumeroEtiqueta.Trim().ToUpperInvariant();
        e.EstadoEquipaje = string.IsNullOrWhiteSpace(m.EstadoEquipaje)
            ? e.EstadoEquipaje
            : m.EstadoEquipaje.Trim().ToUpperInvariant();
        e.Estado = string.IsNullOrWhiteSpace(m.Estado)
            ? e.Estado
            : m.Estado.Trim().ToUpperInvariant();
        e.ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario) ? null : m.ModificadoPorUsuario.Trim();
        e.FechaModificacionUtc = DateTime.UtcNow;
        e.ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp) ? null : m.ModificacionIp.Trim();
    }
}