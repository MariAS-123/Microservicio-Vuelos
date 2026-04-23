using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
    {
        public void Configure(EntityTypeBuilder<ClienteEntity> builder)
        {
            builder.ToTable("CLIENTES", "crm");

            builder.HasKey(e => e.IdCliente)
                .HasName("PK_CLIENTES");

            builder.Property(e => e.IdCliente)
                .HasColumnName("id_cliente");

            builder.Property(e => e.ClienteGuid)
                .HasColumnName("cliente_guid")
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.TipoIdentificacion)
                .HasColumnName("tipo_identificacion")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.NumeroIdentificacion)
                .HasColumnName("numero_identificacion")
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Nombres)
                .HasColumnName("nombres")
                .HasMaxLength(160)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Apellidos)
                .HasColumnName("apellidos")
                .HasMaxLength(160)
                .IsUnicode(false);

            builder.Property(e => e.RazonSocial)
                .HasColumnName("razon_social")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Correo)
                .HasColumnName("correo")
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(250)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.IdCiudadResidencia)
                .HasColumnName("id_ciudad_residencia")
                .IsRequired();

            builder.Property(e => e.IdPaisNacionalidad)
                .HasColumnName("id_pais_nacionalidad")
                .IsRequired();

            builder.Property(e => e.FechaNacimiento)
                .HasColumnName("fecha_nacimiento")
                .HasColumnType("date");

            builder.Property(e => e.Nacionalidad)
                .HasColumnName("nacionalidad")
                .HasMaxLength(80)
                .IsUnicode(false);

            builder.Property(e => e.Genero)
                .HasColumnName("genero")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasColumnType("char(3)")
                .IsRequired()
                .HasDefaultValue("ACT");

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

            builder.HasIndex(e => e.ClienteGuid)
                .IsUnique()
                .HasDatabaseName("UQ_CLIENTES_GUID");

            builder.HasIndex(e => e.NumeroIdentificacion)
                .IsUnique()
                .HasDatabaseName("UQ_CLIENTES_NUM_ID");

            builder.HasIndex(e => e.Correo)
                .IsUnique()
                .HasDatabaseName("UQ_CLIENTES_CORREO");

            builder.HasIndex(e => e.Correo)
                .HasDatabaseName("IX_CLIENTES_Correo");

            builder.HasIndex(e => e.NumeroIdentificacion)
                .HasDatabaseName("IX_CLIENTES_NumId");

            builder.HasOne(e => e.CiudadResidencia)
                .WithMany(c => c.ClientesResidencia)
                .HasForeignKey(e => e.IdCiudadResidencia)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CLIENTES_CIUDAD");

            builder.HasOne(e => e.PaisNacionalidad)
                .WithMany(p => p.ClientesNacionalidad)
                .HasForeignKey(e => e.IdPaisNacionalidad)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CLIENTES_PAIS");
        }
    }
}