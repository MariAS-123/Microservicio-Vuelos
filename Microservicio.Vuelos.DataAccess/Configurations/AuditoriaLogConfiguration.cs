using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class AuditoriaLogConfiguration : IEntityTypeConfiguration<AuditoriaLogEntity>
    {
        public void Configure(EntityTypeBuilder<AuditoriaLogEntity> builder)
        {
            builder.ToTable("AUDITORIA_LOG", "crm");

            builder.HasKey(e => e.IdAuditoria)
                .HasName("PK_AUDITORIA_LOG");

            builder.Property(e => e.IdAuditoria)
                .HasColumnName("id_auditoria");

            builder.Property(e => e.AuditoriaGuid)
                .HasColumnName("auditoria_guid")
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.TablaAfectada)
                .HasColumnName("tabla_afectada")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Operacion)
                .HasColumnName("operacion")
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.IdRegistroAfectado)
                .HasColumnName("id_registro_afectado")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.DatosAnteriores)
                .HasColumnName("datos_anteriores")
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.DatosNuevos)
                .HasColumnName("datos_nuevos")
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.UsuarioEjecutor)
                .HasColumnName("usuario_ejecutor")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValueSql("SYSTEM_USER");

            builder.Property(e => e.IpOrigen)
                .HasColumnName("ip_origen")
                .HasMaxLength(45)
                .IsUnicode(false);

            builder.Property(e => e.FechaEventoUtc)
                .HasColumnName("fecha_evento_utc")
                .HasColumnType("datetime2(0)")
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(e => e.Activo)
                .HasColumnName("activo")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.HasIndex(e => e.AuditoriaGuid)
                .IsUnique()
                .HasDatabaseName("UQ_AUDITORIA_LOG_GUID");

            builder.HasIndex(e => e.AuditoriaGuid)
                .HasDatabaseName("IX_AUDITORIA_GUID");

            builder.HasIndex(e => new { e.TablaAfectada, e.FechaEventoUtc })
                .HasDatabaseName("IX_AUDITORIA_Tabla_Fecha");

            builder.HasIndex(e => new { e.Operacion, e.FechaEventoUtc })
                .HasDatabaseName("IX_AUDITORIA_Operacion");

            builder.HasIndex(e => new { e.UsuarioEjecutor, e.FechaEventoUtc })
                .HasDatabaseName("IX_AUDITORIA_usuario_ejecutor");

            builder.HasIndex(e => e.IdRegistroAfectado)
                .HasDatabaseName("IX_AUDITORIA_RegistroId");
        }
    }
}