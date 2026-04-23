using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class EquipajeQueryRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public EquipajeQueryRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public class EquipajeDetalleDto
        {
            public int IdEquipaje { get; set; }
            public string NumeroEtiqueta { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
            public decimal PesoKg { get; set; }
            public string EstadoEquipaje { get; set; } = string.Empty;
            public string Pasajero { get; set; } = string.Empty;
            public string NumeroVuelo { get; set; } = string.Empty;
        }

        public async Task<List<EquipajeDetalleDto>> ObtenerPorBoletoConDetalleAsync(int idBoleto, CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e => e.IdBoleto == idBoleto && !e.EsEliminado)
                .Select(e => new EquipajeDetalleDto
                {
                    IdEquipaje = e.IdEquipaje,
                    NumeroEtiqueta = e.NumeroEtiqueta,
                    Tipo = e.Tipo,
                    PesoKg = e.PesoKg,
                    EstadoEquipaje = e.EstadoEquipaje,
                    Pasajero = e.Boleto.Reserva.Pasajero.NombrePasajero + " " + e.Boleto.Reserva.Pasajero.ApellidoPasajero,
                    NumeroVuelo = e.Boleto.Vuelo.NumeroVuelo
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EquipajeDetalleDto>> ObtenerEquipajeDeVueloAsync(int idVuelo, CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e => e.Boleto.IdVuelo == idVuelo && !e.EsEliminado && !e.Boleto.EsEliminado)
                .Select(e => new EquipajeDetalleDto
                {
                    IdEquipaje = e.IdEquipaje,
                    NumeroEtiqueta = e.NumeroEtiqueta,
                    Tipo = e.Tipo,
                    PesoKg = e.PesoKg,
                    EstadoEquipaje = e.EstadoEquipaje,
                    Pasajero = e.Boleto.Reserva.Pasajero.NombrePasajero + " " + e.Boleto.Reserva.Pasajero.ApellidoPasajero,
                    NumeroVuelo = e.Boleto.Vuelo.NumeroVuelo
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EquipajeDetalleDto>> ObtenerExtraviadoAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e => e.EstadoEquipaje == "PERDIDO" && !e.EsEliminado)
                .Select(e => new EquipajeDetalleDto
                {
                    IdEquipaje = e.IdEquipaje,
                    NumeroEtiqueta = e.NumeroEtiqueta,
                    Tipo = e.Tipo,
                    PesoKg = e.PesoKg,
                    EstadoEquipaje = e.EstadoEquipaje,
                    Pasajero = e.Boleto.Reserva.Pasajero.NombrePasajero + " " + e.Boleto.Reserva.Pasajero.ApellidoPasajero,
                    NumeroVuelo = e.Boleto.Vuelo.NumeroVuelo
                })
                .ToListAsync(cancellationToken);
        }
    }
}