namespace Microservicio.Vuelos.Business.DTOs.AuditoriaLog;

public class AuditoriaLogResponseDto
{
    public long IdAuditoria { get; set; }

    public Guid AuditoriaGuid { get; set; }

    public string TablaAfectada { get; set; } = null!;

    public string Operacion { get; set; } = null!;

    public string? IdRegistroAfectado { get; set; }

    public string? DatosAnteriores { get; set; }

    public string? DatosNuevos { get; set; }

    public string UsuarioEjecutor { get; set; } = null!;

    public string? IpOrigen { get; set; }

    public DateTime FechaEventoUtc { get; set; }

    public bool Activo { get; set; }
}