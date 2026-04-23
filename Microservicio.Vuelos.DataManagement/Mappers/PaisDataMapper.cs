using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class PaisDataMapper
{
    public static PaisDataModel ToDataModel(PaisEntity entity)
    {
        return new PaisDataModel
        {
            IdPais = entity.IdPais,
            CodigoIso2 = entity.CodigoIso2,
            CodigoIso3 = entity.CodigoIso3,
            Nombre = entity.Nombre,
            Continente = entity.Continente,
            Estado = entity.Estado,
            Eliminado = entity.Eliminado
        };
    }

    public static PaisEntity ToEntity(PaisDataModel model)
    {
        return new PaisEntity
        {
            IdPais = model.IdPais,
            CodigoIso2 = model.CodigoIso2.Trim().ToUpperInvariant(),
            CodigoIso3 = string.IsNullOrWhiteSpace(model.CodigoIso3)
                ? null
                : model.CodigoIso3.Trim().ToUpperInvariant(),
            Nombre = model.Nombre.Trim(),
            Continente = string.IsNullOrWhiteSpace(model.Continente)
                ? null
                : model.Continente.Trim(),
            Estado = string.IsNullOrWhiteSpace(model.Estado)
                ? "ACTIVO"
                : model.Estado.Trim().ToUpperInvariant(),
            Eliminado = model.Eliminado
        };
    }

    public static void UpdateEntity(PaisEntity entity, PaisDataModel model)
    {
        entity.CodigoIso2 = model.CodigoIso2.Trim().ToUpperInvariant();
        entity.CodigoIso3 = string.IsNullOrWhiteSpace(model.CodigoIso3)
            ? null
            : model.CodigoIso3.Trim().ToUpperInvariant();
        entity.Nombre = model.Nombre.Trim();
        entity.Continente = string.IsNullOrWhiteSpace(model.Continente)
            ? null
            : model.Continente.Trim();
        entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
            ? entity.Estado
            : model.Estado.Trim().ToUpperInvariant();
    }
}