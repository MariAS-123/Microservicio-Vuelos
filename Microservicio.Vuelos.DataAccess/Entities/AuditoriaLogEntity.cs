using System;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class AuditoriaLogEntity
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

        public byte[] RowVersion { get; set; } = null!;
    }
}