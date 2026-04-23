using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class PaisConfiguration : IEntityTypeConfiguration<PaisEntity>
    {
        public void Configure(EntityTypeBuilder<PaisEntity> builder)
        {
            builder.ToTable("Pais", "aero");

            builder.HasKey(e => e.IdPais)
                .HasName("PK_Pais");

            builder.Property(e => e.IdPais)
                .HasColumnName("id_pais");

            builder.Property(e => e.CodigoIso2)
                .HasColumnName("codigo_iso2")
                .HasColumnType("char(2)")
                .IsRequired();

            builder.Property(e => e.CodigoIso3)
                .HasColumnName("codigo_iso3")
                .HasColumnType("char(3)");

            builder.Property(e => e.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Continente)
                .HasColumnName("continente")
                .HasMaxLength(50)
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

            builder.HasIndex(e => e.CodigoIso2)
                .IsUnique()
                .HasDatabaseName("UQ_Pais_iso2");

            builder.HasIndex(e => e.CodigoIso3)
                .IsUnique()
                .HasDatabaseName("UQ_Pais_iso3");

            builder.HasIndex(e => e.Nombre)
                .IsUnique()
                .HasDatabaseName("UQ_Pais_nombre");
        }
    }
}