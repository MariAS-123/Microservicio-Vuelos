namespace Microservicio.Vuelos.DataManagement.Models;

public class UsuarioRolFiltroDataModel
{
    public int? IdUsuario { get; set; }

    public int? IdRol { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}