namespace Microservicio.Vuelos.DataManagement.Models;

public class RolFiltroDataModel
{
    public string? NombreRol { get; set; }

    public string? EstadoRol { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}