using System.Text.Json.Serialization;

namespace Microservicio.Vuelos.Business.DTOs.UsuarioApp;

public class UsuarioAppRequestDto
{
    [JsonPropertyName("id_cliente")]
    public int? IdCliente { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}
