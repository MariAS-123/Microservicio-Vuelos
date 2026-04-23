using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class EscalaConfiguration : IEntityTypeConfiguration<EscalaEntity>
    {
        public void Configure(EntityTypeBuilder<EscalaEntity> builder)
        {
            builder.ToTable("Escala", "vuelos");

            builder.HasKey(e => e.IdEscala)
                .HasName("PK_Escala");

            builder.Property(e => e.IdEscala)
                .HasColumnName("id_escala");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.IdVuelo)
                .HasColumnName("id_vuelo")
                .IsRequired();

            builder.Property(e => e.IdAeropuerto)
                .HasColumnName("id_aeropuerto")
                .IsRequired();

            builder.Property(e => e.Orden)
                .HasColumnName("orden")
                .IsRequired();

            builder.Property(e => e.FechaHoraLlegada)
                .HasColumnName("fecha_hora_llegada")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(e => e.FechaHoraSalida)
                .HasColumnName("fecha_hora_salida")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(e => e.DuracionMin)
                .HasColumnName("duracion_min")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.TipoEscala)
                .HasColumnName("tipo_escala")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("COMERCIAL");

            builder.Property(e => e.Terminal)
                .HasColumnName("terminal")
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Puerta)
                .HasColumnName("puerta")
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.Observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("ACTIVO");

            builder.Property(e => e.Eliminado)
                .HasColumnName("eliminado")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.FechaRegistroUtc)
                .HasColumnName("fecha_registro_utc")
                .HasColumnType("datetime2(0)")
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(e => e.CreadoPorUsuario)
                .HasColumnName("creado_por_usuario")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("SYSTEM");

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

            builder.HasIndex(e => new { e.IdVuelo, e.Orden })
                .IsUnique()
                .HasDatabaseName("UQ_Escala_Vuelo_Orden");

            builder.HasOne(e => e.Vuelo)
                .WithMany(v => v.Escalas)
                .HasForeignKey(e => e.IdVuelo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Escala_Vuelo");

            builder.HasOne(e => e.Aeropuerto)
                .WithMany(a => a.Escalas)
                .HasForeignKey(e => e.IdAeropuerto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Escala_Aeropuerto");
        }
    }
}