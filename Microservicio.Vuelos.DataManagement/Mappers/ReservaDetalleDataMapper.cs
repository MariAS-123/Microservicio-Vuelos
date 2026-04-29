using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class ReservaDetalleDataMapper
{
    public static ReservaDetalleDataModel ToDataModel(ReservaDetalleEntity entity) => new()
    {
        IdDetalle = entity.IdDetalle,
        RowVersion = entity.RowVersion,
        IdReserva = entity.IdReserva,
        IdPasajero = entity.IdPasajero,
        IdAsiento = entity.IdAsiento,
        SubtotalLinea = entity.SubtotalLinea,
        ValorIvaLinea = entity.ValorIvaLinea,
        TotalLinea = entity.TotalLinea,
        Estado = entity.Estado,
        EsEliminado = entity.EsEliminado,
        CreadoPorUsuario = entity.CreadoPorUsuario,
        FechaRegistroUtc = entity.FechaRegistroUtc,
        ModificadoPorUsuario = entity.ModificadoPorUsuario,
        FechaModificacionUtc = entity.FechaModificacionUtc,
        ModificacionIp = entity.ModificacionIp
    };

    public static ReservaDetalleEntity ToEntity(ReservaDetalleDataModel model) => new()
    {
        IdDetalle = model.IdDetalle,
        RowVersion = model.RowVersion,
        IdReserva = model.IdReserva,
        IdPasajero = model.IdPasajero,
        IdAsiento = model.IdAsiento,
        SubtotalLinea = model.SubtotalLinea,
        ValorIvaLinea = model.ValorIvaLinea,
        TotalLinea = model.TotalLinea,
        Estado = string.IsNullOrWhiteSpace(model.Estado) ? "ACTIVO" : model.Estado.Trim().ToUpperInvariant(),
        EsEliminado = model.EsEliminado,
        CreadoPorUsuario = string.IsNullOrWhiteSpace(model.CreadoPorUsuario) ? "SYSTEM" : model.CreadoPorUsuario.Trim(),
        FechaRegistroUtc = model.FechaRegistroUtc == default ? DateTime.UtcNow : model.FechaRegistroUtc,
        ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario) ? null : model.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = model.FechaModificacionUtc,
        ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp) ? null : model.ModificacionIp.Trim()
    };
}
