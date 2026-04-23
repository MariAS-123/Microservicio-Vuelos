using Microservicio.Vuelos.Business.DTOs.Ciudad;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class CiudadBusinessMapper
{
    public static CiudadFiltroDataModel ToFiltroDataModel(CiudadFilterDto dto)
    {
        return new CiudadFiltroDataModel
        {
            IdPais = dto.IdPais,
            Nombre = dto.Nombre,
            CodigoPostal = dto.CodigoPostal,
            ZonaHoraria = dto.ZonaHoraria,
            Estado = dto.Estado,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static CiudadDataModel ToDataModel(CiudadRequestDto dto, string creadoPorUsuario)
    {
        return new CiudadDataModel
        {
            IdPais = dto.IdPais,
            Nombre = dto.Nombre,
            CodigoPostal = dto.CodigoPostal,
            ZonaHoraria = dto.ZonaHoraria,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            Estado = "ACTIVO",
            Eliminado = false,
            CreadoPorUsuario = creadoPorUsuario
        };
    }

    public static CiudadDataModel ToDataModel(int idCiudad, CiudadUpdateRequestDto dto)
    {
        return new CiudadDataModel
        {
            IdCiudad = idCiudad,
            IdPais = dto.IdPais,
            Nombre = dto.Nombre,
            CodigoPostal = dto.CodigoPostal,
            ZonaHoraria = dto.ZonaHoraria,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud
        };
    }

    public static CiudadResponseDto ToResponseDto(CiudadDataModel model)
    {
        return new CiudadResponseDto
        {
            IdCiudad = model.IdCiudad,
            IdPais = model.IdPais,
            Nombre = model.Nombre,
            CodigoPostal = model.CodigoPostal,
            ZonaHoraria = model.ZonaHoraria,
            Latitud = model.Latitud,
            Longitud = model.Longitud,
            Estado = model.Estado
        };
    }

    public static List<CiudadResponseDto> ToResponseDtoList(IEnumerable<CiudadDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}