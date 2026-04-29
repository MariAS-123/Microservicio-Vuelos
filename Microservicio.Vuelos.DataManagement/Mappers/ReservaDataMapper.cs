using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class ReservaDataMapper
{
    public static ReservaDataModel ToDataModel(ReservaEntity e) => new()
    {
        IdReserva = e.IdReserva,
        GuidReserva = e.GuidReserva,
        CodigoReserva = e.CodigoReserva,
        IdCliente = e.IdCliente,
        IdVuelo = e.IdVuelo,
        IdPasajero = e.Detalles
            .Where(d => !d.EsEliminado)
            .OrderBy(d => d.IdDetalle)
            .Select(d => d.IdPasajero)
            .FirstOrDefault(),
        IdAsiento = e.Detalles
            .Where(d => !d.EsEliminado)
            .OrderBy(d => d.IdDetalle)
            .Select(d => d.IdAsiento)
            .FirstOrDefault(),
        FechaReservaUtc = e.FechaReservaUtc,
        FechaInicio = e.FechaInicio,
        FechaFin = e.FechaFin,
        SubtotalReserva = e.SubtotalReserva,
        ValorIva = e.ValorIva,
        TotalReserva = e.TotalReserva,
        OrigenCanalReserva = e.OrigenCanalReserva,
        EstadoReserva = e.EstadoReserva,
        FechaConfirmacionUtc = e.FechaConfirmacionUtc,
        FechaCancelacionUtc = e.FechaCancelacionUtc,
        MotivoCancelacion = e.MotivoCancelacion,
        EsEliminado = e.EsEliminado,
        CreadoPorUsuario = e.CreadoPorUsuario,
        FechaRegistroUtc = e.FechaRegistroUtc,
        ModificadoPorUsuario = e.ModificadoPorUsuario,
        FechaModificacionUtc = e.FechaModificacionUtc,
        ModificacionIp = e.ModificacionIp,
        ServicioOrigen = e.ServicioOrigen,
        ContactoEmail = e.ContactoEmail,
        ContactoTelefono = e.ContactoTelefono,
        Observaciones = e.Observaciones,
        FechaInhabilitacionUtc = e.FechaInhabilitacionUtc,
        MotivoInhabilitacion = e.MotivoInhabilitacion,
        RowVersion = e.RowVersion,
        Detalles = e.Detalles
            .Where(d => !d.EsEliminado)
            .OrderBy(d => d.IdDetalle)
            .Select(ReservaDetalleDataMapper.ToDataModel)
            .ToList()
    };

    public static ReservaEntity ToEntity(ReservaDataModel m) => new()
    {
        IdReserva = m.IdReserva,
        GuidReserva = m.GuidReserva == Guid.Empty ? Guid.NewGuid() : m.GuidReserva,

        // Siempre se genera automáticamente en backend.
        CodigoReserva = $"VU-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}",

        IdCliente = m.IdCliente,
        IdVuelo = m.IdVuelo,
        FechaReservaUtc = m.FechaReservaUtc == default ? DateTime.UtcNow : m.FechaReservaUtc,
        FechaInicio = m.FechaInicio,
        FechaFin = m.FechaFin,
        SubtotalReserva = m.SubtotalReserva,
        ValorIva = m.ValorIva,
        TotalReserva = m.TotalReserva,

        // ✅ Default BOOKING si no viene
        OrigenCanalReserva = string.IsNullOrWhiteSpace(m.OrigenCanalReserva)
            ? "BOOKING"
            : m.OrigenCanalReserva.Trim(),

        EstadoReserva = string.IsNullOrWhiteSpace(m.EstadoReserva)
            ? "PEN"
            : m.EstadoReserva.Trim().ToUpperInvariant(),

        FechaConfirmacionUtc = m.FechaConfirmacionUtc,
        FechaCancelacionUtc = m.FechaCancelacionUtc,
        MotivoCancelacion = string.IsNullOrWhiteSpace(m.MotivoCancelacion)
            ? null
            : m.MotivoCancelacion.Trim(),

        EsEliminado = m.EsEliminado,

        // ✅ Default SYSTEM si no viene
        CreadoPorUsuario = string.IsNullOrWhiteSpace(m.CreadoPorUsuario)
            ? "SYSTEM"
            : m.CreadoPorUsuario.Trim(),

        FechaRegistroUtc = m.FechaRegistroUtc == default ? DateTime.UtcNow : m.FechaRegistroUtc,
        ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario)
            ? null
            : m.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = m.FechaModificacionUtc,
        ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp)
            ? null
            : m.ModificacionIp.Trim(),

        // ✅ Default VUELOS si no viene
        ServicioOrigen = string.IsNullOrWhiteSpace(m.ServicioOrigen)
            ? "VUELOS"
            : m.ServicioOrigen.Trim(),

        ContactoEmail = string.IsNullOrWhiteSpace(m.ContactoEmail)
            ? null
            : m.ContactoEmail.Trim().ToLowerInvariant(),
        ContactoTelefono = string.IsNullOrWhiteSpace(m.ContactoTelefono)
            ? null
            : m.ContactoTelefono.Trim(),
        Observaciones = string.IsNullOrWhiteSpace(m.Observaciones)
            ? null
            : m.Observaciones.Trim(),
        FechaInhabilitacionUtc = m.FechaInhabilitacionUtc,
        MotivoInhabilitacion = m.MotivoInhabilitacion,
        RowVersion = m.RowVersion,
        Detalles = BuildDetalles(m)
    };

    public static void UpdateEntity(ReservaEntity e, ReservaDataModel m)
    {
        e.IdCliente = m.IdCliente;
        e.IdVuelo = m.IdVuelo;
        e.FechaInicio = m.FechaInicio;
        e.FechaFin = m.FechaFin;
        e.SubtotalReserva = m.SubtotalReserva;
        e.ValorIva = m.ValorIva;
        e.TotalReserva = m.TotalReserva;

        e.OrigenCanalReserva = string.IsNullOrWhiteSpace(m.OrigenCanalReserva)
            ? e.OrigenCanalReserva
            : m.OrigenCanalReserva.Trim();

        e.EstadoReserva = string.IsNullOrWhiteSpace(m.EstadoReserva)
            ? e.EstadoReserva
            : m.EstadoReserva.Trim().ToUpperInvariant();

        e.FechaConfirmacionUtc = m.FechaConfirmacionUtc;
        e.FechaCancelacionUtc = m.FechaCancelacionUtc;
        e.MotivoCancelacion = string.IsNullOrWhiteSpace(m.MotivoCancelacion)
            ? null
            : m.MotivoCancelacion.Trim();

        e.ContactoEmail = string.IsNullOrWhiteSpace(m.ContactoEmail)
            ? null
            : m.ContactoEmail.Trim().ToLowerInvariant();

        e.ContactoTelefono = string.IsNullOrWhiteSpace(m.ContactoTelefono)
            ? null
            : m.ContactoTelefono.Trim();

        e.Observaciones = string.IsNullOrWhiteSpace(m.Observaciones)
            ? null
            : m.Observaciones.Trim();

        e.ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario)
            ? null
            : m.ModificadoPorUsuario.Trim();

        e.FechaModificacionUtc = DateTime.UtcNow;

        e.ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp)
            ? null
            : m.ModificacionIp.Trim();

    }

    private static List<ReservaDetalleEntity> BuildDetalles(ReservaDataModel model)
    {
        var detalles = model.Detalles
            .Where(d => d.IdPasajero > 0 && d.IdAsiento > 0)
            .Select(ReservaDetalleDataMapper.ToEntity)
            .ToList();

        if (detalles.Count > 0)
            return detalles;

        if (model.IdPasajero > 0 && model.IdAsiento > 0)
        {
            return
            [
                new ReservaDetalleEntity
                {
                    IdPasajero = model.IdPasajero,
                    IdAsiento = model.IdAsiento,
                    SubtotalLinea = model.SubtotalReserva,
                    ValorIvaLinea = model.ValorIva,
                    TotalLinea = model.TotalReserva,
                    Estado = "ACTIVO",
                    EsEliminado = false,
                    CreadoPorUsuario = string.IsNullOrWhiteSpace(model.CreadoPorUsuario)
                        ? "SYSTEM"
                        : model.CreadoPorUsuario.Trim(),
                    FechaRegistroUtc = model.FechaRegistroUtc == default ? DateTime.UtcNow : model.FechaRegistroUtc
                }
            ];
        }

        return [];
    }
}
