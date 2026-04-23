using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class ClienteQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public ClienteQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class ClienteCompletoDto
        {
            public int IdCliente { get; set; }
            public string Nombres { get; set; } = string.Empty;
            public string? Apellidos { get; set; }
            public string Correo { get; set; } = string.Empty;
            public List<string> Usernames { get; set; } = new();
            public int TotalReservas { get; set; }
        }

        public async Task<ClienteCompletoDto?> ObtenerClienteConUsuarioYReservasAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(c => c.IdCliente == idCliente && !c.EsEliminado)
                .Select(c => new ClienteCompletoDto
                {
                    IdCliente = c.IdCliente,
                    Nombres = c.Nombres,
                    Apellidos = c.Apellidos,
                    Correo = c.Correo,
                    Usernames = c.UsuariosApp
                        .Where(u => !u.EsEliminado && u.Activo)
                        .Select(u => u.Username)
                        .ToList(),
                    TotalReservas = c.Reservas.Count(r => !r.EsEliminado)
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public class ClienteFrecuenteDto
        {
            public int IdCliente { get; set; }
            public string Cliente { get; set; } = string.Empty;
            public int CantidadReservas { get; set; }
        }

        public async Task<List<ClienteFrecuenteDto>> ObtenerClientesFrecuentesAsync(int top = 10, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(c => !c.EsEliminado)
                .Select(c => new ClienteFrecuenteDto
                {
                    IdCliente = c.IdCliente,
                    Cliente = c.Nombres + " " + (c.Apellidos ?? string.Empty),
                    CantidadReservas = c.Reservas.Count(r => !r.EsEliminado)
                })
                .OrderByDescending(x => x.CantidadReservas)
                .Take(top)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ClienteFrecuenteDto>> ObtenerConReservasConfirmadasAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(c => !c.EsEliminado && c.Reservas.Any(r => r.EstadoReserva == "CON" && !r.EsEliminado))
                .Select(c => new ClienteFrecuenteDto
                {
                    IdCliente = c.IdCliente,
                    Cliente = c.Nombres + " " + (c.Apellidos ?? string.Empty),
                    CantidadReservas = c.Reservas.Count(r => r.EstadoReserva == "CON" && !r.EsEliminado)
                })
                .ToListAsync(cancellationToken);
        }
    }
}