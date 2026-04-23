using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class ReservaBusinessMapper
{
    public static ReservaFiltroDataModel ToFiltroDataModel(ReservaFilterDto dto)
    {
        return new ReservaFiltroDataModel
        {
            CodigoReserva = dto.CodigoReserva,
            IdCliente = dto.IdCliente,
            IdPasajero = dto.IdPasajero,
            IdVuelo = dto.IdVuelo,
            EstadoReserva = dto.EstadoReserva,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static ReservaDataModel ToDataModel(ReservaRequestDto dto, string creadoPorUsuario)
    {
        return new ReservaDataModel
        {
            IdCliente = dto.IdCliente,
            IdPasajero = dto.IdPasajero,
            IdVuelo = dto.IdVuelo,
            IdAsiento = dto.IdAsiento,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            SubtotalReserva = dto.SubtotalReserva,
            ValorIva = dto.ValorIva,
            TotalReserva = dto.TotalReserva,
            OrigenCanalReserva = string.IsNullOrWhiteSpace(dto.OrigenCanalReserva) ? "BOOKING" : dto.OrigenCanalReserva,
            ContactoEmail = dto.ContactoEmail,
            ContactoTelefono = dto.ContactoTelefono,
            Observaciones = dto.Observaciones,
            EstadoReserva = "PEN",
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario,
            ServicioOrigen = "VUELOS"
        };
    }

    public static ReservaDataModel ToDataModel(int idReserva, ReservaUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new ReservaDataModel
        {
            IdReserva = idReserva,
            EstadoReserva = dto.EstadoReserva,
            MotivoCancelacion = dto.MotivoCancelacion,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static ReservaResponseDto ToResponseDto(ReservaDataModel model)
    {
        return new ReservaResponseDto
        {
            IdReserva = model.IdReserva,
            GuidReserva = model.GuidReserva,
            CodigoReserva = model.CodigoReserva,
            IdCliente = model.IdCliente,
            IdPasajero = model.IdPasajero,
            IdVuelo = model.IdVuelo,
            IdAsiento = model.IdAsiento,
            FechaReservaUtc = model.FechaReservaUtc,
            FechaInicio = model.FechaInicio,
            FechaFin = model.FechaFin,
            SubtotalReserva = model.SubtotalReserva,
            ValorIva = model.ValorIva,
            TotalReserva = model.TotalReserva,
            OrigenCanalReserva = model.OrigenCanalReserva,
            EstadoReserva = model.EstadoReserva,
            FechaConfirmacionUtc = model.FechaConfirmacionUtc,
            FechaCancelacionUtc = model.FechaCancelacionUtc,
            MotivoCancelacion = model.MotivoCancelacion,
            ContactoEmail = model.ContactoEmail,
            ContactoTelefono = model.ContactoTelefono,
            Observaciones = model.Observaciones
        };
    }

    public static List<ReservaResponseDto> ToResponseDtoList(IEnumerable<ReservaDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}