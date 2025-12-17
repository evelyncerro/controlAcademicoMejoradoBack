namespace Control_academico_api.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
