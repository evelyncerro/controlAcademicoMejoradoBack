namespace Control_academico_api.DTOs;

public class AsignaturaCreateDto
{
    public string Nombre { get; set; } = null!;
    public string Codigo { get; set; } = null!;
    public int MaxClasesSemana { get; set; }
    public string? UserInsert { get; set; }
}
