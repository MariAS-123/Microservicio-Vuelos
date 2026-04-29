using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class BoletoQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public BoletoQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class EquipajeDto
        {
            public int IdEquipaje { get; set; }
            public string Tipo { get; set; } = string.Empty;
            public decimal PesoKg { get; set; }
            public string EstadoEquipaje { get; set; } = string.Empty;
        }

        public class BoletoDetalleDto
        {
            public int IdBoleto { get; set; }
            public string CodigoBoleto { get; set; } = string.Empty;
            public string Pasajero { get; set; } = string.Empty;
            public string NumeroVuelo { get; set; } = string.Empty;
            public string Asiento { get; set; } = string.Empty;
            public DateTime FechaSalida { get; set; }
            public DateTime FechaLlegada { get; set; }
            public string Origen { get; set; } = string.Empty;
            public string Destino { get; set; } = string.Empty;
            public decimal PrecioFinal { get; set; }
            public List<EquipajeDto> Equipajes { get; set; } = new();
        }

        public async Task<BoletoDetalleDto?> ObtenerDetalleCompletoAsync(int idBoleto, CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b => b.IdBoleto == idBoleto && !b.EsEliminado)
                .Select(b => new BoletoDetalleDto
                {
                    IdBoleto = b.IdBoleto,
                    CodigoBoleto = b.CodigoBoleto,
                    Pasajero = b.Detalle.Pasajero.NombrePasajero + " " + b.Detalle.Pasajero.ApellidoPasajero,
                    NumeroVuelo = b.Vuelo.NumeroVuelo,
                    Asiento = b.Asiento.NumeroAsiento,
                    FechaSalida = b.Vuelo.FechaHoraSalida,
                    FechaLlegada = b.Vuelo.FechaHoraLlegada,
                    Origen = b.Vuelo.AeropuertoOrigen.Nombre,
                    Destino = b.Vuelo.AeropuertoDestino.Nombre,
                    PrecioFinal = b.PrecioFinal,
                    Equipajes = b.Equipajes
                        .Where(e => !e.EsEliminado)
                        .Select(e => new EquipajeDto
                        {
                            IdEquipaje = e.IdEquipaje,
                            Tipo = e.Tipo,
                            PesoKg = e.PesoKg,
                            EstadoEquipaje = e.EstadoEquipaje
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<BoletoDetalleDto>> ObtenerPorReservaAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b => b.IdReserva == idReserva && !b.EsEliminado)
                .Select(b => new BoletoDetalleDto
                {
                    IdBoleto = b.IdBoleto,
                    CodigoBoleto = b.CodigoBoleto,
                    Pasajero = b.Detalle.Pasajero.NombrePasajero + " " + b.Detalle.Pasajero.ApellidoPasajero,
                    NumeroVuelo = b.Vuelo.NumeroVuelo,
                    Asiento = b.Asiento.NumeroAsiento,
                    FechaSalida = b.Vuelo.FechaHoraSalida,
                    FechaLlegada = b.Vuelo.FechaHoraLlegada,
                    Origen = b.Vuelo.AeropuertoOrigen.Nombre,
                    Destino = b.Vuelo.AeropuertoDestino.Nombre,
                    PrecioFinal = b.PrecioFinal,
                    Equipajes = b.Equipajes
                        .Where(e => !e.EsEliminado)
                        .Select(e => new EquipajeDto
                        {
                            IdEquipaje = e.IdEquipaje,
                            Tipo = e.Tipo,
                            PesoKg = e.PesoKg,
                            EstadoEquipaje = e.EstadoEquipaje
                        }).ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public class ManifiestoPasajeroDto
        {
            public string Pasajero { get; set; } = string.Empty;
            public string Documento { get; set; } = string.Empty;
            public string Asiento { get; set; } = string.Empty;
            public string CodigoBoleto { get; set; } = string.Empty;
        }

        public async Task<List<ManifiestoPasajeroDto>> ObtenerManifiestoPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
                .OrderBy(b => b.Asiento.NumeroAsiento)
                .Select(b => new ManifiestoPasajeroDto
                {
                    Pasajero = b.Detalle.Pasajero.NombrePasajero + " " + b.Detalle.Pasajero.ApellidoPasajero,
                    Documento = b.Detalle.Pasajero.TipoDocumentoPasajero + " " + b.Detalle.Pasajero.NumeroDocumentoPasajero,
                    Asiento = b.Asiento.NumeroAsiento,
                    CodigoBoleto = b.CodigoBoleto
                })
                .ToListAsync(cancellationToken);
        }
    }
}
