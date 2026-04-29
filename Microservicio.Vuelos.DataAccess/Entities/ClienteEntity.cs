using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class ClienteEntity
    {
        public int IdCliente { get; set; }

        public Guid ClienteGuid { get; set; }

        public string TipoIdentificacion { get; set; } = null!;

        public string NumeroIdentificacion { get; set; } = null!;

        public string Nombres { get; set; } = null!;

        public string? Apellidos { get; set; }

        public string? RazonSocial { get; set; }

        public string Correo { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public int IdCiudadResidencia { get; set; }

        public int IdPaisNacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? Genero { get; set; }

        public string Estado { get; set; } = null!;

        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public string ServicioOrigen { get; set; } = null!;

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public string? MotivoInhabilitacion { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        // Navigation properties
        public virtual CiudadEntity CiudadResidencia { get; set; } = null!;

        public virtual PaisEntity PaisNacionalidad { get; set; } = null!;

        public virtual ICollection<UsuarioAppEntity> UsuariosApp { get; set; } = new HashSet<UsuarioAppEntity>();

        public virtual ICollection<PasajeroEntity> Pasajeros { get; set; } = new HashSet<PasajeroEntity>();

        public virtual ICollection<ReservaEntity> Reservas { get; set; } = new HashSet<ReservaEntity>();

        public virtual ICollection<FacturaEntity> Facturas { get; set; } = new HashSet<FacturaEntity>();
    }
}
