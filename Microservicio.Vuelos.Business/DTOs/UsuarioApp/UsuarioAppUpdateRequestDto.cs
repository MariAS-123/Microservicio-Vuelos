using System.Text.Json.Serialization;

namespace Microservicio.Vuelos.Business.DTOs.UsuarioApp;

public class UsuarioAppUpdateRequestDto
{
    [JsonPropertyName("correo")]
    public string Correo { get; set; } = null!;

    [JsonPropertyName("password")]
    public string? Password { get; set; }
}
