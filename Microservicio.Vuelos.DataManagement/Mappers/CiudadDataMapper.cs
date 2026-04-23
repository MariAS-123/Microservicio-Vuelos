using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class CiudadDataMapper
{
    public static CiudadDataModel ToDataModel(CiudadEntity entity)
    {
        return new CiudadDataModel
        {
            IdCiudad = entity.IdCiudad,
            RowVersion = entity.RowVersion,
            IdPais = entity.IdPais,
            Nombre = entity.Nombre,
            CodigoPostal = entity.CodigoPostal,
            ZonaHoraria = entity.ZonaHoraria,
            Latitud = entity.Latitud,
            Longitud = entity.Longitud,
            Estado = entity.Estado,
            Eliminado = entity.Eliminado,
            FechaRegistroUtc = entity.FechaRegistroUtc,
            CreadoPorUsuario = entity.CreadoPorUsuario,
            ModificadoPorUsuario = entity.ModificadoPorUsuario,
            FechaModificacionUtc = entity.FechaModificacionUtc,
            ModificacionIp = entity.ModificacionIp
        };
    }

    public static CiudadEntity ToEntity(CiudadDataModel model)
    {
        return new CiudadEntity
        {
            IdCiudad = model.IdCiudad,
            IdPais = model.IdPais,
            Nombre = model.Nombre.Trim(),
            CodigoPostal = string.IsNullOrWhiteSpace(model.CodigoPostal) ? null : model.CodigoPostal.Trim(),
            ZonaHoraria = string.IsNullOrWhiteSpace(model.ZonaHoraria) ? null : model.ZonaHoraria.Trim(),
            Latitud = model.Latitud,
            Longitud = model.Longitud,
            Estado = string.IsNullOrWhiteSpace(model.Estado) ? "ACTIVO" : model.Estado.Trim(),
            Eliminado = model.Eliminado,
            FechaRegistroUtc = model.FechaRegistroUtc,
            CreadoPorUsuario = model.CreadoPorUsuario.Trim(),
            ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario) ? null : model.ModificadoPorUsuario.Trim(),
            FechaModificacionUtc = model.FechaModificacionUtc,
            ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp) ? null : model.ModificacionIp.Trim()
        };
    }

    public static void UpdateEntity(CiudadEntity entity, CiudadDataModel model)
    {
        entity.IdPais = model.IdPais;
        entity.Nombre = model.Nombre.Trim();
        entity.CodigoPostal = string.IsNullOrWhiteSpace(model.CodigoPostal) ? null : model.CodigoPostal.Trim();
        entity.ZonaHoraria = string.IsNullOrWhiteSpace(model.ZonaHoraria) ? null : model.ZonaHoraria.Trim();
        entity.Latitud = model.Latitud;
        entity.Longitud = model.Longitud;
        entity.Estado = string.IsNullOrWhiteSpace(model.Estado) ? entity.Estado : model.Estado.Trim();
        entity.ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario) ? null : model.ModificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = model.FechaModificacionUtc;
        entity.ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp) ? null : model.ModificacionIp.Trim();
    }
}