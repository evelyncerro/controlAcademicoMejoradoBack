using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_academico_api.Models;

[Table("usuario")]
public class Usuario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Required]
    [Column("correo")]
    public string Correo { get; set; } = null!;

    [Required]
    [Column("password")]
    public string Password { get; set; } = null!;

    [Required]
    [Column("rol")]
    public string Rol { get; set; } = null!;

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
