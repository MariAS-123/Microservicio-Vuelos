using System;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class EscalaEntity
    {
        public int IdEscala { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public int IdVuelo { get; set; }

        public int IdAeropuerto { get; set; }

        public int Orden { get; set; }

        public DateTime FechaHoraLlegada { get; set; }

        public DateTime FechaHoraSalida { get; set; }

        public int DuracionMin { get; set; }

        public string TipoEscala { get; set; } = null!;

        public string? Terminal { get; set; }

        public string? Puerta { get; set; }

        public string? Observaciones { get; set; }

        public string Estado { get; set; } = null!;

        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public virtual VueloEntity Vuelo { get; set; } = null!;

        public virtual AeropuertoEntity Aeropuerto { get; set; } = null!;
    }
}