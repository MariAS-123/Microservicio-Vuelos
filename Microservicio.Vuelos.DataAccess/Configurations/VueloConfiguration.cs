using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class VueloConfiguration : IEntityTypeConfiguration<VueloEntity>
    {
        public void Configure(EntityTypeBuilder<VueloEntity> builder)
        {
            builder.ToTable("Vuelo", "vuelos");

            builder.HasKey(e => e.IdVuelo)
                .HasName("PK_Vuelo");

            builder.Property(e => e.IdVuelo)
                .HasColumnName("id_vuelo");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.IdAeropuertoOrigen)
                .HasColumnName("id_aeropuerto_origen")
                .IsRequired();

            builder.Property(e => e.IdAeropuertoDestino)
                .HasColumnName("id_aeropuerto_destino")
                .IsRequired();

            builder.Property(e => e.NumeroVuelo)
                .HasColumnName("numero_vuelo")
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.FechaHoraSalida)
                .HasColumnName("fecha_hora_salida")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(e => e.FechaHoraLlegada)
                .HasColumnName("fecha_hora_llegada")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(e => e.DuracionMin)
                .HasColumnName("duracion_min")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.PrecioBase)
                .HasColumnName("precio_base")
                .HasColumnType("decimal(12,2)")
                .IsRequired();

            builder.Property(e => e.CapacidadTotal)
                .HasColumnName("capacidad_total")
                .IsRequired();

            builder.Property(e => e.EstadoVuelo)
                .HasColumnName("estado_vuelo")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("PROGRAMADO");

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

            builder.HasOne(e => e.AeropuertoOrigen)
                .WithMany(a => a.VuelosOrigen)
                .HasForeignKey(e => e.IdAeropuertoOrigen)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Vuelo_AeropuertoOrigen");

            builder.HasOne(e => e.AeropuertoDestino)
                .WithMany(a => a.VuelosDestino)
                .HasForeignKey(e => e.IdAeropuertoDestino)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Vuelo_AeropuertoDestino");
        }
    }
}