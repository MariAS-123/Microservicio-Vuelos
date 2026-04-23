namespace Microservicio.Vuelos.DataManagement.Models;

public class CiudadFiltroDataModel
{
    public int? IdPais { get; set; }

    public string? Nombre { get; set; }

    public string? CodigoPostal { get; set; }

    public string? ZonaHoraria { get; set; }

    public string? Estado { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}