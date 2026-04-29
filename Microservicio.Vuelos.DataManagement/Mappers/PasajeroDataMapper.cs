using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class PasajeroDataMapper
{
    public static PasajeroDataModel ToDataModel(PasajeroEntity entity) => new()
    {
        IdPasajero = entity.IdPasajero,
        RowVersion = entity.RowVersion,
        IdCliente = entity.IdCliente,
        NombrePasajero = entity.NombrePasajero,
        ApellidoPasajero = entity.ApellidoPasajero,
        TipoDocumentoPasajero = entity.TipoDocumentoPasajero,
        NumeroDocumentoPasajero = entity.NumeroDocumentoPasajero,
        FechaNacimientoPasajero = entity.FechaNacimientoPasajero,
        IdPaisNacionalidad = entity.IdPaisNacionalidad,
        EmailContactoPasajero = entity.EmailContactoPasajero,
        TelefonoContactoPasajero = entity.TelefonoContactoPasajero,
        GeneroPasajero = entity.GeneroPasajero,
        RequiereAsistencia = entity.RequiereAsistencia,
        ObservacionesPasajero = entity.ObservacionesPasajero,
        Estado = entity.Estado,
        EsEliminado = entity.EsEliminado,
        CreadoPorUsuario = entity.CreadoPorUsuario,
        FechaRegistroUtc = entity.FechaRegistroUtc,
        ModificadoPorUsuario = entity.ModificadoPorUsuario,
        FechaModificacionUtc = entity.FechaModificacionUtc,
        ModificacionIp = entity.ModificacionIp
    };

    public static PasajeroEntity ToEntity(PasajeroDataModel model) => new()
    {
        IdPasajero = model.IdPasajero,
        IdCliente = model.IdCliente,
        NombrePasajero = model.NombrePasajero.Trim(),
        ApellidoPasajero = model.ApellidoPasajero.Trim(),
        TipoDocumentoPasajero = model.TipoDocumentoPasajero.Trim().ToUpperInvariant(),
        NumeroDocumentoPasajero = model.NumeroDocumentoPasajero.Trim(),
        FechaNacimientoPasajero = model.FechaNacimientoPasajero,
        IdPaisNacionalidad = model.IdPaisNacionalidad,
        EmailContactoPasajero = string.IsNullOrWhiteSpace(model.EmailContactoPasajero)
            ? null
            : model.EmailContactoPasajero.Trim().ToLowerInvariant(),
        TelefonoContactoPasajero = string.IsNullOrWhiteSpace(model.TelefonoContactoPasajero)
            ? null
            : model.TelefonoContactoPasajero.Trim(),
        GeneroPasajero = string.IsNullOrWhiteSpace(model.GeneroPasajero)
            ? null
            : model.GeneroPasajero.Trim().ToUpperInvariant(),
        RequiereAsistencia = model.RequiereAsistencia,
        ObservacionesPasajero = string.IsNullOrWhiteSpace(model.ObservacionesPasajero)
            ? null
            : model.ObservacionesPasajero.Trim(),
        Estado = string.IsNullOrWhiteSpace(model.Estado)
            ? "ACTIVO"
            : model.Estado.Trim().ToUpperInvariant(),
        EsEliminado = model.EsEliminado,
        CreadoPorUsuario = model.CreadoPorUsuario.Trim(),
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

    public static void UpdateEntity(PasajeroEntity entity, PasajeroDataModel model)
    {
        entity.IdCliente = model.IdCliente;
        entity.NombrePasajero = model.NombrePasajero.Trim();
        entity.ApellidoPasajero = model.ApellidoPasajero.Trim();
        entity.TipoDocumentoPasajero = model.TipoDocumentoPasajero.Trim().ToUpperInvariant();
        entity.NumeroDocumentoPasajero = model.NumeroDocumentoPasajero.Trim();
        entity.FechaNacimientoPasajero = model.FechaNacimientoPasajero;
        entity.IdPaisNacionalidad = model.IdPaisNacionalidad;
        entity.EmailContactoPasajero = string.IsNullOrWhiteSpace(model.EmailContactoPasajero)
            ? null
            : model.EmailContactoPasajero.Trim().ToLowerInvariant();
        entity.TelefonoContactoPasajero = string.IsNullOrWhiteSpace(model.TelefonoContactoPasajero)
            ? null
            : model.TelefonoContactoPasajero.Trim();
        entity.GeneroPasajero = string.IsNullOrWhiteSpace(model.GeneroPasajero)
            ? null
            : model.GeneroPasajero.Trim().ToUpperInvariant();
        entity.RequiereAsistencia = model.RequiereAsistencia;
        entity.ObservacionesPasajero = string.IsNullOrWhiteSpace(model.ObservacionesPasajero)
            ? null
            : model.ObservacionesPasajero.Trim();
        entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
            ? entity.Estado
            : model.Estado.Trim().ToUpperInvariant();
        entity.ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario)
            ? null
            : model.ModificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;
        entity.ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp)
            ? null
            : model.ModificacionIp.Trim();
    }
}
