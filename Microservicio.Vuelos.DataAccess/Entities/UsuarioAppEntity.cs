using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class UsuarioAppEntity
    {
        public int IdUsuario { get; set; }

        public Guid UsuarioGuid { get; set; }

        public int? IdCliente { get; set; }

        public string Username { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string PasswordSalt { get; set; } = null!;

        public DateTime? FechaUltimoLogin { get; set; }

        public string EstadoUsuario { get; set; } = null!;

        public bool EsEliminado { get; set; }

        public bool Activo { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        // Navigation properties
        public virtual ClienteEntity? Cliente { get; set; }

        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; } = new HashSet<UsuarioRolEntity>();
    }
}