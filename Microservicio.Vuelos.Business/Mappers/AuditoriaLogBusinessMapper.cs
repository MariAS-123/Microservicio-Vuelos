using Microservicio.Vuelos.Business.DTOs.AuditoriaLog;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class AuditoriaLogBusinessMapper
{
    public static AuditoriaLogFiltroDataModel ToFiltroDataModel(AuditoriaLogFilterDto dto)
    {
        return new AuditoriaLogFiltroDataModel
        {
            TablaAfectada = dto.TablaAfectada,
            Operacion = dto.Operacion,
            UsuarioEjecutor = dto.UsuarioEjecutor,
            FechaDesde = dto.FechaDesde,
            FechaHasta = dto.FechaHasta,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static AuditoriaLogDataModel ToDataModel(AuditoriaLogRequestDto dto)
    {
        return new AuditoriaLogDataModel
        {
            TablaAfectada = dto.TablaAfectada,
            Operacion = dto.Operacion,
            IdRegistroAfectado = dto.IdRegistroAfectado,
            DatosAnteriores = dto.DatosAnteriores,
            DatosNuevos = dto.DatosNuevos,
            UsuarioEjecutor = dto.UsuarioEjecutor,
            IpOrigen = dto.IpOrigen,
            Activo = true
        };
    }

    public static AuditoriaLogDataModel ToDataModel(long idAuditoria, AuditoriaLogUpdateRequestDto dto)
    {
        return new AuditoriaLogDataModel
        {
            IdAuditoria = idAuditoria,
            DatosAnteriores = dto.DatosAnteriores,
            DatosNuevos = dto.DatosNuevos
        };
    }

    public static AuditoriaLogResponseDto ToResponseDto(AuditoriaLogDataModel model)
    {
        return new AuditoriaLogResponseDto
        {
            IdAuditoria = model.IdAuditoria,
            AuditoriaGuid = model.AuditoriaGuid,
            TablaAfectada = model.TablaAfectada,
            Operacion = model.Operacion,
            IdRegistroAfectado = model.IdRegistroAfectado,
            DatosAnteriores = model.DatosAnteriores,
            DatosNuevos = model.DatosNuevos,
            UsuarioEjecutor = model.UsuarioEjecutor,
            IpOrigen = model.IpOrigen,
            FechaEventoUtc = model.FechaEventoUtc,
            Activo = model.Activo
        };
    }

    public static List<AuditoriaLogResponseDto> ToResponseDtoList(IEnumerable<AuditoriaLogDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}