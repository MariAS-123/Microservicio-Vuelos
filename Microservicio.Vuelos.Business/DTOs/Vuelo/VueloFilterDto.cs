namespace Microservicio.Vuelos.Business.DTOs.Vuelo;

public class VueloFilterDto
{
    public int? IdAeropuertoOrigen { get; set; }

    public int? IdAeropuertoDestino { get; set; }

    public DateTime? FechaSalida { get; set; }

    public string? NumeroVuelo { get; set; }

    public string? EstadoVuelo { get; set; }

    public string? Estado { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}