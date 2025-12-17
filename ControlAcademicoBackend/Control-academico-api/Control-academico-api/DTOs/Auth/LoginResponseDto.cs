namespace Control_academico_api.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }

        public UsuarioDto Usuario { get; set; } = default!;
    }
}
