namespace Microservicio.Vuelos.DataManagement.Models;

public class PasajeroDataModel
{
    public int IdPasajero { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public int? IdCliente { get; set; }

    public string NombrePasajero { get; set; } = null!;

    public string ApellidoPasajero { get; set; } = null!;

    public string TipoDocumentoPasajero { get; set; } = null!;

    public string NumeroDocumentoPasajero { get; set; } = null!;

    public DateTime? FechaNacimientoPasajero { get; set; }
    public int? IdPaisNacionalidad { get; set; }

    public string? EmailContactoPasajero { get; set; }

    public string? TelefonoContactoPasajero { get; set; }

    public string? GeneroPasajero { get; set; }

    public bool RequiereAsistencia { get; set; }

    public string? ObservacionesPasajero { get; set; }

    public string Estado { get; set; } = null!; // ACTIVO / INACTIVO

    public bool EsEliminado { get; set; }

    public string CreadoPorUsuario { get; set; } = null!;

    public DateTime FechaRegistroUtc { get; set; }

    public string? ModificadoPorUsuario { get; set; }

    public DateTime? FechaModificacionUtc { get; set; }

    public string? ModificacionIp { get; set; }
}
