namespace Control_academico_api.DTOs;

public class ScheduleCreateDto
{
    public string Dia { get; set; } = null!;
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }

    public int IdUsuario { get; set; }
    public int IdAsignatura { get; set; }

    public string? UserInsert { get; set; }
}
