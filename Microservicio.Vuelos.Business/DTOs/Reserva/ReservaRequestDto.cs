namespace Microservicio.Vuelos.Business.DTOs.Reserva;

public class ReservaRequestDto
{
    public int IdCliente { get; set; }

    public int IdPasajero { get; set; }

    public int IdVuelo { get; set; }

    public int IdAsiento { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public decimal SubtotalReserva { get; set; }

    public decimal ValorIva { get; set; }

    public decimal TotalReserva { get; set; }

    public string? OrigenCanalReserva { get; set; }

    public string? ContactoEmail { get; set; }

    public string? ContactoTelefono { get; set; }

    public string? Observaciones { get; set; }
}