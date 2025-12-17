using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_academico_api.Models;

[Table("asignatura")]
public class Asignatura
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Required]
    [Column("codigo")]
    public string Codigo { get; set; } = null!;

    [Required]
    [Column("max_clases_semana")]
    public int MaxClasesSemana { get; set; }

    // 🔎 Auditoría
    [Required]
    [Column("user_insert")]
    public string UserInsert { get; set; } = null!;

    [Column("user_update")]
    public string? UserUpdate { get; set; }

    [Column("fecha_sistema")]
    public DateTime FechaSistema { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("eliminado")]
    public bool Eliminado { get; set; }

    // 🔗 Relaciones
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
