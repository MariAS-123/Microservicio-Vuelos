using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class UsuarioAppConfiguration : IEntityTypeConfiguration<UsuarioAppEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioAppEntity> builder)
        {
            builder.ToTable("USUARIO_APP", "seg");

            builder.HasKey(e => e.IdUsuario)
                .HasName("PK_USUARIO_APP");

            builder.Property(e => e.IdUsuario)
                .HasColumnName("id_usuario");

            builder.Property(e => e.UsuarioGuid)
                .HasColumnName("usuario_guid")
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.IdCliente)
                .HasColumnName("id_cliente");

            builder.Property(e => e.Username)
                .HasColumnName("username")
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Correo)
                .HasColumnName("correo")
                .HasMaxLength(120)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(500)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.PasswordSalt)
                .HasColumnName("password_salt")
                .HasMaxLength(250)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.FechaUltimoLogin)
                .HasColumnName("fecha_ultimo_login")
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.EstadoUsuario)
                .HasColumnName("estado_usuario")
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

            builder.Property(e => e.ModificacionIp)
                .HasColumnName("modificacion_ip")
                .HasMaxLength(45)
                .IsUnicode(false);

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.HasIndex(e => e.UsuarioGuid)
                .IsUnique()
                .HasDatabaseName("UQ_USUARIO_APP_GUID");

            builder.HasIndex(e => e.Username)
                .IsUnique()
                .HasDatabaseName("UQ_USUARIO_APP_USERNAME");

            builder.HasIndex(e => e.Correo)
                .IsUnique()
                .HasDatabaseName("UQ_USUARIO_APP_CORREO");

            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.UsuariosApp)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_USUARIO_APP_CLIENTE");
        }
    }
}