using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class ClienteDataMapper
{
    public static ClienteDataModel ToDataModel(ClienteEntity entity) => new()
    {
        IdCliente = entity.IdCliente,
        ClienteGuid = entity.ClienteGuid,
        TipoIdentificacion = entity.TipoIdentificacion,
        NumeroIdentificacion = entity.NumeroIdentificacion,
        Nombres = entity.Nombres,
        Apellidos = entity.Apellidos,
        RazonSocial = entity.RazonSocial,
        Correo = entity.Correo,
        Telefono = entity.Telefono,
        Direccion = entity.Direccion,
        IdCiudadResidencia = entity.IdCiudadResidencia,
        IdPaisNacionalidad = entity.IdPaisNacionalidad,
        FechaNacimiento = entity.FechaNacimiento,
        Nacionalidad = entity.Nacionalidad,
        Genero = entity.Genero,
        Estado = entity.Estado,
        EsEliminado = entity.EsEliminado,
        CreadoPorUsuario = entity.CreadoPorUsuario,
        FechaRegistroUtc = entity.FechaRegistroUtc,
        ModificadoPorUsuario = entity.ModificadoPorUsuario,
        FechaModificacionUtc = entity.FechaModificacionUtc,
        ModificacionIp = entity.ModificacionIp,
        ServicioOrigen = entity.ServicioOrigen,
        FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
        MotivoInhabilitacion = entity.MotivoInhabilitacion,
        RowVersion = entity.RowVersion
    };

    public static ClienteEntity ToEntity(ClienteDataModel model) => new()
    {
        IdCliente = model.IdCliente,
        ClienteGuid = model.ClienteGuid == Guid.Empty ? Guid.NewGuid() : model.ClienteGuid,
        TipoIdentificacion = model.TipoIdentificacion.Trim().ToUpperInvariant(),
        NumeroIdentificacion = model.NumeroIdentificacion.Trim(),
        Nombres = model.Nombres.Trim(),
        Apellidos = string.IsNullOrWhiteSpace(model.Apellidos) ? null : model.Apellidos.Trim(),
        RazonSocial = string.IsNullOrWhiteSpace(model.RazonSocial) ? null : model.RazonSocial.Trim(),
        Correo = model.Correo.Trim().ToLowerInvariant(),
        Telefono = model.Telefono.Trim(),
        Direccion = model.Direccion.Trim(),
        IdCiudadResidencia = model.IdCiudadResidencia,
        IdPaisNacionalidad = model.IdPaisNacionalidad,
        FechaNacimiento = model.FechaNacimiento,
        Nacionalidad = string.IsNullOrWhiteSpace(model.Nacionalidad) ? null : model.Nacionalidad.Trim(),
        Genero = string.IsNullOrWhiteSpace(model.Genero) ? null : model.Genero.Trim().ToUpperInvariant(),
        Estado = string.IsNullOrWhiteSpace(model.Estado) ? "ACT" : model.Estado.Trim().ToUpperInvariant(),
        EsEliminado = model.EsEliminado,
        CreadoPorUsuario = model.CreadoPorUsuario.Trim(),
        FechaRegistroUtc = model.FechaRegistroUtc == default ? DateTime.UtcNow : model.FechaRegistroUtc,
        ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario) ? null : model.ModificadoPorUsuario.Trim(),
        FechaModificacionUtc = model.FechaModificacionUtc,
        ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp) ? null : model.ModificacionIp.Trim(),
        ServicioOrigen = string.IsNullOrWhiteSpace(model.ServicioOrigen) ? "VUELOS" : model.ServicioOrigen.Trim().ToUpperInvariant(),
        FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
        MotivoInhabilitacion = string.IsNullOrWhiteSpace(model.MotivoInhabilitacion) ? null : model.MotivoInhabilitacion.Trim(),
        RowVersion = model.RowVersion
    };

    public static void UpdateEntity(ClienteEntity entity, ClienteDataModel model)
    {
        entity.TipoIdentificacion = model.TipoIdentificacion.Trim().ToUpperInvariant();
        entity.NumeroIdentificacion = model.NumeroIdentificacion.Trim();
        entity.Nombres = model.Nombres.Trim();
        entity.Apellidos = string.IsNullOrWhiteSpace(model.Apellidos) ? null : model.Apellidos.Trim();
        entity.RazonSocial = string.IsNullOrWhiteSpace(model.RazonSocial) ? null : model.RazonSocial.Trim();
        entity.Correo = model.Correo.Trim().ToLowerInvariant();
        entity.Telefono = model.Telefono.Trim();
        entity.Direccion = model.Direccion.Trim();
        entity.IdCiudadResidencia = model.IdCiudadResidencia;
        entity.IdPaisNacionalidad = model.IdPaisNacionalidad;
        entity.FechaNacimiento = model.FechaNacimiento;
        entity.Nacionalidad = string.IsNullOrWhiteSpace(model.Nacionalidad) ? null : model.Nacionalidad.Trim();
        entity.Genero = string.IsNullOrWhiteSpace(model.Genero) ? null : model.Genero.Trim().ToUpperInvariant();
        entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
            ? entity.Estado
            : model.Estado.Trim().ToUpperInvariant();
        entity.ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario) ? null : model.ModificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = model.FechaModificacionUtc;
        entity.ModificacionIp = string.IsNullOrWhiteSpace(model.ModificacionIp) ? null : model.ModificacionIp.Trim();
        entity.FechaInhabilitacionUtc = model.FechaInhabilitacionUtc;
        entity.MotivoInhabilitacion = string.IsNullOrWhiteSpace(model.MotivoInhabilitacion) ? null : model.MotivoInhabilitacion.Trim();
    }
}