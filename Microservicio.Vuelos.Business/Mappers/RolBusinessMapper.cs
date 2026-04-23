using Microservicio.Vuelos.Business.DTOs.Rol;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class RolBusinessMapper
{
    public static RolFiltroDataModel ToFiltroDataModel(RolFilterDto dto)
    {
        return new RolFiltroDataModel
        {
            NombreRol = dto.NombreRol,
            EstadoRol = dto.EstadoRol,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static RolDataModel ToDataModel(RolRequestDto dto, string creadoPorUsuario)
    {
        return new RolDataModel
        {
            NombreRol = dto.NombreRol,
            DescripcionRol = dto.DescripcionRol,
            EstadoRol = "ACT",
            Activo = true,
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario
        };
    }

    public static RolDataModel ToDataModel(int idRol, RolUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new RolDataModel
        {
            IdRol = idRol,
            NombreRol = dto.NombreRol,
            DescripcionRol = dto.DescripcionRol,
            EstadoRol = "ACT",
            Activo = true,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static RolResponseDto ToResponseDto(RolDataModel model)
    {
        return new RolResponseDto
        {
            IdRol = model.IdRol,
            RolGuid = model.RolGuid,
            NombreRol = model.NombreRol,
            DescripcionRol = model.DescripcionRol,
            EstadoRol = model.EstadoRol,
            Activo = model.Activo
        };
    }

    public static List<RolResponseDto> ToResponseDtoList(IEnumerable<RolDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}