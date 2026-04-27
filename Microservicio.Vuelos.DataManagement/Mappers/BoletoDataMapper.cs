using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class BoletoDataMapper
{
    public static BoletoDataModel ToDataModel(BoletoEntity entity) => new()
    {
        IdBoleto = entity.IdBoleto,
        RowVersion = entity.RowVersion,
        IdReserva = entity.IdReserva,
        IdVuelo = entity.IdVuelo,
        IdAsiento = entity.IdAsiento,
        IdFactura = entity.IdFactura,
        CodigoBoleto = entity.CodigoBoleto,
        Clase = entity.Clase,
        PrecioVueloBase = entity.PrecioVueloBase,
        PrecioAsientoExtra = entity.PrecioAsientoExtra,
        ImpuestosBoleto = entity.ImpuestosBoleto,
        CargoEquipaje = entity.CargoEquipaje,
        PrecioFinal = entity.PrecioFinal,
        EstadoBoleto = entity.EstadoBoleto,
        FechaEmision = entity.FechaEmision,
        EsEliminado = entity.EsEliminado,
        Estado = entity.Estado,
        CreadoPorUsuario = entity.CreadoPorUsuario,
        FechaRegistroUtc = entity.FechaRegistroUtc,
        ModificadoPorUsuario = entity.ModificadoPorUsuario,
        FechaModificacionUtc = entity.FechaModificacionUtc,
        ModificacionIp = entity.ModificacionIp
    };

    public static BoletoEntity ToEntity(BoletoDataModel model) => new()
    {
        IdBoleto = model.IdBoleto,
        RowVersion = model.RowVersion,
        IdReserva = model.IdReserva,
        IdVuelo = model.IdVuelo,
        IdAsiento = model.IdAsiento,
        IdFactura = model.IdFactura,
        CodigoBoleto = string.IsNullOrWhiteSpace(model.CodigoBoleto)
            ? $"BO-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}"
            : model.CodigoBoleto.Trim().ToUpperInvariant(),
        Clase = model.Clase.Trim().ToUpperInvariant(),
        PrecioVueloBase = model.PrecioVueloBase,
        PrecioAsientoExtra = model.PrecioAsientoExtra,
        ImpuestosBoleto = model.ImpuestosBoleto,
        CargoEquipaje = model.CargoEquipaje,
        PrecioFinal = model.PrecioFinal,
        EstadoBoleto = string.IsNullOrWhiteSpace(model.EstadoBoleto)
            ? "ACTIVO"
            : model.EstadoBoleto.Trim().ToUpperInvariant(),
        FechaEmision = model.FechaEmision == default
            ? DateTime.UtcNow
            : model.FechaEmision,
        EsEliminado = model.EsEliminado,
        Estado = string.IsNullOrWhiteSpace(model.Estado)
            ? "ACTIVO"
            : model.Estado.Trim().ToUpperInvariant(),
        CreadoPorUsuario = string.IsNullOrWhiteSpace(model.CreadoPorUsuario)
            ? "SYSTEM"
            : model.CreadoPorUsuario.Trim(),
        FechaRegistroUtc = model.FechaRegistroUtc == default
            ? DateTime.UtcNow
            : model.FechaRegistroUtc,
        ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario)
            ? null
            : model.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = model.FechaModificacionUtc,
        ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp)
            ? null
            : model.ModificacionIp.Trim()
    };

    public static void UpdateEntity(BoletoEntity entity, BoletoDataModel model)
    {
        entity.IdReserva = model.IdReserva;
        entity.IdVuelo = model.IdVuelo;
        entity.IdAsiento = model.IdAsiento;
        entity.IdFactura = model.IdFactura;
        entity.CodigoBoleto = model.CodigoBoleto.Trim().ToUpperInvariant();
        entity.Clase = model.Clase.Trim().ToUpperInvariant();
        entity.PrecioVueloBase = model.PrecioVueloBase;
        entity.PrecioAsientoExtra = model.PrecioAsientoExtra;
        entity.ImpuestosBoleto = model.ImpuestosBoleto;
        entity.CargoEquipaje = model.CargoEquipaje;
        entity.PrecioFinal = model.PrecioFinal;
        entity.EstadoBoleto = model.EstadoBoleto.Trim().ToUpperInvariant();
        entity.FechaEmision = model.FechaEmision;
        entity.Estado = model.Estado.Trim().ToUpperInvariant();
        entity.ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario)
            ? null
            : model.ModificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = model.FechaModificacionUtc;
        entity.ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp)
            ? null
            : model.ModificacionIp.Trim();
    }
}