using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class AeropuertoEntity
    {
        public int IdAeropuerto { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public string CodigoIata { get; set; } = null!;

        public string? CodigoIcao { get; set; }

        public string Nombre { get; set; } = null!;

        public int? IdCiudad { get; set; }

        public int IdPais { get; set; }

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

        public virtual CiudadEntity? Ciudad { get; set; }

        public virtual PaisEntity Pais { get; set; } = null!;

        public virtual ICollection<VueloEntity> VuelosOrigen { get; set; } = new HashSet<VueloEntity>();

        public virtual ICollection<VueloEntity> VuelosDestino { get; set; } = new HashSet<VueloEntity>();

        public virtual ICollection<EscalaEntity> Escalas { get; set; } = new HashSet<EscalaEntity>();
    }
}