namespace Microservicio.Vuelos.Business.DTOs.Cliente;

public class ClienteResponseDto
{
    public int IdCliente { get; set; }

    public Guid ClienteGuid { get; set; }

    public string TipoIdentificacion { get; set; } = null!;

    public string NumeroIdentificacion { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string? Apellidos { get; set; }

    public string? RazonSocial { get; set; }

    public string Correo { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int IdCiudadResidencia { get; set; }

    public int IdPaisNacionalidad { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Nacionalidad { get; set; }

    public string? Genero { get; set; }

    public string Estado { get; set; } = null!;
}