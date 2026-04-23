namespace Microservicio.Vuelos.Business.DTOs.Asiento;

public class AsientoFilterDto
{
    public int? IdVuelo { get; set; }

    public bool? Disponible { get; set; }

    public string? Clase { get; set; }

    public string? NumeroAsiento { get; set; }

    public string? Posicion { get; set; }

    public string? Estado { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}