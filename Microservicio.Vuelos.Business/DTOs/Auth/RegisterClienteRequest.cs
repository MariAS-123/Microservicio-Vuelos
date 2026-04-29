using System.Text.Json.Serialization;

namespace Microservicio.Vuelos.Business.DTOs.Auth;

public class RegisterClienteRequest
{
    [JsonPropertyName("tipo_identificacion")]
    public string TipoIdentificacion { get; set; } = null!;

    [JsonPropertyName("numero_identificacion")]
    public string NumeroIdentificacion { get; set; } = null!;

    [JsonPropertyName("nombres")]
    public string Nombres { get; set; } = null!;

    [JsonPropertyName("apellidos")]
    public string? Apellidos { get; set; }

    [JsonPropertyName("razon_social")]
    public string? RazonSocial { get; set; }

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = null!;

    [JsonPropertyName("telefono")]
    public string Telefono { get; set; } = null!;

    [JsonPropertyName("direccion")]
    public string Direccion { get; set; } = null!;

    [JsonPropertyName("id_ciudad_residencia")]
    public int IdCiudadResidencia { get; set; }

    [JsonPropertyName("id_pais_nacionalidad")]
    public int IdPaisNacionalidad { get; set; }

    [JsonPropertyName("fecha_nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [JsonPropertyName("genero")]
    public string? Genero { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}
