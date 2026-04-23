namespace Microservicio.Vuelos.DataManagement.Models;

public class PaisFiltroDataModel
{
    public string? Nombre { get; set; }

    public string? CodigoIso2 { get; set; }

    public string? CodigoIso3 { get; set; }

    public string? Continente { get; set; }

    public string? Estado { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}