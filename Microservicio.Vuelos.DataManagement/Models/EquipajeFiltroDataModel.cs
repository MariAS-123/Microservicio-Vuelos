namespace Microservicio.Vuelos.DataManagement.Models;

public class EquipajeFiltroDataModel
{
    /// <summary>Restringe resultados a estos boletos (p. ej. solo boletos de reservas del cliente en token).</summary>
    public List<int>? IdsBoletoPermitidos { get; set; }

    public int? IdBoleto { get; set; }

    public string? NumeroEtiqueta { get; set; }

    public string? EstadoEquipaje { get; set; }

    public string? Estado { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}