using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Vuelos.DataAccess.Context
{
    public class SistemaVuelosDBContext : DbContext
    {
        public SistemaVuelosDBContext(DbContextOptions<SistemaVuelosDBContext> options)
            : base(options)
        {
        }

        public DbSet<PaisEntity> Paises => Set<PaisEntity>();
        public DbSet<CiudadEntity> Ciudades => Set<CiudadEntity>();
        public DbSet<AeropuertoEntity> Aeropuertos => Set<AeropuertoEntity>();

        public DbSet<VueloEntity> Vuelos => Set<VueloEntity>();
        public DbSet<EscalaEntity> Escalas => Set<EscalaEntity>();
        public DbSet<AsientoEntity> Asientos => Set<AsientoEntity>();

        public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();
        public DbSet<UsuarioAppEntity> Usuarios => Set<UsuarioAppEntity>();
        public DbSet<RolEntity> Roles => Set<RolEntity>();
        public DbSet<UsuarioRolEntity> UsuariosRoles => Set<UsuarioRolEntity>();

        public DbSet<PasajeroEntity> Pasajeros => Set<PasajeroEntity>();
        public DbSet<ReservaEntity> Reservas => Set<ReservaEntity>();
        public DbSet<ReservaDetalleEntity> ReservaDetalles => Set<ReservaDetalleEntity>();
        public DbSet<FacturaEntity> Facturas => Set<FacturaEntity>();
        public DbSet<BoletoEntity> Boletos => Set<BoletoEntity>();
        public DbSet<EquipajeEntity> Equipajes => Set<EquipajeEntity>();

        public DbSet<AuditoriaLogEntity> AuditoriaLogs => Set<AuditoriaLogEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SistemaVuelosDBContext).Assembly);
        }
    }
}
