namespace Microservicio.Vuelos.DataManagement.Models;

public class VueloFiltroDataModel
{
    public string? NumeroVuelo { get; set; }

    public int? IdAeropuertoOrigen { get; set; }

    public int? IdAeropuertoDestino { get; set; }

    public string? EstadoVuelo { get; set; } // PROGRAMADO, EN_VUELO, ATERRIZADO, CANCELADO, DEMORADO

    public string? Estado { get; set; } // ACTIVO, INACTIVO

    public DateTime? FechaSalidaDesde { get; set; }

    public DateTime? FechaSalidaHasta { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}