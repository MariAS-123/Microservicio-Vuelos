using System.Text.Json.Serialization;

namespace Microservicio.Vuelos.Business.DTOs.Pasajero;

public class PasajeroRequestDto
{
    [JsonPropertyName("id_cliente")]
    public int? IdCliente { get; set; }

    [JsonPropertyName("nombre_pasajero")]
    public string NombrePasajero { get; set; } = null!;

    [JsonPropertyName("apellido_pasajero")]
    public string ApellidoPasajero { get; set; } = null!;

    [JsonPropertyName("tipo_documento_pasajero")]
    public string TipoDocumentoPasajero { get; set; } = null!;

    [JsonPropertyName("numero_documento_pasajero")]
    public string NumeroDocumentoPasajero { get; set; } = null!;

    [JsonPropertyName("fecha_nacimiento_pasajero")]
    public DateTime? FechaNacimientoPasajero { get; set; }

    [JsonPropertyName("id_pais_nacionalidad")]
    public int? IdPaisNacionalidad { get; set; }

    [JsonPropertyName("email_contacto_pasajero")]
    public string? EmailContactoPasajero { get; set; }

    [JsonPropertyName("telefono_contacto_pasajero")]
    public string? TelefonoContactoPasajero { get; set; }

    [JsonPropertyName("genero_pasajero")]
    public string? GeneroPasajero { get; set; }

    [JsonPropertyName("requiere_asistencia")]
    public bool RequiereAsistencia { get; set; }

    [JsonPropertyName("observaciones_pasajero")]
    public string? ObservacionesPasajero { get; set; }
}
