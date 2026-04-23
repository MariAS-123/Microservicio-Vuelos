namespace Microservicio.Vuelos.Business.DTOs.Equipaje;

public class EquipajeFilterDto
{
    public int? IdBoleto { get; set; }

    public string? NumeroEtiqueta { get; set; }

    public string? EstadoEquipaje { get; set; }

    public string? Estado { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}