using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class EquipajeConfiguration : IEntityTypeConfiguration<EquipajeEntity>
    {
        public void Configure(EntityTypeBuilder<EquipajeEntity> builder)
        {
            builder.ToTable("Equipaje", "ventas");

            builder.HasKey(e => e.IdEquipaje)
                .HasName("PK_Equipaje");

            builder.Property(e => e.IdEquipaje)
                .HasColumnName("id_equipaje");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.IdBoleto)
                .HasColumnName("id_boleto")
                .IsRequired();

            builder.Property(e => e.Tipo)
                .HasColumnName("tipo")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.PesoKg)
                .HasColumnName("peso_kg")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(e => e.DescripcionEquipaje)
                .HasColumnName("descripcion_equipaje")
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.PrecioExtra)
                .HasColumnName("precio_extra")
                .HasColumnType("decimal(8,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.DimensionesCm)
                .HasColumnName("dimensiones_cm")
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.NumeroEtiqueta)
                .HasColumnName("numero_etiqueta")
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'EQ-' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(20))");

            builder.Property(e => e.EstadoEquipaje)
                .HasColumnName("estado_equipaje")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("REGISTRADO");

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

            builder.HasIndex(e => e.NumeroEtiqueta)
                .IsUnique()
                .HasDatabaseName("UQ_Equipaje_NumEtiqueta");

            builder.HasIndex(e => e.IdBoleto)
                .HasDatabaseName("IX_Equipaje_Boleto");

            builder.HasOne(e => e.Boleto)
                .WithMany(b => b.Equipajes)
                .HasForeignKey(e => e.IdBoleto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Equipaje_Boleto");
        }
    }
}