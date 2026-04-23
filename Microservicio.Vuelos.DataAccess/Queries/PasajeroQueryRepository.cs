using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class PasajeroQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public PasajeroQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class PasajeroReservaDto
        {
            public int IdPasajero { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;
            public string Documento { get; set; } = string.Empty;
            public string CodigoReserva { get; set; } = string.Empty;
            public List<string> CodigosBoleto { get; set; } = new();
        }

        public async Task<List<PasajeroReservaDto>> ObtenerPasajerosDeReservaAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r => r.IdReserva == idReserva && !r.EsEliminado)
                .Select(r => new PasajeroReservaDto
                {
                    IdPasajero = r.Pasajero.IdPasajero,
                    NombreCompleto = r.Pasajero.NombrePasajero + " " + r.Pasajero.ApellidoPasajero,
                    Documento = r.Pasajero.TipoDocumentoPasajero + " " + r.Pasajero.NumeroDocumentoPasajero,
                    CodigoReserva = r.CodigoReserva,
                    CodigosBoleto = r.Boletos
                        .Where(b => !b.EsEliminado)
                        .Select(b => b.CodigoBoleto)
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PasajeroReservaDto>> ObtenerPasajerosDeVueloAsync(int idVuelo, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r => r.IdVuelo == idVuelo && !r.EsEliminado)
                .Select(r => new PasajeroReservaDto
                {
                    IdPasajero = r.Pasajero.IdPasajero,
                    NombreCompleto = r.Pasajero.NombrePasajero + " " + r.Pasajero.ApellidoPasajero,
                    Documento = r.Pasajero.TipoDocumentoPasajero + " " + r.Pasajero.NumeroDocumentoPasajero,
                    CodigoReserva = r.CodigoReserva,
                    CodigosBoleto = r.Boletos
                        .Where(b => !b.EsEliminado)
                        .Select(b => b.CodigoBoleto)
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public class PasajeroEspecialDto
        {
            public int IdPasajero { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;
            public string? Observaciones { get; set; }
        }

        public async Task<List<PasajeroEspecialDto>> ObtenerConAsistenciaEspecialAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Pasajeros
                .AsNoTracking()
                .Where(p => p.RequiereAsistencia && !p.EsEliminado)
                .Select(p => new PasajeroEspecialDto
                {
                    IdPasajero = p.IdPasajero,
                    NombreCompleto = p.NombrePasajero + " " + p.ApellidoPasajero,
                    Observaciones = p.ObservacionesPasajero
                })
                .ToListAsync(cancellationToken);
        }
    }
}