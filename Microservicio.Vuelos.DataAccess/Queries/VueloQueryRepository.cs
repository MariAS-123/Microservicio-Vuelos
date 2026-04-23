using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class VueloQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public VueloQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class VueloDisponibleDto
        {
            public int IdVuelo { get; set; }
            public string NumeroVuelo { get; set; } = string.Empty;
            public DateTime FechaHoraSalida { get; set; }
            public DateTime FechaHoraLlegada { get; set; }
            public decimal PrecioBase { get; set; }
            public string AeropuertoOrigen { get; set; } = string.Empty;
            public string AeropuertoDestino { get; set; } = string.Empty;
            public int AsientosDisponibles { get; set; }
        }

        public async Task<List<VueloDisponibleDto>> BuscarDisponiblesAsync(
            int idAeropuertoOrigen,
            int idAeropuertoDestino,
            DateTime fecha,
            CancellationToken cancellationToken = default)
        {
            var fechaInicio = fecha.Date;
            var fechaFin = fechaInicio.AddDays(1);

            return await _context.Vuelos
                .AsNoTracking()
                .Where(v =>
                    v.IdAeropuertoOrigen == idAeropuertoOrigen &&
                    v.IdAeropuertoDestino == idAeropuertoDestino &&
                    v.FechaHoraSalida >= fechaInicio &&
                    v.FechaHoraSalida < fechaFin &&
                    v.EstadoVuelo == "PROGRAMADO" &&
                    !v.Eliminado)
                .Select(v => new VueloDisponibleDto
                {
                    IdVuelo = v.IdVuelo,
                    NumeroVuelo = v.NumeroVuelo,
                    FechaHoraSalida = v.FechaHoraSalida,
                    FechaHoraLlegada = v.FechaHoraLlegada,
                    PrecioBase = v.PrecioBase,
                    AeropuertoOrigen = v.AeropuertoOrigen.Nombre,
                    AeropuertoDestino = v.AeropuertoDestino.Nombre,
                    AsientosDisponibles = v.Asientos.Count(a => a.Disponible && !a.Eliminado)
                })
                .ToListAsync(cancellationToken);
        }

        public class EscalaDto
        {
            public int Orden { get; set; }
            public string Aeropuerto { get; set; } = string.Empty;
            public DateTime FechaHoraLlegada { get; set; }
            public DateTime FechaHoraSalida { get; set; }
            public string TipoEscala { get; set; } = string.Empty;
        }

        public class VueloDetalleDto
        {
            public int IdVuelo { get; set; }
            public string NumeroVuelo { get; set; } = string.Empty;
            public DateTime FechaHoraSalida { get; set; }
            public DateTime FechaHoraLlegada { get; set; }
            public decimal PrecioBase { get; set; }
            public int CapacidadTotal { get; set; }
            public string AeropuertoOrigen { get; set; } = string.Empty;
            public string AeropuertoDestino { get; set; } = string.Empty;
            public int AsientosDisponibles { get; set; }
            public int BoletosEmitidos { get; set; }
            public List<EscalaDto> Escalas { get; set; } = new();
        }

        public async Task<VueloDetalleDto?> ObtenerDetalleCompletoAsync(int idVuelo, CancellationToken cancellationToken = default)
        {
            return await _context.Vuelos
                .AsNoTracking()
                .Where(v => v.IdVuelo == idVuelo && !v.Eliminado)
                .Select(v => new VueloDetalleDto
                {
                    IdVuelo = v.IdVuelo,
                    NumeroVuelo = v.NumeroVuelo,
                    FechaHoraSalida = v.FechaHoraSalida,
                    FechaHoraLlegada = v.FechaHoraLlegada,
                    PrecioBase = v.PrecioBase,
                    CapacidadTotal = v.CapacidadTotal,
                    AeropuertoOrigen = v.AeropuertoOrigen.Nombre,
                    AeropuertoDestino = v.AeropuertoDestino.Nombre,
                    AsientosDisponibles = v.Asientos.Count(a => a.Disponible && !a.Eliminado),
                    BoletosEmitidos = v.Boletos.Count(b => !b.EsEliminado),
                    Escalas = v.Escalas
                        .Where(e => !e.Eliminado)
                        .OrderBy(e => e.Orden)
                        .Select(e => new EscalaDto
                        {
                            Orden = e.Orden,
                            Aeropuerto = e.Aeropuerto.Nombre,
                            FechaHoraLlegada = e.FechaHoraLlegada,
                            FechaHoraSalida = e.FechaHoraSalida,
                            TipoEscala = e.TipoEscala
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public class VueloOcupacionDto
        {
            public int IdVuelo { get; set; }
            public string NumeroVuelo { get; set; } = string.Empty;
            public int CapacidadTotal { get; set; }
            public int AsientosDisponibles { get; set; }
            public int AsientosOcupados { get; set; }
        }

        public async Task<List<VueloOcupacionDto>> ObtenerOcupacionPorRangoAsync(DateTime desde, DateTime hasta, CancellationToken cancellationToken = default)
        {
            return await _context.Vuelos
                .AsNoTracking()
                .Where(v => v.FechaHoraSalida >= desde && v.FechaHoraSalida <= hasta && !v.Eliminado)
                .Select(v => new VueloOcupacionDto
                {
                    IdVuelo = v.IdVuelo,
                    NumeroVuelo = v.NumeroVuelo,
                    CapacidadTotal = v.CapacidadTotal,
                    AsientosDisponibles = v.Asientos.Count(a => a.Disponible && !a.Eliminado),
                    AsientosOcupados = v.Asientos.Count(a => !a.Disponible && !a.Eliminado)
                })
                .ToListAsync(cancellationToken);
        }
    }
}