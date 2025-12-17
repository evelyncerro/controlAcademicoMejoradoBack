using Control_academico_api.Data;
using Control_academico_api.DTOs.Auth;
using Control_academico_api.Mappers;
using Control_academico_api.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Control_academico_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginRequestDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            if (dto == null)
                return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                
                var usuario = await _context.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u =>
                        u.Correo == dto.Correo && !u.Eliminado
                    );

                if (usuario == null)
                    return Unauthorized(new { mensaje = "Credenciales incorrectas." });

                // 2. Validar contraseña
                if (usuario.Password != dto.Password)
                    return Unauthorized(new { mensaje = "Credenciales incorrectas." });

                // 3. Generar token JWT
                var token = GeneraJwtToken(usuario, out DateTime expiration);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Expiration = expiration,
                    Usuario = usuario.ToDto()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "Error al procesar la solicitud de inicio de sesión.",
                        detalle = ex.Message
                    }
                );
            }
        }

        private string GeneraJwtToken(Usuario usuario, out DateTime expiration)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");
            expiration = DateTime.UtcNow.AddMinutes(expireMinutes);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Correo),

            //rol
            // new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
