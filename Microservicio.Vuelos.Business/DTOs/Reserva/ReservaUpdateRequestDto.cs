namespace Microservicio.Vuelos.Business.DTOs.Reserva;

public class ReservaUpdateRequestDto
{
    public string EstadoReserva { get; set; } = null!;

    public string? MotivoCancelacion { get; set; }
}