using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class BoletoEntity
    {
        public int IdBoleto { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public int IdReserva { get; set; }

        public int IdVuelo { get; set; }

        public int IdAsiento { get; set; }

        public int IdFactura { get; set; }

        public string CodigoBoleto { get; set; } = null!;

        public string Clase { get; set; } = null!;

        public decimal PrecioVueloBase { get; set; }

        public decimal PrecioAsientoExtra { get; set; }

        public decimal ImpuestosBoleto { get; set; }

        public decimal CargoEquipaje { get; set; }

        public decimal PrecioFinal { get; set; }

        public string EstadoBoleto { get; set; } = null!;

        public DateTime FechaEmision { get; set; }

        public bool EsEliminado { get; set; }

        public string Estado { get; set; } = null!;

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        // Navigation properties
        public virtual ReservaEntity Reserva { get; set; } = null!;

        public virtual VueloEntity Vuelo { get; set; } = null!;

        public virtual AsientoEntity Asiento { get; set; } = null!;

        public virtual FacturaEntity Factura { get; set; } = null!;

        public virtual ICollection<EquipajeEntity> Equipajes { get; set; } = new HashSet<EquipajeEntity>();
    }
}