using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class PaisEntity
    {
        public int IdPais { get; set; }

        public string CodigoIso2 { get; set; } = null!;

        public string? CodigoIso3 { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Continente { get; set; }

        public string Estado { get; set; } = null!;

        public bool Eliminado { get; set; }

        // Navigation properties
        public virtual ICollection<CiudadEntity> Ciudades { get; set; } = new HashSet<CiudadEntity>();

        public virtual ICollection<AeropuertoEntity> Aeropuertos { get; set; } = new HashSet<AeropuertoEntity>();

        public virtual ICollection<ClienteEntity> ClientesNacionalidad { get; set; } = new HashSet<ClienteEntity>();
        public virtual ICollection<PasajeroEntity> PasajerosNacionalidad { get; set; } = new HashSet<PasajeroEntity>();
    }
}
