namespace Microservicio.Vuelos.Business.DTOs.Reserva;

public class ReservaPagarResponseDto
{
    public ReservaPagoReservaResumenDto Reserva { get; set; } = null!;
    public Factura.FacturaResponseDto Factura { get; set; } = null!;
    public List<Boleto.BoletoResponseDto> Boletos { get; set; } = new();
    public List<Equipaje.EquipajeResponseDto> Equipajes { get; set; } = new();
}
