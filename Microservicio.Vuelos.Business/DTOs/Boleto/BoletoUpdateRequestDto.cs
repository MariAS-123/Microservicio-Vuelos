namespace Microservicio.Vuelos.Business.DTOs.Boleto;

public class BoletoUpdateRequestDto
{
    public string EstadoBoleto { get; set; } = null!; // ACTIVO / USADO / CANCELADO
}