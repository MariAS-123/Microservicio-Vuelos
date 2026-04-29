using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Business.DTOs.Pasajero;

public class PasajeroFilterDto
{
    [FromQuery(Name = "id_cliente")]
    public int? IdCliente { get; set; }

    [FromQuery(Name = "nombre_pasajero")]
    public string? NombrePasajero { get; set; }

    [FromQuery(Name = "apellido_pasajero")]
    public string? ApellidoPasajero { get; set; }

    [FromQuery(Name = "tipo_documento_pasajero")]
    public string? TipoDocumentoPasajero { get; set; }

    [FromQuery(Name = "numero_documento_pasajero")]
    public string? NumeroDocumentoPasajero { get; set; }

    [FromQuery(Name = "estado")]
    public string? Estado { get; set; }

    [FromQuery(Name = "requiere_asistencia")]
    public bool? RequiereAsistencia { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}
