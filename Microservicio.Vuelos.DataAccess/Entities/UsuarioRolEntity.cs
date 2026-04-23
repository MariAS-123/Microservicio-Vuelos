using System;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class UsuarioRolEntity
    {
        public int IdUsuarioRol { get; set; }

        public int IdUsuario { get; set; }

        public int IdRol { get; set; }

        public string EstadoUsuarioRol { get; set; } = null!;

        public bool EsEliminado { get; set; }

        public bool Activo { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        // Navigation properties
        public virtual UsuarioAppEntity Usuario { get; set; } = null!;

        public virtual RolEntity Rol { get; set; } = null!;
    }
}