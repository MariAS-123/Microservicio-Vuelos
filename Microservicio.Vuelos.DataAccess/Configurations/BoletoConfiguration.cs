using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class BoletoConfiguration : IEntityTypeConfiguration<BoletoEntity>
    {
        public void Configure(EntityTypeBuilder<BoletoEntity> builder)
        {
            builder.ToTable("Boleto", "ventas");

            builder.HasKey(e => e.IdBoleto)
                .HasName("PK_Boleto");

            builder.Property(e => e.IdBoleto)
                .HasColumnName("id_boleto");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva")
                .IsRequired();

            builder.Property(e => e.IdVuelo)
                .HasColumnName("id_vuelo")
                .IsRequired();

            builder.Property(e => e.IdAsiento)
                .HasColumnName("id_asiento")
                .IsRequired();

            builder.Property(e => e.IdFactura)
                .HasColumnName("id_factura")
                .IsRequired();

            builder.Property(e => e.CodigoBoleto)
                .HasColumnName("codigo_boleto")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Clase)
                .HasColumnName("clase")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("ECONOMICA");

            builder.Property(e => e.PrecioVueloBase)
                .HasColumnName("precio_vuelo_base")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.PrecioAsientoExtra)
                .HasColumnName("precio_asiento_extra")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.ImpuestosBoleto)
                .HasColumnName("impuestos_boleto")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.CargoEquipaje)
                .HasColumnName("cargo_equipaje")
                .HasColumnType("decimal(8,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.PrecioFinal)
                .HasColumnName("precio_final")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.EstadoBoleto)
                .HasColumnName("estado_boleto")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("ACTIVO");

            builder.Property(e => e.FechaEmision)
                .HasColumnName("fecha_emision")
                .HasColumnType("datetime2(0)")
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(e => e.EsEliminado)
                .HasColumnName("es_eliminado")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("ACTIVO");

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

            builder.HasIndex(e => e.CodigoBoleto)
                .IsUnique()
                .HasDatabaseName("UQ_Boleto_Codigo");

            builder.HasIndex(e => e.IdReserva)
                .HasDatabaseName("IX_Boleto_Reserva");

            builder.HasIndex(e => e.IdVuelo)
                .HasDatabaseName("IX_Boleto_Vuelo");

            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.Boletos)
                .HasForeignKey(e => e.IdReserva)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Boleto_Reserva");

            builder.HasOne(e => e.Vuelo)
                .WithMany(v => v.Boletos)
                .HasForeignKey(e => e.IdVuelo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Boleto_Vuelo");

            builder.HasOne(e => e.Asiento)
                .WithMany(a => a.Boletos)
                .HasForeignKey(e => e.IdAsiento)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Boleto_Asiento");

            builder.HasOne(e => e.Factura)
                .WithMany(f => f.Boletos)
                .HasForeignKey(e => e.IdFactura)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Boleto_Factura");
        }
    }
}