namespace Microservicio.Vuelos.DataManagement.Models;

public class UsuarioAppFiltroDataModel
{
    public string? Username { get; set; }

    public string? Correo { get; set; }

    public bool? Activo { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}