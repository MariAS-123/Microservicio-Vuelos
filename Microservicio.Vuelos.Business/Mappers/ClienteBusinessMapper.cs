using Microservicio.Vuelos.Business.DTOs.Cliente;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class ClienteBusinessMapper
{
    public static ClienteFiltroDataModel ToFiltroDataModel(ClienteFilterDto dto)
    {
        return new ClienteFiltroDataModel
        {
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Correo = dto.Correo,
            Estado = dto.Estado,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static ClienteDataModel ToDataModel(ClienteRequestDto dto, string creadoPorUsuario)
    {
        return new ClienteDataModel
        {
            TipoIdentificacion = dto.TipoIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            RazonSocial = dto.RazonSocial,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            IdCiudadResidencia = dto.IdCiudadResidencia,
            IdPaisNacionalidad = dto.IdPaisNacionalidad,
            FechaNacimiento = dto.FechaNacimiento,
            Nacionalidad = dto.Nacionalidad,
            Genero = dto.Genero,
            Estado = "ACT",
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario,
            ServicioOrigen = "VUELOS"
        };
    }

    public static ClienteDataModel ToDataModel(int idCliente, ClienteUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new ClienteDataModel
        {
            IdCliente = idCliente,
            TipoIdentificacion = dto.TipoIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            RazonSocial = dto.RazonSocial,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            IdCiudadResidencia = dto.IdCiudadResidencia,
            IdPaisNacionalidad = dto.IdPaisNacionalidad,
            FechaNacimiento = dto.FechaNacimiento,
            Nacionalidad = dto.Nacionalidad,
            Genero = dto.Genero,
            Estado = "ACT",
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static ClienteResponseDto ToResponseDto(ClienteDataModel model)
    {
        return new ClienteResponseDto
        {
            IdCliente = model.IdCliente,
            ClienteGuid = model.ClienteGuid,
            TipoIdentificacion = model.TipoIdentificacion,
            NumeroIdentificacion = model.NumeroIdentificacion,
            Nombres = model.Nombres,
            Apellidos = model.Apellidos,
            RazonSocial = model.RazonSocial,
            Correo = model.Correo,
            Telefono = model.Telefono,
            Direccion = model.Direccion,
            IdCiudadResidencia = model.IdCiudadResidencia,
            IdPaisNacionalidad = model.IdPaisNacionalidad,
            FechaNacimiento = model.FechaNacimiento,
            Nacionalidad = model.Nacionalidad,
            Genero = model.Genero,
            Estado = model.Estado
        };
    }

    public static List<ClienteResponseDto> ToResponseDtoList(IEnumerable<ClienteDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}