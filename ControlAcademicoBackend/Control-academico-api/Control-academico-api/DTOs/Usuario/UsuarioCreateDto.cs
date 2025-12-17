namespace Control_academico_api.DTOs;

public class UsuarioCreateDto
{
    public string Nombre { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public string? UserInsert { get; set; }

}
