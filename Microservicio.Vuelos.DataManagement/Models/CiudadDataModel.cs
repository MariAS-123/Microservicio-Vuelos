namespace Microservicio.Vuelos.DataManagement.Models;

public class CiudadDataModel
{
    public int IdCiudad { get; set; }
    public byte[] RowVersion { get; set; } = null!;
    public int IdPais { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoPostal { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string Estado { get; set; } = null!;
    public bool Eliminado { get; set; }
    public DateTime FechaRegistroUtc { get; set; }
    public string CreadoPorUsuario { get; set; } = null!;
    public string? ModificadoPorUsuario { get; set; }
    public DateTime? FechaModificacionUtc { get; set; }
    public string? ModificacionIp { get; set; }
}