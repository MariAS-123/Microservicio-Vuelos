using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRolEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioRolEntity> builder)
        {
            builder.ToTable("USUARIOS_ROLES", "seg");

            builder.HasKey(e => e.IdUsuarioRol)
                .HasName("PK_USUARIOS_ROLES");

            builder.Property(e => e.IdUsuarioRol)
                .HasColumnName("id_usuario_rol");

            builder.Property(e => e.IdUsuario)
                .HasColumnName("id_usuario")
                .IsRequired();

            builder.Property(e => e.IdRol)
                .HasColumnName("id_rol")
                .IsRequired();

            builder.Property(e => e.EstadoUsuarioRol)
                .HasColumnName("estado_usuario_rol")
                .HasColumnType("char(3)")
                .IsRequired()
                .HasDefaultValue("ACT");

            builder.Property(e => e.EsEliminado)
                .HasColumnName("es_eliminado")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.Activo)
                .HasColumnName("activo")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.CreadoPorUsuario)
                .HasColumnName("creado_por_usuario")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("SYSTEM");

            builder.Property(e => e.FechaRegistroUtc)
                .HasColumnName("fecha_registro_utc")
                .HasColumnType("datetime2(0)")
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(e => e.ModificadoPorUsuario)
                .HasColumnName("modificado_por_usuario")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.FechaModificacionUtc)
                .HasColumnName("fecha_modificacion_utc")
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.HasIndex(e => new { e.IdUsuario, e.IdRol })
                .IsUnique()
                .HasDatabaseName("UQ_USUARIOS_ROLES_USR_ROL");

            builder.HasIndex(e => e.IdUsuario)
                .HasDatabaseName("IX_USUARIOS_ROLES_USUARIO");

            builder.HasIndex(e => e.IdRol)
                .HasDatabaseName("IX_USUARIOS_ROLES_ROL");

            builder.HasOne(e => e.Usuario)
                .WithMany(u => u.UsuariosRoles)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_USUARIOS_ROLES_USUARIO");

            builder.HasOne(e => e.Rol)
                .WithMany(r => r.UsuariosRoles)
                .HasForeignKey(e => e.IdRol)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_USUARIOS_ROLES_ROL");
        }
    }
}