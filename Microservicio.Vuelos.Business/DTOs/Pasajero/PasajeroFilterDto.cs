namespace Microservicio.Vuelos.Business.DTOs.Pasajero;

public class PasajeroFilterDto
{
    public int? IdCliente { get; set; }

    public string? NombrePasajero { get; set; }

    public string? ApellidoPasajero { get; set; }

    public string? TipoDocumentoPasajero { get; set; }

    public string? NumeroDocumentoPasajero { get; set; }

    public string? Estado { get; set; } // ACTIVO / INACTIVO

    public bool? RequiereAsistencia { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}