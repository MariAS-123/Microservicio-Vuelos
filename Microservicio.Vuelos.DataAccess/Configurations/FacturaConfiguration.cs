using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class FacturaConfiguration : IEntityTypeConfiguration<FacturaEntity>
    {
        public void Configure(EntityTypeBuilder<FacturaEntity> builder)
        {
            builder.ToTable("FACTURAS", "ventas");

            builder.HasKey(e => e.IdFactura)
                .HasName("PK_FACTURAS");

            builder.Property(e => e.IdFactura)
                .HasColumnName("id_factura");

            builder.Property(e => e.GuidFactura)
                .HasColumnName("guid_factura")
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();

            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva")
                .IsRequired();

            builder.Property(e => e.NumeroFactura)
                .HasColumnName("numero_factura")
                .HasMaxLength(40)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.FechaEmision)
                .HasColumnName("fecha_emision")
                .HasColumnType("datetime2(0)")
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(e => e.Subtotal)
                .HasColumnName("subtotal")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.ValorIva)
                .HasColumnName("valor_iva")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.CargoServicio)
                .HasColumnName("cargo_servicio")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.Total)
                .HasColumnName("total")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.ObservacionesFactura)
                .HasColumnName("observaciones_factura")
                .HasMaxLength(300)
                .IsUnicode(false);

            builder.Property(e => e.OrigenCanalFactura)
                .HasColumnName("origen_canal_factura")
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasColumnType("char(3)")
                .IsRequired()
                .HasDefaultValue("ABI");

            builder.Property(e => e.FechaInhabilitacionUtc)
                .HasColumnName("fecha_inhabilitacion_utc")
                .HasColumnType("datetime2(0)");

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

            builder.Property(e => e.ServicioOrigen)
                .HasColumnName("servicio_origen")
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("VUELOS");

            builder.Property(e => e.MotivoInhabilitacion)
                .HasColumnName("motivo_inhabilitacion")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.HasIndex(e => e.GuidFactura)
                .IsUnique()
                .HasDatabaseName("UQ_FACTURAS_GUID");

            builder.HasIndex(e => e.NumeroFactura)
                .IsUnique()
                .HasDatabaseName("UQ_FACTURAS_NUMERO");

            builder.HasIndex(e => e.IdCliente)
                .HasDatabaseName("IX_FACTURAS_Cliente");

            builder.HasIndex(e => e.IdReserva)
                .HasDatabaseName("IX_FACTURAS_Reserva");

            builder.HasIndex(e => e.Estado)
                .HasDatabaseName("IX_FACTURAS_Estado");

            builder.HasIndex(e => e.FechaEmision)
                .HasDatabaseName("IX_FACTURAS_Fecha");

            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_FACTURAS_Cliente");

            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.Facturas)
                .HasForeignKey(e => e.IdReserva)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_FACTURAS_Reserva");
        }
    }
}