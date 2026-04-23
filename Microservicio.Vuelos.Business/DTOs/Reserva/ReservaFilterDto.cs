namespace Microservicio.Vuelos.Business.DTOs.Reserva;

public class ReservaFilterDto
{
    public string? CodigoReserva { get; set; }

    public int? IdCliente { get; set; }

    public int? IdPasajero { get; set; }

    public int? IdVuelo { get; set; }

    public string? EstadoReserva { get; set; } // PEN / CON / CAN / EXP / FIN / EMI

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}