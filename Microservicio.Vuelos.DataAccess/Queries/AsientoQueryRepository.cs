using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class AsientoQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public AsientoQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class AsientoMapaDto
        {
            public int IdAsiento { get; set; }
            public string NumeroAsiento { get; set; } = string.Empty;
            public string Clase { get; set; } = string.Empty;
            public bool Disponible { get; set; }
            public string? Pasajero { get; set; }
        }

        public async Task<List<AsientoMapaDto>> ObtenerMapaPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default)
        {
            return await _context.Asientos
                .AsNoTracking()
                .Where(a => a.IdVuelo == idVuelo && !a.Eliminado)
                .OrderBy(a => a.NumeroAsiento)
                .Select(a => new AsientoMapaDto
                {
                    IdAsiento = a.IdAsiento,
                    NumeroAsiento = a.NumeroAsiento,
                    Clase = a.Clase,
                    Disponible = a.Disponible,
                    Pasajero = a.Reservas
                        .Where(r => !r.EsEliminado)
                        .Select(r => r.Pasajero.NombrePasajero + " " + r.Pasajero.ApellidoPasajero)
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AsientoMapaDto>> ObtenerDisponiblesPorClaseAsync(int idVuelo, string clase, CancellationToken cancellationToken = default)
        {
            return await _context.Asientos
                .AsNoTracking()
                .Where(a => a.IdVuelo == idVuelo && a.Clase == clase && a.Disponible && !a.Eliminado)
                .OrderBy(a => a.NumeroAsiento)
                .Select(a => new AsientoMapaDto
                {
                    IdAsiento = a.IdAsiento,
                    NumeroAsiento = a.NumeroAsiento,
                    Clase = a.Clase,
                    Disponible = a.Disponible
                })
                .ToListAsync(cancellationToken);
        }

        public class ResumenOcupacionDto
        {
            public int IdVuelo { get; set; }
            public int TotalAsientos { get; set; }
            public int Disponibles { get; set; }
            public int Ocupados { get; set; }
        }

        public async Task<ResumenOcupacionDto?> ObtenerResumenOcupacionAsync(int idVuelo, CancellationToken cancellationToken = default)
        {
            return await _context.Vuelos
                .AsNoTracking()
                .Where(v => v.IdVuelo == idVuelo && !v.Eliminado)
                .Select(v => new ResumenOcupacionDto
                {
                    IdVuelo = v.IdVuelo,
                    TotalAsientos = v.Asientos.Count(a => !a.Eliminado),
                    Disponibles = v.Asientos.Count(a => a.Disponible && !a.Eliminado),
                    Ocupados = v.Asientos.Count(a => !a.Disponible && !a.Eliminado)
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}