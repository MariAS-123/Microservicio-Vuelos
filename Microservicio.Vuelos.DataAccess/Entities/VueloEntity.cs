using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class VueloEntity
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

        public string EstadoVuelo { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public virtual AeropuertoEntity AeropuertoOrigen { get; set; } = null!;

        public virtual AeropuertoEntity AeropuertoDestino { get; set; } = null!;

        public virtual ICollection<EscalaEntity> Escalas { get; set; } = new HashSet<EscalaEntity>();

        public virtual ICollection<AsientoEntity> Asientos { get; set; } = new HashSet<AsientoEntity>();

        public virtual ICollection<ReservaEntity> Reservas { get; set; } = new HashSet<ReservaEntity>();

        public virtual ICollection<BoletoEntity> Boletos { get; set; } = new HashSet<BoletoEntity>();
    }
}