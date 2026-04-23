namespace Microservicio.Vuelos.DataManagement.Models;

public class VueloDataModel
{
    public int IdVuelo { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public int IdAeropuertoOrigen { get; set; }

    public int IdAeropuertoDestino { get; set; }

    public string NumeroVuelo { get; set; } = null!;

    public DateTime FechaHoraSalida { get; set; }

    public DateTime FechaHoraLlegada { get; set; }

    public int DuracionMin { get; set; }

    public decimal PrecioBase { get; set; }

    public int CapacidadTotal { get; set; }

    public string EstadoVuelo { get; set; } = null!; // PROGRAMADO, EN_VUELO, ATERRIZADO, CANCELADO, DEMORADO

    public string Estado { get; set; } = null!; // ACTIVO, INACTIVO

    public bool Eliminado { get; set; }

    public DateTime FechaRegistroUtc { get; set; }

    public string CreadoPorUsuario { get; set; } = null!;

    public string? ModificadoPorUsuario { get; set; }

    public DateTime? FechaModificacionUtc { get; set; }

    public string? ModificacionIp { get; set; }
}