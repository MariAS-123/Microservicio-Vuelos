using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class AsientoConfiguration : IEntityTypeConfiguration<AsientoEntity>
    {
        public void Configure(EntityTypeBuilder<AsientoEntity> builder)
        {
            builder.ToTable("Asiento", "vuelos");

            builder.HasKey(e => e.IdAsiento)
                .HasName("PK_Asiento");

            builder.Property(e => e.IdAsiento)
                .HasColumnName("id_asiento");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.IdVuelo)
                .HasColumnName("id_vuelo")
                .IsRequired();

            builder.Property(e => e.NumeroAsiento)
                .HasColumnName("numero_asiento")
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Clase)
                .HasColumnName("clase")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("ECONOMICA");

            builder.Property(e => e.Disponible)
                .HasColumnName("disponible")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.PrecioExtra)
                .HasColumnName("precio_extra")
                .HasColumnType("decimal(8,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.Posicion)
                .HasColumnName("posicion")
                .HasMaxLength(20)
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

            builder.HasIndex(e => new { e.IdVuelo, e.NumeroAsiento })
                .IsUnique()
                .HasDatabaseName("UQ_Asiento_Vuelo_Num");

            builder.HasOne(e => e.Vuelo)
                .WithMany(v => v.Asientos)
                .HasForeignKey(e => e.IdVuelo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Asiento_Vuelo");
        }
    }
}