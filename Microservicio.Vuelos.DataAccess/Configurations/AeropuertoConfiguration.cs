using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class AeropuertoConfiguration : IEntityTypeConfiguration<AeropuertoEntity>
    {
        public void Configure(EntityTypeBuilder<AeropuertoEntity> builder)
        {
            builder.ToTable("Aeropuerto", "aero");

            builder.HasKey(e => e.IdAeropuerto)
                .HasName("PK_Aeropuerto");

            builder.Property(e => e.IdAeropuerto)
                .HasColumnName("id_aeropuerto");

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.Property(e => e.CodigoIata)
                .HasColumnName("codigo_iata")
                .HasColumnType("char(3)")
                .IsRequired();

            builder.Property(e => e.CodigoIcao)
                .HasColumnName("codigo_icao")
                .HasColumnType("char(4)");

            builder.Property(e => e.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.IdCiudad)
                .HasColumnName("id_ciudad");

            builder.Property(e => e.IdPais)
                .HasColumnName("id_pais")
                .IsRequired();

            builder.Property(e => e.ZonaHoraria)
                .HasColumnName("zona_horaria")
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Latitud)
                .HasColumnName("latitud")
                .HasColumnType("decimal(9,6)");

            builder.Property(e => e.Longitud)
                .HasColumnName("longitud")
                .HasColumnType("decimal(9,6)");

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

            builder.HasIndex(e => e.CodigoIata)
                .IsUnique()
                .HasDatabaseName("UQ_Aeropuerto_IATA");

            builder.HasOne(e => e.Pais)
                .WithMany(p => p.Aeropuertos)
                .HasForeignKey(e => e.IdPais)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Aeropuerto_Pais");

            builder.HasOne(e => e.Ciudad)
                .WithMany(c => c.Aeropuertos)
                .HasForeignKey(e => e.IdCiudad)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Aeropuerto_Ciudad");
        }
    }
}