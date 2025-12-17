using Control_academico_api.Data;
using Control_academico_api.DTOs;
using Control_academico_api.Mappers;
using Control_academico_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Control_academico_api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Usuario>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        try
        {
            var usuarios = await _context.Usuarios
                .Where(u => !u.Eliminado)
                .ToListAsync();

            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al obtener los usuarios.",
                    detalle = ex.Message
                }
            );
        }
    }


    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null || usuario.Eliminado)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al consultar el usuario.",
                    detalle = ex.Message
                }
            );
        }
    }


    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] UsuarioCreateDto dto)
    {
        if (dto == null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var usuario = dto.ToEntity();

            usuario.FechaSistema = DateTime.UtcNow;
            usuario.FechaActualizacion = null;
            usuario.Eliminado = false;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUsuario),
                new { id = usuario.Id },
                usuario.ToDto()
            );
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al guardar el usuario en la base de datos.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error inesperado al crear el usuario.",
                    detalle = ex.Message
                }
            );
        }
    }


    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutUsuario([FromBody] UsuarioDto usuario)
    {
        if (usuario == null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        try
        {
            var existing = await _context.Usuarios.FindAsync(usuario.Id);

            if (existing == null || existing.Eliminado)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            
            existing.Nombre = usuario.Nombre;
            existing.Correo = usuario.Correo;
            existing.Rol = usuario.Rol;
                        
            existing.UserUpdate = string.IsNullOrWhiteSpace(usuario.UserUpdate)
                ? "api"
                : usuario.UserUpdate;

            existing.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al actualizar el usuario en la base de datos.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error inesperado al actualizar el usuario.",
                    detalle = ex.Message
                }
            );
        }
    }


    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null || usuario.Eliminado)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            usuario.Eliminado = true;
            usuario.UserUpdate = "api";
            usuario.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al eliminar el usuario en la base de datos.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error inesperado al eliminar el usuario.",
                    detalle = ex.Message
                }
            );
        }
    }



    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<Usuario>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosAll()
    {
        try
        {
            // Aquí NO filtramos por Eliminado
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .ToListAsync();

            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al obtener todos los usuarios (incluidos eliminados).",
                    detalle = ex.Message
                }
            );
        }
    }


}
