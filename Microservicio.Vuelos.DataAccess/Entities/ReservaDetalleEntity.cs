using System;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class ReservaDetalleEntity
    {
        public int IdDetalle { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public int IdReserva { get; set; }

        public int IdPasajero { get; set; }

        public int IdAsiento { get; set; }

        public decimal SubtotalLinea { get; set; }

        public decimal ValorIvaLinea { get; set; }

        public decimal TotalLinea { get; set; }

        public string Estado { get; set; } = null!;

        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public virtual ReservaEntity Reserva { get; set; } = null!;

        public virtual PasajeroEntity Pasajero { get; set; } = null!;

        public virtual AsientoEntity Asiento { get; set; } = null!;

        public virtual BoletoEntity? Boleto { get; set; }
    }
}
