using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<RolEntity>
    {
        public void Configure(EntityTypeBuilder<RolEntity> builder)
        {
            builder.ToTable("ROL", "seg");

            builder.HasKey(e => e.IdRol)
                .HasName("PK_ROL");

            builder.Property(e => e.IdRol)
                .HasColumnName("id_rol");

            builder.Property(e => e.RolGuid)
                .HasColumnName("rol_guid")
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.NombreRol)
                .HasColumnName("nombre_rol")
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.DescripcionRol)
                .HasColumnName("descripcion_rol")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.EstadoRol)
                .HasColumnName("estado_rol")
                .HasColumnType("char(3)")
                .IsRequired()
                .HasDefaultValue("ACT");

            builder.Property(e => e.EsEliminado)
                .HasColumnName("es_eliminado")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.Activo)
                .HasColumnName("activo")
                .IsRequired()
                .HasDefaultValue(true);

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

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.HasIndex(e => e.RolGuid)
                .IsUnique()
                .HasDatabaseName("UQ_ROL_GUID");

            builder.HasIndex(e => e.NombreRol)
                .IsUnique()
                .HasDatabaseName("UQ_ROL_NOMBRE");
        }
    }
}