using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class PasajeroConfiguration : IEntityTypeConfiguration<PasajeroEntity>
    {
        public void Configure(EntityTypeBuilder<PasajeroEntity> builder)
        {
            builder.ToTable("Pasajero", "ventas");

            builder.HasKey(e => e.IdPasajero)
                .HasName("PK_Pasajero");

            builder.Property(e => e.IdPasajero)
                .HasColumnName("id_pasajero");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.IdCliente)
                .HasColumnName("id_cliente");

            builder.Property(e => e.NombrePasajero)
                .HasColumnName("nombre_pasajero")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.ApellidoPasajero)
                .HasColumnName("apellido_pasajero")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.TipoDocumentoPasajero)
                .HasColumnName("tipo_documento_pasajero")
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.NumeroDocumentoPasajero)
                .HasColumnName("numero_documento_pasajero")
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.FechaNacimientoPasajero)
                .HasColumnName("fecha_nacimiento_pasajero")
                .HasColumnType("date");

            builder.Property(e => e.IdPaisNacionalidad)
                .HasColumnName("id_pais_nacionalidad");

            builder.Property(e => e.EmailContactoPasajero)
                .HasColumnName("email_contacto_pasajero")
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.TelefonoContactoPasajero)
                .HasColumnName("telefono_contacto_pasajero")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.GeneroPasajero)
                .HasColumnName("genero_pasajero")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.RequiereAsistencia)
                .HasColumnName("requiere_asistencia")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.ObservacionesPasajero)
                .HasColumnName("observaciones_pasajero")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("ACTIVO");

            builder.Property(e => e.EsEliminado)
                .HasColumnName("es_eliminado")
                .IsRequired()
                .HasDefaultValue(false);

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

            builder.HasIndex(e => e.IdCliente)
                .HasDatabaseName("IX_Pasajero_Cliente");

            builder.HasIndex(e => e.IdPaisNacionalidad)
                .HasDatabaseName("IX_Pasajero_Pais");

            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Pasajeros)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Pasajero_Cliente");

            builder.HasOne(e => e.PaisNacionalidad)
                .WithMany(p => p.PasajerosNacionalidad)
                .HasForeignKey(e => e.IdPaisNacionalidad)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Pasajero_Pais");
        }
    }
}
