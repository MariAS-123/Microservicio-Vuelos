using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class ReservaDetalleConfiguration : IEntityTypeConfiguration<ReservaDetalleEntity>
    {
        public void Configure(EntityTypeBuilder<ReservaDetalleEntity> builder)
        {
            builder.ToTable("ReservaDetalle", "ventas");

            builder.HasKey(e => e.IdDetalle)
                .HasName("PK_ReservaDetalle");

            builder.Property(e => e.IdDetalle)
                .HasColumnName("id_detalle");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva")
                .IsRequired();

            builder.Property(e => e.IdPasajero)
                .HasColumnName("id_pasajero")
                .IsRequired();

            builder.Property(e => e.IdAsiento)
                .HasColumnName("id_asiento")
                .IsRequired();

            builder.Property(e => e.SubtotalLinea)
                .HasColumnName("subtotal_linea")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.ValorIvaLinea)
                .HasColumnName("valor_iva_linea")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.TotalLinea)
                .HasColumnName("total_linea")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

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

            builder.HasIndex(e => e.IdReserva)
                .HasDatabaseName("IX_RD_Reserva");

            builder.HasIndex(e => e.IdPasajero)
                .HasDatabaseName("IX_RD_Pasajero");

            builder.HasIndex(e => e.IdAsiento)
                .HasDatabaseName("IX_RD_Asiento");

            builder.HasIndex(e => new { e.IdReserva, e.IdPasajero })
                .IsUnique()
                .HasDatabaseName("UQ_RD_Pasajero_Reserva");

            builder.HasIndex(e => new { e.IdReserva, e.IdAsiento })
                .IsUnique()
                .HasDatabaseName("UQ_RD_Asiento_Reserva");

            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.Detalles)
                .HasForeignKey(e => e.IdReserva)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RD_Reserva");

            builder.HasOne(e => e.Pasajero)
                .WithMany(p => p.ReservaDetalles)
                .HasForeignKey(e => e.IdPasajero)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RD_Pasajero");

            builder.HasOne(e => e.Asiento)
                .WithMany(a => a.ReservaDetalles)
                .HasForeignKey(e => e.IdAsiento)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RD_Asiento");
        }
    }
}
