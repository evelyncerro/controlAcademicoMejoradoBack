namespace Control_academico_api.DTOs;

public class ScheduleDto
{
    public int Id { get; set; }
    public string Dia { get; set; } = null!;
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }

    public int IdUsuario { get; set; }
    public string? NombreUsuario { get; set; }

    public int IdAsignatura { get; set; }
    public string? NombreAsignatura { get; set; }

    public string? UserUpdate { get; set; }
}
