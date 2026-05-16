using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Microservicio.Vuelos.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "aero");

            migrationBuilder.EnsureSchema(
                name: "vuelos");

            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.EnsureSchema(
                name: "ventas");

            migrationBuilder.EnsureSchema(
                name: "seg");

            migrationBuilder.CreateTable(
                name: "AUDITORIA_LOG",
                schema: "crm",
                columns: table => new
                {
                    id_auditoria = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    auditoria_guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    tabla_afectada = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    operacion = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    id_registro_afectado = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    datos_anteriores = table.Column<string>(type: "text", nullable: true),
                    datos_nuevos = table.Column<string>(type: "text", nullable: true),
                    usuario_ejecutor = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValueSql: "'SYSTEM'"),
                    ip_origen = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true),
                    fecha_evento_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDITORIA_LOG", x => x.id_auditoria);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                schema: "aero",
                columns: table => new
                {
                    id_pais = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo_iso2 = table.Column<string>(type: "char(2)", nullable: false),
                    codigo_iso3 = table.Column<string>(type: "char(3)", nullable: true),
                    nombre = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    continente = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.id_pais);
                });

            migrationBuilder.CreateTable(
                name: "ROL",
                schema: "seg",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rol_guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    nombre_rol = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    descripcion_rol = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: true),
                    estado_rol = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ACT"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Ciudad",
                schema: "aero",
                columns: table => new
                {
                    id_ciudad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_pais = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    codigo_postal = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    zona_horaria = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudad", x => x.id_ciudad);
                    table.ForeignKey(
                        name: "FK_Ciudad_Pais",
                        column: x => x.id_pais,
                        principalSchema: "aero",
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Aeropuerto",
                schema: "aero",
                columns: table => new
                {
                    id_aeropuerto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    codigo_iata = table.Column<string>(type: "char(3)", nullable: false),
                    codigo_icao = table.Column<string>(type: "char(4)", nullable: true),
                    nombre = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    id_ciudad = table.Column<int>(type: "integer", nullable: true),
                    id_pais = table.Column<int>(type: "integer", nullable: false),
                    zona_horaria = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aeropuerto", x => x.id_aeropuerto);
                    table.ForeignKey(
                        name: "FK_Aeropuerto_Ciudad",
                        column: x => x.id_ciudad,
                        principalSchema: "aero",
                        principalTable: "Ciudad",
                        principalColumn: "id_ciudad",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aeropuerto_Pais",
                        column: x => x.id_pais,
                        principalSchema: "aero",
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTES",
                schema: "crm",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cliente_guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    tipo_identificacion = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    numero_identificacion = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    nombres = table.Column<string>(type: "character varying(160)", unicode: false, maxLength: 160, nullable: false),
                    apellidos = table.Column<string>(type: "character varying(160)", unicode: false, maxLength: 160, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: true),
                    correo = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    telefono = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    direccion = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: false),
                    id_ciudad_residencia = table.Column<int>(type: "integer", nullable: false),
                    id_pais_nacionalidad = table.Column<int>(type: "integer", nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "date", nullable: true),
                    genero = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    estado = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ACT"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true),
                    servicio_origen = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "VUELOS"),
                    fecha_inhabilitacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    motivo_inhabilitacion = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTES", x => x.id_cliente);
                    table.ForeignKey(
                        name: "FK_CLIENTES_CIUDAD",
                        column: x => x.id_ciudad_residencia,
                        principalSchema: "aero",
                        principalTable: "Ciudad",
                        principalColumn: "id_ciudad",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CLIENTES_PAIS",
                        column: x => x.id_pais_nacionalidad,
                        principalSchema: "aero",
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vuelo",
                schema: "vuelos",
                columns: table => new
                {
                    id_vuelo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_aeropuerto_origen = table.Column<int>(type: "integer", nullable: false),
                    id_aeropuerto_destino = table.Column<int>(type: "integer", nullable: false),
                    numero_vuelo = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    fecha_hora_salida = table.Column<DateTime>(type: "timestamp", nullable: false),
                    fecha_hora_llegada = table.Column<DateTime>(type: "timestamp", nullable: false),
                    duracion_min = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    precio_base = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    capacidad_total = table.Column<int>(type: "integer", nullable: false),
                    estado_vuelo = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "PROGRAMADO"),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vuelo", x => x.id_vuelo);
                    table.ForeignKey(
                        name: "FK_Vuelo_AeropuertoDestino",
                        column: x => x.id_aeropuerto_destino,
                        principalSchema: "aero",
                        principalTable: "Aeropuerto",
                        principalColumn: "id_aeropuerto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vuelo_AeropuertoOrigen",
                        column: x => x.id_aeropuerto_origen,
                        principalSchema: "aero",
                        principalTable: "Aeropuerto",
                        principalColumn: "id_aeropuerto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pasajero",
                schema: "ventas",
                columns: table => new
                {
                    id_pasajero = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_cliente = table.Column<int>(type: "integer", nullable: true),
                    nombre_pasajero = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    apellido_pasajero = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    tipo_documento_pasajero = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    numero_documento_pasajero = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    fecha_nacimiento_pasajero = table.Column<DateTime>(type: "date", nullable: true),
                    id_pais_nacionalidad = table.Column<int>(type: "integer", nullable: true),
                    email_contacto_pasajero = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: true),
                    telefono_contacto_pasajero = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    genero_pasajero = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    requiere_asistencia = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    observaciones_pasajero = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasajero", x => x.id_pasajero);
                    table.ForeignKey(
                        name: "FK_Pasajero_Cliente",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pasajero_Pais",
                        column: x => x.id_pais_nacionalidad,
                        principalSchema: "aero",
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO_APP",
                schema: "seg",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    id_cliente = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "character varying(120)", unicode: false, maxLength: 120, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    password_salt = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: false),
                    fecha_ultimo_login = table.Column<DateTime>(type: "timestamp", nullable: true),
                    estado_usuario = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ACT"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO_APP", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_USUARIO_APP_CLIENTE",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Asiento",
                schema: "vuelos",
                columns: table => new
                {
                    id_asiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    numero_asiento = table.Column<string>(type: "character varying(5)", unicode: false, maxLength: 5, nullable: false),
                    clase = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ECONOMICA"),
                    disponible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    precio_extra = table.Column<decimal>(type: "numeric(8,2)", nullable: false, defaultValue: 0m),
                    posicion = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asiento", x => x.id_asiento);
                    table.ForeignKey(
                        name: "FK_Asiento_Vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Escala",
                schema: "vuelos",
                columns: table => new
                {
                    id_escala = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    id_aeropuerto = table.Column<int>(type: "integer", nullable: false),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    fecha_hora_llegada = table.Column<DateTime>(type: "timestamp", nullable: false),
                    fecha_hora_salida = table.Column<DateTime>(type: "timestamp", nullable: false),
                    duracion_min = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    tipo_escala = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "COMERCIAL"),
                    terminal = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    puerta = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true),
                    observaciones = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escala", x => x.id_escala);
                    table.ForeignKey(
                        name: "FK_Escala_Aeropuerto",
                        column: x => x.id_aeropuerto,
                        principalSchema: "aero",
                        principalTable: "Aeropuerto",
                        principalColumn: "id_aeropuerto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Escala_Vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RESERVAS",
                schema: "ventas",
                columns: table => new
                {
                    id_reserva = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid_reserva = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    codigo_reserva = table.Column<string>(type: "character varying(40)", unicode: false, maxLength: 40, nullable: false),
                    id_cliente = table.Column<int>(type: "integer", nullable: false),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    fecha_reserva_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp", nullable: false),
                    subtotal_reserva = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    valor_iva = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    total_reserva = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    origen_canal_reserva = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "WEB"),
                    estado_reserva = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "PEN"),
                    fecha_confirmacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    fecha_cancelacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    motivo_cancelacion = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: true),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true),
                    servicio_origen = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "VUELOS"),
                    contacto_email = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: true),
                    contacto_telefono = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    observaciones = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    fecha_inhabilitacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    motivo_inhabilitacion = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVAS", x => x.id_reserva);
                    table.ForeignKey(
                        name: "FK_RESERVAS_Cliente",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RESERVAS_Vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS_ROLES",
                schema: "seg",
                columns: table => new
                {
                    id_usuario_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    id_rol = table.Column<int>(type: "integer", nullable: false),
                    estado_usuario_rol = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ACT"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS_ROLES", x => x.id_usuario_rol);
                    table.ForeignKey(
                        name: "FK_USUARIOS_ROLES_ROL",
                        column: x => x.id_rol,
                        principalSchema: "seg",
                        principalTable: "ROL",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USUARIOS_ROLES_USUARIO",
                        column: x => x.id_usuario,
                        principalSchema: "seg",
                        principalTable: "USUARIO_APP",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FACTURAS",
                schema: "ventas",
                columns: table => new
                {
                    id_factura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid_factura = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    id_cliente = table.Column<int>(type: "integer", nullable: false),
                    id_reserva = table.Column<int>(type: "integer", nullable: false),
                    numero_factura = table.Column<string>(type: "character varying(40)", unicode: false, maxLength: 40, nullable: false),
                    fecha_emision = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    subtotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    valor_iva = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    cargo_servicio = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    total = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    observaciones_factura = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    origen_canal_factura = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    estado = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ABI"),
                    fecha_inhabilitacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true),
                    servicio_origen = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "VUELOS"),
                    motivo_inhabilitacion = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FACTURAS", x => x.id_factura);
                    table.ForeignKey(
                        name: "FK_FACTURAS_Cliente",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FACTURAS_Reserva",
                        column: x => x.id_reserva,
                        principalSchema: "ventas",
                        principalTable: "RESERVAS",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservaDetalle",
                schema: "ventas",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_reserva = table.Column<int>(type: "integer", nullable: false),
                    id_pasajero = table.Column<int>(type: "integer", nullable: false),
                    id_asiento = table.Column<int>(type: "integer", nullable: false),
                    subtotal_linea = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    valor_iva_linea = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    total_linea = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaDetalle", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_RD_Asiento",
                        column: x => x.id_asiento,
                        principalSchema: "vuelos",
                        principalTable: "Asiento",
                        principalColumn: "id_asiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RD_Pasajero",
                        column: x => x.id_pasajero,
                        principalSchema: "ventas",
                        principalTable: "Pasajero",
                        principalColumn: "id_pasajero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RD_Reserva",
                        column: x => x.id_reserva,
                        principalSchema: "ventas",
                        principalTable: "RESERVAS",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Boleto",
                schema: "ventas",
                columns: table => new
                {
                    id_boleto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_reserva = table.Column<int>(type: "integer", nullable: false),
                    id_detalle = table.Column<int>(type: "integer", nullable: false),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    id_asiento = table.Column<int>(type: "integer", nullable: false),
                    id_factura = table.Column<int>(type: "integer", nullable: false),
                    codigo_boleto = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    clase = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ECONOMICA"),
                    precio_vuelo_base = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    precio_asiento_extra = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    impuestos_boleto = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    cargo_equipaje = table.Column<decimal>(type: "numeric(8,2)", nullable: false, defaultValue: 0m),
                    precio_final = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    estado_boleto = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    fecha_emision = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boleto", x => x.id_boleto);
                    table.ForeignKey(
                        name: "FK_Boleto_Asiento",
                        column: x => x.id_asiento,
                        principalSchema: "vuelos",
                        principalTable: "Asiento",
                        principalColumn: "id_asiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Boleto_Detalle",
                        column: x => x.id_detalle,
                        principalSchema: "ventas",
                        principalTable: "ReservaDetalle",
                        principalColumn: "id_detalle",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Boleto_Factura",
                        column: x => x.id_factura,
                        principalSchema: "ventas",
                        principalTable: "FACTURAS",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Boleto_Reserva",
                        column: x => x.id_reserva,
                        principalSchema: "ventas",
                        principalTable: "RESERVAS",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Boleto_Vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipaje",
                schema: "ventas",
                columns: table => new
                {
                    id_equipaje = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    id_boleto = table.Column<int>(type: "integer", nullable: false),
                    tipo = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    peso_kg = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    descripcion_equipaje = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: true),
                    precio_extra = table.Column<decimal>(type: "numeric(8,2)", nullable: false, defaultValue: 0m),
                    dimensiones_cm = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    numero_etiqueta = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "'EQ-' || floor(random()*1000000)::text"),
                    estado_equipaje = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "REGISTRADO"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    estado = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipaje", x => x.id_equipaje);
                    table.ForeignKey(
                        name: "FK_Equipaje_Boleto",
                        column: x => x.id_boleto,
                        principalSchema: "ventas",
                        principalTable: "Boleto",
                        principalColumn: "id_boleto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aeropuerto_id_ciudad",
                schema: "aero",
                table: "Aeropuerto",
                column: "id_ciudad");

            migrationBuilder.CreateIndex(
                name: "IX_Aeropuerto_id_pais",
                schema: "aero",
                table: "Aeropuerto",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "UQ_Aeropuerto_IATA",
                schema: "aero",
                table: "Aeropuerto",
                column: "codigo_iata",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Asiento_Vuelo_Num",
                schema: "vuelos",
                table: "Asiento",
                columns: new[] { "id_vuelo", "numero_asiento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AUDITORIA_GUID",
                schema: "crm",
                table: "AUDITORIA_LOG",
                column: "auditoria_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AUDITORIA_Operacion",
                schema: "crm",
                table: "AUDITORIA_LOG",
                columns: new[] { "operacion", "fecha_evento_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_AUDITORIA_RegistroId",
                schema: "crm",
                table: "AUDITORIA_LOG",
                column: "id_registro_afectado");

            migrationBuilder.CreateIndex(
                name: "IX_AUDITORIA_Tabla_Fecha",
                schema: "crm",
                table: "AUDITORIA_LOG",
                columns: new[] { "tabla_afectada", "fecha_evento_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_AUDITORIA_usuario_ejecutor",
                schema: "crm",
                table: "AUDITORIA_LOG",
                columns: new[] { "usuario_ejecutor", "fecha_evento_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_Boleto_id_asiento",
                schema: "ventas",
                table: "Boleto",
                column: "id_asiento");

            migrationBuilder.CreateIndex(
                name: "IX_Boleto_id_factura",
                schema: "ventas",
                table: "Boleto",
                column: "id_factura");

            migrationBuilder.CreateIndex(
                name: "IX_Boleto_Reserva",
                schema: "ventas",
                table: "Boleto",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "IX_Boleto_Vuelo",
                schema: "ventas",
                table: "Boleto",
                column: "id_vuelo");

            migrationBuilder.CreateIndex(
                name: "UQ_Boleto_Codigo",
                schema: "ventas",
                table: "Boleto",
                column: "codigo_boleto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Boleto_Detalle",
                schema: "ventas",
                table: "Boleto",
                column: "id_detalle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Ciudad_Nombre_Pais",
                schema: "aero",
                table: "Ciudad",
                columns: new[] { "id_pais", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_Correo",
                schema: "crm",
                table: "CLIENTES",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_id_ciudad_residencia",
                schema: "crm",
                table: "CLIENTES",
                column: "id_ciudad_residencia");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_id_pais_nacionalidad",
                schema: "crm",
                table: "CLIENTES",
                column: "id_pais_nacionalidad");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_NumId",
                schema: "crm",
                table: "CLIENTES",
                column: "numero_identificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CLIENTES_GUID",
                schema: "crm",
                table: "CLIENTES",
                column: "cliente_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipaje_Boleto",
                schema: "ventas",
                table: "Equipaje",
                column: "id_boleto");

            migrationBuilder.CreateIndex(
                name: "UQ_Equipaje_NumEtiqueta",
                schema: "ventas",
                table: "Equipaje",
                column: "numero_etiqueta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Escala_id_aeropuerto",
                schema: "vuelos",
                table: "Escala",
                column: "id_aeropuerto");

            migrationBuilder.CreateIndex(
                name: "UQ_Escala_Vuelo_Orden",
                schema: "vuelos",
                table: "Escala",
                columns: new[] { "id_vuelo", "orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FACTURAS_Cliente",
                schema: "ventas",
                table: "FACTURAS",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_FACTURAS_Estado",
                schema: "ventas",
                table: "FACTURAS",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "IX_FACTURAS_Fecha",
                schema: "ventas",
                table: "FACTURAS",
                column: "fecha_emision");

            migrationBuilder.CreateIndex(
                name: "IX_FACTURAS_Reserva",
                schema: "ventas",
                table: "FACTURAS",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "UQ_FACTURAS_GUID",
                schema: "ventas",
                table: "FACTURAS",
                column: "guid_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_FACTURAS_NUMERO",
                schema: "ventas",
                table: "FACTURAS",
                column: "numero_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Pais_iso2",
                schema: "aero",
                table: "Pais",
                column: "codigo_iso2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Pais_iso3",
                schema: "aero",
                table: "Pais",
                column: "codigo_iso3",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Pais_nombre",
                schema: "aero",
                table: "Pais",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pasajero_Cliente",
                schema: "ventas",
                table: "Pasajero",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pasajero_Pais",
                schema: "ventas",
                table: "Pasajero",
                column: "id_pais_nacionalidad");

            migrationBuilder.CreateIndex(
                name: "IX_RD_Asiento",
                schema: "ventas",
                table: "ReservaDetalle",
                column: "id_asiento");

            migrationBuilder.CreateIndex(
                name: "IX_RD_Pasajero",
                schema: "ventas",
                table: "ReservaDetalle",
                column: "id_pasajero");

            migrationBuilder.CreateIndex(
                name: "IX_RD_Reserva",
                schema: "ventas",
                table: "ReservaDetalle",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "UQ_RD_Asiento_Reserva",
                schema: "ventas",
                table: "ReservaDetalle",
                columns: new[] { "id_reserva", "id_asiento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_RD_Pasajero_Reserva",
                schema: "ventas",
                table: "ReservaDetalle",
                columns: new[] { "id_reserva", "id_pasajero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_Cliente",
                schema: "ventas",
                table: "RESERVAS",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_Estado",
                schema: "ventas",
                table: "RESERVAS",
                column: "estado_reserva");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_Fecha",
                schema: "ventas",
                table: "RESERVAS",
                column: "fecha_reserva_utc");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_Vuelo",
                schema: "ventas",
                table: "RESERVAS",
                column: "id_vuelo");

            migrationBuilder.CreateIndex(
                name: "UQ_RESERVAS_CODIGO",
                schema: "ventas",
                table: "RESERVAS",
                column: "codigo_reserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_RESERVAS_GUID",
                schema: "ventas",
                table: "RESERVAS",
                column: "guid_reserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ROL_GUID",
                schema: "seg",
                table: "ROL",
                column: "rol_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ROL_NOMBRE",
                schema: "seg",
                table: "ROL",
                column: "nombre_rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_APP_id_cliente",
                schema: "seg",
                table: "USUARIO_APP",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "UQ_USUARIO_APP_CORREO",
                schema: "seg",
                table: "USUARIO_APP",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_USUARIO_APP_GUID",
                schema: "seg",
                table: "USUARIO_APP",
                column: "usuario_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_USUARIO_APP_USERNAME",
                schema: "seg",
                table: "USUARIO_APP",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_ROLES_ROL",
                schema: "seg",
                table: "USUARIOS_ROLES",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_ROLES_USUARIO",
                schema: "seg",
                table: "USUARIOS_ROLES",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "UQ_USUARIOS_ROLES_USR_ROL",
                schema: "seg",
                table: "USUARIOS_ROLES",
                columns: new[] { "id_usuario", "id_rol" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vuelo_id_aeropuerto_destino",
                schema: "vuelos",
                table: "Vuelo",
                column: "id_aeropuerto_destino");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelo_id_aeropuerto_origen",
                schema: "vuelos",
                table: "Vuelo",
                column: "id_aeropuerto_origen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDITORIA_LOG",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "Equipaje",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "Escala",
                schema: "vuelos");

            migrationBuilder.DropTable(
                name: "USUARIOS_ROLES",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "Boleto",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "ROL",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "USUARIO_APP",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "ReservaDetalle",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "FACTURAS",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "Asiento",
                schema: "vuelos");

            migrationBuilder.DropTable(
                name: "Pasajero",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "RESERVAS",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "CLIENTES",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "Vuelo",
                schema: "vuelos");

            migrationBuilder.DropTable(
                name: "Aeropuerto",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "Ciudad",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "Pais",
                schema: "aero");
        }
    }
}
