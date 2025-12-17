namespace Control_academico_api.DTOs;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public string? UserUpdate { get; set; }
}
