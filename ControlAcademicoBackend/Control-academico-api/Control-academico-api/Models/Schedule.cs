using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_academico_api.Models;

[Table("schedules")]
public class Schedule
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("dia")]
    public string Dia { get; set; } = null!; // Ej: "LUNES", "MARTES"

    [Required]
    [Column("hora_inicio")]
    public TimeSpan HoraInicio { get; set; }   // Mapea a TIME en MySQL

    [Required]
    [Column("hora_fin")]
    public TimeSpan HoraFin { get; set; }      // Mapea a TIME en MySQL

    [Required]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Required]
    [Column("id_asignatura")]
    public int IdAsignatura { get; set; }

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

    // 🔗 Relaciones de navegación
    [ForeignKey(nameof(IdUsuario))]
    public Usuario? Usuario { get; set; }

    [ForeignKey(nameof(IdAsignatura))]
    public Asignatura? Asignatura { get; set; }
}
