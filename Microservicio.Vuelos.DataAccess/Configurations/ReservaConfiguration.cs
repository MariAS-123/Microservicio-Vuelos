using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<ReservaEntity>
    {
        public void Configure(EntityTypeBuilder<ReservaEntity> builder)
        {
            builder.ToTable("RESERVAS", "ventas");

            builder.HasKey(e => e.IdReserva)
                .HasName("PK_RESERVAS");

            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva");

            builder.Property(e => e.GuidReserva)
                .HasColumnName("guid_reserva")
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.CodigoReserva)
                .HasColumnName("codigo_reserva")
                .HasMaxLength(40)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();

            builder.Property(e => e.IdPasajero)
                .HasColumnName("id_pasajero")
                .IsRequired();

            builder.Property(e => e.IdVuelo)
                .HasColumnName("id_vuelo")
                .IsRequired();

            builder.Property(e => e.IdAsiento)
                .HasColumnName("id_asiento")
                .IsRequired();

            builder.Property(e => e.FechaReservaUtc)
                .HasColumnName("fecha_reserva_utc")
                .HasColumnType("datetime2(0)")
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(e => e.FechaInicio)
                .HasColumnName("fecha_inicio")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(e => e.FechaFin)
                .HasColumnName("fecha_fin")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(e => e.SubtotalReserva)
                .HasColumnName("subtotal_reserva")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.ValorIva)
                .HasColumnName("valor_iva")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.TotalReserva)
                .HasColumnName("total_reserva")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.OrigenCanalReserva)
                .HasColumnName("origen_canal_reserva")
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired()
                .HasDefaultValue("WEB");

            builder.Property(e => e.EstadoReserva)
                .HasColumnName("estado_reserva")
                .HasColumnType("char(3)")
                .IsRequired()
                .HasDefaultValue("PEN");

            builder.Property(e => e.FechaConfirmacionUtc)
                .HasColumnName("fecha_confirmacion_utc")
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.FechaCancelacionUtc)
                .HasColumnName("fecha_cancelacion_utc")
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.MotivoCancelacion)
                .HasColumnName("motivo_cancelacion")
                .HasMaxLength(250)
                .IsUnicode(false);

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

            builder.Property(e => e.ContactoEmail)
                .HasColumnName("contacto_email")
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.ContactoTelefono)
                .HasColumnName("contacto_telefono")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.FechaInhabilitacionUtc)
                .HasColumnName("fecha_inhabilitacion_utc")
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.MotivoInhabilitacion)
                .HasColumnName("motivo_inhabilitacion")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            builder.HasIndex(e => e.GuidReserva)
                .IsUnique()
                .HasDatabaseName("UQ_RESERVAS_GUID");

            builder.HasIndex(e => e.CodigoReserva)
                .IsUnique()
                .HasDatabaseName("UQ_RESERVAS_CODIGO");

            builder.HasIndex(e => new { e.IdVuelo, e.IdAsiento })
                .IsUnique()
                .HasDatabaseName("UQ_RESERVAS_Vuelo_Asiento");

            builder.HasIndex(e => new { e.IdVuelo, e.IdPasajero })
                .IsUnique()
                .HasDatabaseName("UQ_RESERVAS_Vuelo_Pasajero");

            builder.HasIndex(e => e.IdCliente)
                .HasDatabaseName("IX_RESERVAS_Cliente");

            builder.HasIndex(e => e.IdVuelo)
                .HasDatabaseName("IX_RESERVAS_Vuelo");

            builder.HasIndex(e => e.EstadoReserva)
                .HasDatabaseName("IX_RESERVAS_Estado");

            builder.HasIndex(e => e.FechaReservaUtc)
                .HasDatabaseName("IX_RESERVAS_Fecha");

            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RESERVAS_Cliente");

            builder.HasOne(e => e.Pasajero)
                .WithMany(p => p.Reservas)
                .HasForeignKey(e => e.IdPasajero)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RESERVAS_Pasajero");

            builder.HasOne(e => e.Vuelo)
                .WithMany(v => v.Reservas)
                .HasForeignKey(e => e.IdVuelo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RESERVAS_Vuelo");

            builder.HasOne(e => e.Asiento)
                .WithMany(a => a.Reservas)
                .HasForeignKey(e => e.IdAsiento)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RESERVAS_Asiento");
        }
    }
}