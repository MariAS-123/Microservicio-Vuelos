using System;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class FacturaDataMapper
{
    public static FacturaDataModel ToDataModel(FacturaEntity e) => new()
    {
        IdFactura = e.IdFactura,
        GuidFactura = e.GuidFactura,
        IdCliente = e.IdCliente,
        IdReserva = e.IdReserva,
        NumeroFactura = e.NumeroFactura,
        FechaEmision = e.FechaEmision,
        Subtotal = e.Subtotal,
        ValorIva = e.ValorIva,
        CargoServicio = e.CargoServicio,
        Total = e.Total,
        ObservacionesFactura = e.ObservacionesFactura,
        OrigenCanalFactura = e.OrigenCanalFactura,
        Estado = e.Estado,
        FechaInhabilitacionUtc = e.FechaInhabilitacionUtc,
        EsEliminado = e.EsEliminado,
        CreadoPorUsuario = e.CreadoPorUsuario,
        FechaRegistroUtc = e.FechaRegistroUtc,
        ModificadoPorUsuario = e.ModificadoPorUsuario,
        FechaModificacionUtc = e.FechaModificacionUtc,
        ModificacionIp = e.ModificacionIp,
        ServicioOrigen = e.ServicioOrigen,
        MotivoInhabilitacion = e.MotivoInhabilitacion,
        RowVersion = e.RowVersion
    };

    public static FacturaEntity ToEntity(FacturaDataModel m) => new()
    {
        IdFactura = m.IdFactura,

        GuidFactura = m.GuidFactura == Guid.Empty
            ? Guid.NewGuid()
            : m.GuidFactura,

        IdCliente = m.IdCliente,
        IdReserva = m.IdReserva,

        NumeroFactura = string.IsNullOrWhiteSpace(m.NumeroFactura)
            ? $"FA-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}"
            : m.NumeroFactura.Trim().ToUpperInvariant(),

        FechaEmision = m.FechaEmision == default
            ? DateTime.UtcNow
            : m.FechaEmision,

        Subtotal = m.Subtotal,
        ValorIva = m.ValorIva,
        CargoServicio = m.CargoServicio,
        Total = m.Total,

        ObservacionesFactura = string.IsNullOrWhiteSpace(m.ObservacionesFactura)
            ? null
            : m.ObservacionesFactura.Trim(),

        OrigenCanalFactura = string.IsNullOrWhiteSpace(m.OrigenCanalFactura)
            ? null
            : m.OrigenCanalFactura.Trim(),

        // 🔥 ESTADO CORRECTO SEGÚN BD
        Estado = string.IsNullOrWhiteSpace(m.Estado)
            ? "ABI"
            : m.Estado.Trim().ToUpperInvariant(),

        FechaInhabilitacionUtc = m.FechaInhabilitacionUtc,

        EsEliminado = m.EsEliminado,

        CreadoPorUsuario = string.IsNullOrWhiteSpace(m.CreadoPorUsuario)
            ? "SYSTEM"
            : m.CreadoPorUsuario.Trim(),

        FechaRegistroUtc = m.FechaRegistroUtc == default
            ? DateTime.UtcNow
            : m.FechaRegistroUtc,

        ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario)
            ? null
            : m.ModificadoPorUsuario.Trim(),

        FechaModificacionUtc = m.FechaModificacionUtc,

        ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp)
            ? null
            : m.ModificacionIp.Trim(),

        ServicioOrigen = string.IsNullOrWhiteSpace(m.ServicioOrigen)
            ? "VUELOS"
            : m.ServicioOrigen.Trim(),

        MotivoInhabilitacion = m.MotivoInhabilitacion,

        RowVersion = m.RowVersion
    };

    public static void UpdateEntity(FacturaEntity e, FacturaDataModel m)
    {
        e.IdCliente = m.IdCliente;
        e.IdReserva = m.IdReserva;

        e.NumeroFactura = string.IsNullOrWhiteSpace(m.NumeroFactura)
            ? e.NumeroFactura
            : m.NumeroFactura.Trim().ToUpperInvariant();

        e.FechaEmision = m.FechaEmision;

        e.Subtotal = m.Subtotal;
        e.ValorIva = m.ValorIva;
        e.CargoServicio = m.CargoServicio;
        e.Total = m.Total;

        e.ObservacionesFactura = string.IsNullOrWhiteSpace(m.ObservacionesFactura)
            ? null
            : m.ObservacionesFactura.Trim();

        e.OrigenCanalFactura = string.IsNullOrWhiteSpace(m.OrigenCanalFactura)
            ? null
            : m.OrigenCanalFactura.Trim();

        // 🔥 SOLO VALORES VÁLIDOS
        e.Estado = m.Estado.Trim().ToUpperInvariant();

        e.ModificadoPorUsuario = string.IsNullOrWhiteSpace(m.ModificadoPorUsuario)
            ? e.ModificadoPorUsuario
            : m.ModificadoPorUsuario.Trim();

        e.FechaModificacionUtc = DateTime.UtcNow;

        e.ModificacionIp = string.IsNullOrWhiteSpace(m.ModificacionIp)
            ? null
            : m.ModificacionIp.Trim();

        e.MotivoInhabilitacion = m.MotivoInhabilitacion;
    }
}