namespace Control_academico_api.DTOs;

public class AsignaturaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Codigo { get; set; } = null!;
    public int MaxClasesSemana { get; set; }
    public string UserUpdate { get; set; } = null!;
}
