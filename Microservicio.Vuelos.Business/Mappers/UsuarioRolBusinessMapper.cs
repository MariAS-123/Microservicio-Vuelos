using Microservicio.Vuelos.Business.DTOs.UsuarioRol;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class UsuarioRolBusinessMapper
{
    public static UsuarioRolFiltroDataModel ToFiltroDataModel(UsuarioRolFilterDto dto)
    {
        return new UsuarioRolFiltroDataModel
        {
            IdUsuario = dto.IdUsuario,
            IdRol = dto.IdRol,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static UsuarioRolDataModel ToDataModel(UsuarioRolRequestDto dto, string creadoPorUsuario)
    {
        return new UsuarioRolDataModel
        {
            IdUsuario = dto.IdUsuario,
            IdRol = dto.IdRol,
            EstadoUsuarioRol = "ACT",
            Activo = true,
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario
        };
    }

    // ✅ ToDataModel para Update eliminado — UpdateAsync ya no existe en el service

    public static UsuarioRolResponseDto ToResponseDto(UsuarioRolDataModel model)
    {
        return new UsuarioRolResponseDto
        {
            IdUsuarioRol = model.IdUsuarioRol,
            IdUsuario = model.IdUsuario,
            IdRol = model.IdRol,
            EstadoUsuarioRol = model.EstadoUsuarioRol,
            Activo = model.Activo
        };
    }

    public static List<UsuarioRolResponseDto> ToResponseDtoList(IEnumerable<UsuarioRolDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}