namespace Microservicio.Vuelos.DataManagement.Models;

public class ClienteFiltroDataModel
{
    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? TipoIdentificacion { get; set; }

    public string? NumeroIdentificacion { get; set; }

    public string? Correo { get; set; }

    public int? IdPaisNacionalidad { get; set; }

    public int? IdCiudadResidencia { get; set; }

    public string? Estado { get; set; } // ACT / INA

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}