using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class AeropuertoBusinessMapper
{
    public static AeropuertoFiltroDataModel ToFiltroDataModel(AeropuertoFilterDto dto)
    {
        return new AeropuertoFiltroDataModel
        {
            CodigoIata = dto.CodigoIata,
            CodigoIcao = dto.CodigoIcao,
            Nombre = dto.Nombre,
            IdCiudad = dto.IdCiudad,
            IdPais = dto.IdPais,
            ZonaHoraria = dto.ZonaHoraria,
            Estado = dto.Estado,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static AeropuertoDataModel ToDataModel(AeropuertoRequestDto dto, string creadoPorUsuario)
    {
        return new AeropuertoDataModel
        {
            CodigoIata = dto.CodigoIata,
            CodigoIcao = dto.CodigoIcao,
            Nombre = dto.Nombre,
            IdCiudad = dto.IdCiudad,
            IdPais = dto.IdPais,
            ZonaHoraria = dto.ZonaHoraria,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            Estado = "ACTIVO",
            Eliminado = false,
            CreadoPorUsuario = creadoPorUsuario
        };
    }

    public static AeropuertoDataModel ToDataModel(int idAeropuerto, AeropuertoUpdateRequestDto dto)
    {
        return new AeropuertoDataModel
        {
            IdAeropuerto = idAeropuerto,
            CodigoIata = dto.CodigoIata,
            CodigoIcao = dto.CodigoIcao,
            Nombre = dto.Nombre,
            IdCiudad = dto.IdCiudad,
            IdPais = dto.IdPais,
            ZonaHoraria = dto.ZonaHoraria,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud
        };
    }

    public static AeropuertoResponseDto ToResponseDto(AeropuertoDataModel model)
    {
        return new AeropuertoResponseDto
        {
            IdAeropuerto = model.IdAeropuerto,
            CodigoIata = model.CodigoIata,
            CodigoIcao = model.CodigoIcao,
            Nombre = model.Nombre,
            IdCiudad = model.IdCiudad ?? 0,
            IdPais = model.IdPais,
            ZonaHoraria = model.ZonaHoraria,
            Latitud = model.Latitud,
            Longitud = model.Longitud,
            Estado = model.Estado
        };
    }

    public static List<AeropuertoResponseDto> ToResponseDtoList(IEnumerable<AeropuertoDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}