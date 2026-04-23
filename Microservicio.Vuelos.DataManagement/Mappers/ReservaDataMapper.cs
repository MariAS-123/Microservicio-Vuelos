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
        IdPasajero = e.IdPasajero,
        IdVuelo = e.IdVuelo,
        IdAsiento = e.IdAsiento,
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
        RowVersion = e.RowVersion
    };

    public static ReservaEntity ToEntity(ReservaDataModel m) => new()
    {
        IdReserva = m.IdReserva,
        GuidReserva = m.GuidReserva == Guid.Empty ? Guid.NewGuid() : m.GuidReserva,

        // ✅ Generar código automáticamente si no viene
        CodigoReserva = string.IsNullOrWhiteSpace(m.CodigoReserva)
            ? $"VU-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}"
            : m.CodigoReserva.Trim().ToUpperInvariant(),

        IdCliente = m.IdCliente,
        IdPasajero = m.IdPasajero,
        IdVuelo = m.IdVuelo,
        IdAsiento = m.IdAsiento,
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
        RowVersion = m.RowVersion
    };

    public static void UpdateEntity(ReservaEntity e, ReservaDataModel m)
    {
        e.CodigoReserva = string.IsNullOrWhiteSpace(m.CodigoReserva)
            ? e.CodigoReserva
            : m.CodigoReserva.Trim().ToUpperInvariant();

        e.IdCliente = m.IdCliente;
        e.IdPasajero = m.IdPasajero;
        e.IdVuelo = m.IdVuelo;
        e.IdAsiento = m.IdAsiento;
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
}