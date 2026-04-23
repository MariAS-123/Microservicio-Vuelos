using Microservicio.Vuelos.Business.DTOs.Pais;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class PaisBusinessMapper
{
    public static PaisFiltroDataModel ToFiltroDataModel(PaisFilterDto dto)
    {
        return new PaisFiltroDataModel
        {
            Nombre = dto.Nombre,
            CodigoIso2 = dto.CodigoIso2,
            CodigoIso3 = dto.CodigoIso3,
            Continente = dto.Continente,
            Estado = dto.Estado,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static PaisDataModel ToDataModel(PaisRequestDto dto, string creadoPorUsuario)
    {
        return new PaisDataModel
        {
            CodigoIso2 = dto.CodigoIso2,
            CodigoIso3 = dto.CodigoIso3,
            Nombre = dto.Nombre,
            Continente = dto.Continente,
            Estado = "ACTIVO",
            Eliminado = false,
        };
    }

    public static PaisDataModel ToDataModel(int idPais, PaisUpdateRequestDto dto)
    {
        return new PaisDataModel
        {
            IdPais = idPais,
            CodigoIso2 = dto.CodigoIso2,
            CodigoIso3 = dto.CodigoIso3,
            Nombre = dto.Nombre,
            Continente = dto.Continente
        };
    }

    public static PaisResponseDto ToResponseDto(PaisDataModel model)
    {
        return new PaisResponseDto
        {
            IdPais = model.IdPais,
            CodigoIso2 = model.CodigoIso2,
            CodigoIso3 = model.CodigoIso3,
            Nombre = model.Nombre,
            Continente = model.Continente,
            Estado = model.Estado
        };
    }

    public static List<PaisResponseDto> ToResponseDtoList(IEnumerable<PaisDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}