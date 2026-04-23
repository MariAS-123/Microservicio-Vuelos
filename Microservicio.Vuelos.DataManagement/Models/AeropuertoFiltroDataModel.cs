namespace Microservicio.Vuelos.DataManagement.Models;

public class AeropuertoFiltroDataModel
{
    public string? CodigoIata { get; set; }

    public string? CodigoIcao { get; set; }

    public string? Nombre { get; set; }

    public int? IdCiudad { get; set; }

    public int? IdPais { get; set; }

    public string? ZonaHoraria { get; set; }

    public string? Estado { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}