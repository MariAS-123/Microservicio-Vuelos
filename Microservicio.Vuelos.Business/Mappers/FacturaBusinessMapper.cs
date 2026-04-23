using Microservicio.Vuelos.Business.DTOs.Factura;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class FacturaBusinessMapper
{
    public static FacturaFiltroDataModel ToFiltroDataModel(FacturaFilterDto dto)
    {
        return new FacturaFiltroDataModel
        {
            NumeroFactura = dto.NumeroFactura,
            IdCliente = dto.IdCliente,
            IdReserva = dto.IdReserva,
            Estado = dto.Estado,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static FacturaDataModel ToDataModel(FacturaRequestDto dto, string creadoPorUsuario)
    {
        return new FacturaDataModel
        {
            IdCliente = dto.IdCliente,
            IdReserva = dto.IdReserva,
            Subtotal = dto.Subtotal,
            ValorIva = dto.ValorIva,
            CargoServicio = dto.CargoServicio,
            Total = dto.Total,
            ObservacionesFactura = dto.ObservacionesFactura,
            Estado = "ABI",
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario,
            ServicioOrigen = "VUELOS"
        };
    }

    public static FacturaDataModel ToDataModel(int idFactura, FacturaUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new FacturaDataModel
        {
            IdFactura = idFactura,
            Estado = dto.Estado,
            MotivoInhabilitacion = dto.MotivoInhabilitacion,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static FacturaResponseDto ToResponseDto(FacturaDataModel model)
    {
        return new FacturaResponseDto
        {
            IdFactura = model.IdFactura,
            GuidFactura = model.GuidFactura,
            NumeroFactura = model.NumeroFactura,
            IdCliente = model.IdCliente,
            IdReserva = model.IdReserva,
            FechaEmision = model.FechaEmision,
            Subtotal = model.Subtotal,
            ValorIva = model.ValorIva,
            CargoServicio = model.CargoServicio,
            Total = model.Total,
            Estado = model.Estado,
            ObservacionesFactura = model.ObservacionesFactura
        };
    }

    public static List<FacturaResponseDto> ToResponseDtoList(IEnumerable<FacturaDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}