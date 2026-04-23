using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class AeropuertoQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public AeropuertoQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class AeropuertoDetalleDto
        {
            public int IdAeropuerto { get; set; }
            public string CodigoIata { get; set; } = string.Empty;
            public string Nombre { get; set; } = string.Empty;
            public string? Ciudad { get; set; }
            public string Pais { get; set; } = string.Empty;
        }

        public async Task<List<AeropuertoDetalleDto>> ObtenerConCiudadYPaisAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AsNoTracking()
                .Where(a => !a.Eliminado)
                .Select(a => new AeropuertoDetalleDto
                {
                    IdAeropuerto = a.IdAeropuerto,
                    CodigoIata = a.CodigoIata,
                    Nombre = a.Nombre,
                    Ciudad = a.Ciudad != null && !a.Ciudad.Eliminado ? a.Ciudad.Nombre : null,
                    Pais = a.Pais.Nombre
                })
                .ToListAsync(cancellationToken);
        }

        public class VueloAeropuertoDto
        {
            public int IdVuelo { get; set; }
            public string NumeroVuelo { get; set; } = string.Empty;
            public DateTime FechaHoraSalida { get; set; }
            public DateTime FechaHoraLlegada { get; set; }
            public string TipoMovimiento { get; set; } = string.Empty;
        }

        public async Task<List<VueloAeropuertoDto>> ObtenerVuelosSaliendoAsync(int idAeropuerto, CancellationToken cancellationToken = default)
        {
            return await _context.Vuelos
                .AsNoTracking()
                .Where(v => v.IdAeropuertoOrigen == idAeropuerto && !v.Eliminado)
                .Select(v => new VueloAeropuertoDto
                {
                    IdVuelo = v.IdVuelo,
                    NumeroVuelo = v.NumeroVuelo,
                    FechaHoraSalida = v.FechaHoraSalida,
                    FechaHoraLlegada = v.FechaHoraLlegada,
                    TipoMovimiento = "SALIDA"
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VueloAeropuertoDto>> ObtenerVuelosLlegandoAsync(int idAeropuerto, CancellationToken cancellationToken = default)
        {
            return await _context.Vuelos
                .AsNoTracking()
                .Where(v => v.IdAeropuertoDestino == idAeropuerto && !v.Eliminado)
                .Select(v => new VueloAeropuertoDto
                {
                    IdVuelo = v.IdVuelo,
                    NumeroVuelo = v.NumeroVuelo,
                    FechaHoraSalida = v.FechaHoraSalida,
                    FechaHoraLlegada = v.FechaHoraLlegada,
                    TipoMovimiento = "LLEGADA"
                })
                .ToListAsync(cancellationToken);
        }

        public class ResumenOperativoAeropuertoDto
        {
            public int IdAeropuerto { get; set; }
            public string Aeropuerto { get; set; } = string.Empty;
            public int Salidas { get; set; }
            public int Llegadas { get; set; }
            public int Escalas { get; set; }
        }

        public async Task<ResumenOperativoAeropuertoDto?> ObtenerResumenOperativoAsync(int idAeropuerto, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AsNoTracking()
                .Where(a => a.IdAeropuerto == idAeropuerto && !a.Eliminado)
                .Select(a => new ResumenOperativoAeropuertoDto
                {
                    IdAeropuerto = a.IdAeropuerto,
                    Aeropuerto = a.Nombre,
                    Salidas = a.VuelosOrigen.Count(v => !v.Eliminado),
                    Llegadas = a.VuelosDestino.Count(v => !v.Eliminado),
                    Escalas = a.Escalas.Count(e => !e.Eliminado)
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}