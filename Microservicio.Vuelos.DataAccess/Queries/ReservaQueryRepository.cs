using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class ReservaQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public ReservaQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class ReservaCompletaDto
        {
            public int IdReserva { get; set; }
            public Guid GuidReserva { get; set; }
            public string CodigoReserva { get; set; } = string.Empty;
            public string EstadoReserva { get; set; } = string.Empty;
            public DateTime FechaReservaUtc { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public decimal SubtotalReserva { get; set; }
            public decimal ValorIva { get; set; }
            public decimal TotalReserva { get; set; }
            public ClienteDto Cliente { get; set; } = new();
            public PasajeroDto Pasajero { get; set; } = new();
            public VueloDto Vuelo { get; set; } = new();
            public AsientoDto Asiento { get; set; } = new();
            public List<FacturaDto> Facturas { get; set; } = new();
            public List<BoletoDto> Boletos { get; set; } = new();
        }

        public class ClienteDto
        {
            public int IdCliente { get; set; }
            public string Nombres { get; set; } = string.Empty;
            public string? Apellidos { get; set; }
            public string Correo { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
        }

        public class PasajeroDto
        {
            public int IdPasajero { get; set; }
            public string NombrePasajero { get; set; } = string.Empty;
            public string ApellidoPasajero { get; set; } = string.Empty;
            public string TipoDocumentoPasajero { get; set; } = string.Empty;
            public string NumeroDocumentoPasajero { get; set; } = string.Empty;
        }

        public class VueloDto
        {
            public int IdVuelo { get; set; }
            public string NumeroVuelo { get; set; } = string.Empty;
            public DateTime FechaHoraSalida { get; set; }
            public DateTime FechaHoraLlegada { get; set; }
            public string AeropuertoOrigen { get; set; } = string.Empty;
            public string AeropuertoDestino { get; set; } = string.Empty;
        }

        public class AsientoDto
        {
            public int IdAsiento { get; set; }
            public string NumeroAsiento { get; set; } = string.Empty;
            public string Clase { get; set; } = string.Empty;
        }

        public class FacturaDto
        {
            public int IdFactura { get; set; }
            public string NumeroFactura { get; set; } = string.Empty;
            public decimal Total { get; set; }
            public string Estado { get; set; } = string.Empty;
            public DateTime FechaEmision { get; set; }
        }

        public class BoletoDto
        {
            public int IdBoleto { get; set; }
            public string CodigoBoleto { get; set; } = string.Empty;
            public decimal PrecioFinal { get; set; }
            public string EstadoBoleto { get; set; } = string.Empty;
        }

        public async Task<ReservaCompletaDto?> ObtenerDetalleCompletoPorIdAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r => r.IdReserva == idReserva && !r.EsEliminado)
                .Select(r => new ReservaCompletaDto
                {
                    IdReserva = r.IdReserva,
                    GuidReserva = r.GuidReserva,
                    CodigoReserva = r.CodigoReserva,
                    EstadoReserva = r.EstadoReserva,
                    FechaReservaUtc = r.FechaReservaUtc,
                    FechaInicio = r.FechaInicio,
                    FechaFin = r.FechaFin,
                    SubtotalReserva = r.SubtotalReserva,
                    ValorIva = r.ValorIva,
                    TotalReserva = r.TotalReserva,
                    Cliente = new ClienteDto
                    {
                        IdCliente = r.Cliente.IdCliente,
                        Nombres = r.Cliente.Nombres,
                        Apellidos = r.Cliente.Apellidos,
                        Correo = r.Cliente.Correo,
                        Telefono = r.Cliente.Telefono
                    },
                    Pasajero = new PasajeroDto
                    {
                        IdPasajero = r.Pasajero.IdPasajero,
                        NombrePasajero = r.Pasajero.NombrePasajero,
                        ApellidoPasajero = r.Pasajero.ApellidoPasajero,
                        TipoDocumentoPasajero = r.Pasajero.TipoDocumentoPasajero,
                        NumeroDocumentoPasajero = r.Pasajero.NumeroDocumentoPasajero
                    },
                    Vuelo = new VueloDto
                    {
                        IdVuelo = r.Vuelo.IdVuelo,
                        NumeroVuelo = r.Vuelo.NumeroVuelo,
                        FechaHoraSalida = r.Vuelo.FechaHoraSalida,
                        FechaHoraLlegada = r.Vuelo.FechaHoraLlegada,
                        AeropuertoOrigen = r.Vuelo.AeropuertoOrigen.Nombre,
                        AeropuertoDestino = r.Vuelo.AeropuertoDestino.Nombre
                    },
                    Asiento = new AsientoDto
                    {
                        IdAsiento = r.Asiento.IdAsiento,
                        NumeroAsiento = r.Asiento.NumeroAsiento,
                        Clase = r.Asiento.Clase
                    },
                    Facturas = r.Facturas
                        .Where(f => !f.EsEliminado)
                        .Select(f => new FacturaDto
                        {
                            IdFactura = f.IdFactura,
                            NumeroFactura = f.NumeroFactura,
                            Total = f.Total,
                            Estado = f.Estado,
                            FechaEmision = f.FechaEmision
                        }).ToList(),
                    Boletos = r.Boletos
                        .Where(b => !b.EsEliminado)
                        .Select(b => new BoletoDto
                        {
                            IdBoleto = b.IdBoleto,
                            CodigoBoleto = b.CodigoBoleto,
                            PrecioFinal = b.PrecioFinal,
                            EstadoBoleto = b.EstadoBoleto
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ReservaCompletaDto?> ObtenerDetalleCompletoPorCodigoAsync(string codigoReserva, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r => r.CodigoReserva == codigoReserva && !r.EsEliminado)
                .Select(r => new ReservaCompletaDto
                {
                    IdReserva = r.IdReserva,
                    GuidReserva = r.GuidReserva,
                    CodigoReserva = r.CodigoReserva,
                    EstadoReserva = r.EstadoReserva,
                    FechaReservaUtc = r.FechaReservaUtc,
                    FechaInicio = r.FechaInicio,
                    FechaFin = r.FechaFin,
                    SubtotalReserva = r.SubtotalReserva,
                    ValorIva = r.ValorIva,
                    TotalReserva = r.TotalReserva,
                    Cliente = new ClienteDto
                    {
                        IdCliente = r.Cliente.IdCliente,
                        Nombres = r.Cliente.Nombres,
                        Apellidos = r.Cliente.Apellidos,
                        Correo = r.Cliente.Correo,
                        Telefono = r.Cliente.Telefono
                    },
                    Pasajero = new PasajeroDto
                    {
                        IdPasajero = r.Pasajero.IdPasajero,
                        NombrePasajero = r.Pasajero.NombrePasajero,
                        ApellidoPasajero = r.Pasajero.ApellidoPasajero,
                        TipoDocumentoPasajero = r.Pasajero.TipoDocumentoPasajero,
                        NumeroDocumentoPasajero = r.Pasajero.NumeroDocumentoPasajero
                    },
                    Vuelo = new VueloDto
                    {
                        IdVuelo = r.Vuelo.IdVuelo,
                        NumeroVuelo = r.Vuelo.NumeroVuelo,
                        FechaHoraSalida = r.Vuelo.FechaHoraSalida,
                        FechaHoraLlegada = r.Vuelo.FechaHoraLlegada,
                        AeropuertoOrigen = r.Vuelo.AeropuertoOrigen.Nombre,
                        AeropuertoDestino = r.Vuelo.AeropuertoDestino.Nombre
                    },
                    Asiento = new AsientoDto
                    {
                        IdAsiento = r.Asiento.IdAsiento,
                        NumeroAsiento = r.Asiento.NumeroAsiento,
                        Clase = r.Asiento.Clase
                    },
                    Facturas = r.Facturas
                        .Where(f => !f.EsEliminado)
                        .Select(f => new FacturaDto
                        {
                            IdFactura = f.IdFactura,
                            NumeroFactura = f.NumeroFactura,
                            Total = f.Total,
                            Estado = f.Estado,
                            FechaEmision = f.FechaEmision
                        }).ToList(),
                    Boletos = r.Boletos
                        .Where(b => !b.EsEliminado)
                        .Select(b => new BoletoDto
                        {
                            IdBoleto = b.IdBoleto,
                            CodigoBoleto = b.CodigoBoleto,
                            PrecioFinal = b.PrecioFinal,
                            EstadoBoleto = b.EstadoBoleto
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public class ReservaResumenDto
        {
            public int IdReserva { get; set; }
            public string CodigoReserva { get; set; } = string.Empty;
            public string EstadoReserva { get; set; } = string.Empty;
            public decimal TotalReserva { get; set; }
            public DateTime FechaReservaUtc { get; set; }
            public string Pasajero { get; set; } = string.Empty;
        }

        public async Task<List<ReservaResumenDto>> ObtenerReservasPorClienteAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r => r.IdCliente == idCliente && !r.EsEliminado)
                .OrderByDescending(r => r.FechaReservaUtc)
                .Select(r => new ReservaResumenDto
                {
                    IdReserva = r.IdReserva,
                    CodigoReserva = r.CodigoReserva,
                    EstadoReserva = r.EstadoReserva,
                    TotalReserva = r.TotalReserva,
                    FechaReservaUtc = r.FechaReservaUtc,
                    Pasajero = r.Pasajero.NombrePasajero + " " + r.Pasajero.ApellidoPasajero
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ReservaResumenDto>> ObtenerHistorialPorClienteAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await ObtenerReservasPorClienteAsync(idCliente, cancellationToken);
        }

        public async Task<List<ReservaResumenDto>> ObtenerPendientesPorVencerAsync(DateTime fechaLimiteUtc, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r => r.EstadoReserva == "PEN" && r.FechaInicio <= fechaLimiteUtc && !r.EsEliminado)
                .OrderBy(r => r.FechaInicio)
                .Select(r => new ReservaResumenDto
                {
                    IdReserva = r.IdReserva,
                    CodigoReserva = r.CodigoReserva,
                    EstadoReserva = r.EstadoReserva,
                    TotalReserva = r.TotalReserva,
                    FechaReservaUtc = r.FechaReservaUtc,
                    Pasajero = r.Pasajero.NombrePasajero + " " + r.Pasajero.ApellidoPasajero
                })
                .ToListAsync(cancellationToken);
        }
    }
}