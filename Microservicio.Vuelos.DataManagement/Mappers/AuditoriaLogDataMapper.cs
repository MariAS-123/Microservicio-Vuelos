using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class AuditoriaLogDataMapper
{
    public static AuditoriaLogDataModel ToDataModel(AuditoriaLogEntity e) => new()
    {
        IdAuditoria = e.IdAuditoria,
        AuditoriaGuid = e.AuditoriaGuid,
        TablaAfectada = e.TablaAfectada,
        Operacion = e.Operacion,
        IdRegistroAfectado = e.IdRegistroAfectado,
        DatosAnteriores = e.DatosAnteriores,
        DatosNuevos = e.DatosNuevos,
        UsuarioEjecutor = e.UsuarioEjecutor,
        IpOrigen = e.IpOrigen,
        FechaEventoUtc = e.FechaEventoUtc,
        Activo = e.Activo,
        RowVersion = e.RowVersion
    };

    public static AuditoriaLogEntity ToEntity(AuditoriaLogDataModel m) => new()
    {
        IdAuditoria = m.IdAuditoria,

        AuditoriaGuid = m.AuditoriaGuid == Guid.Empty
            ? Guid.NewGuid()
            : m.AuditoriaGuid,

        TablaAfectada = m.TablaAfectada.Trim().ToUpperInvariant(),
        Operacion = m.Operacion.Trim().ToUpperInvariant(),

        IdRegistroAfectado = string.IsNullOrWhiteSpace(m.IdRegistroAfectado) ? null : m.IdRegistroAfectado.Trim(),

        DatosAnteriores = m.DatosAnteriores,
        DatosNuevos = m.DatosNuevos,

        UsuarioEjecutor = m.UsuarioEjecutor.Trim().ToUpperInvariant(),

        IpOrigen = string.IsNullOrWhiteSpace(m.IpOrigen) ? null : m.IpOrigen.Trim(),

        FechaEventoUtc = m.FechaEventoUtc == default
            ? DateTime.UtcNow
            : m.FechaEventoUtc,

        Activo = true,

        RowVersion = m.RowVersion
    };

    public static void UpdateEntity(AuditoriaLogEntity e, AuditoriaLogDataModel m)
    {
        // ⚠️ Auditoría NO debería modificarse normalmente,
        // pero dejamos esto por consistencia técnica

        e.TablaAfectada = m.TablaAfectada.Trim().ToUpperInvariant();
        e.Operacion = m.Operacion.Trim().ToUpperInvariant();
        e.IdRegistroAfectado = m.IdRegistroAfectado;
        e.DatosAnteriores = m.DatosAnteriores;
        e.DatosNuevos = m.DatosNuevos;
        e.UsuarioEjecutor = m.UsuarioEjecutor.Trim().ToUpperInvariant();
        e.IpOrigen = m.IpOrigen;
        e.FechaEventoUtc = m.FechaEventoUtc;
    }
}