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
public class AsignaturasController : ControllerBase
{
    private readonly AppDbContext _context;

    public AsignaturasController(AppDbContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Asignatura>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Asignatura>>> GetAsignaturas()
    {
        try
        {
            var asignaturas = await _context.Asignaturas
                .Where(a => !a.Eliminado)
                .ToListAsync();

            return Ok(asignaturas);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al obtener las asignaturas.",
                    detalle = ex.Message
                }
            );
        }
    }

    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Asignatura), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Asignatura>> GetAsignatura(int id)
    {
        try
        {
            var asignatura = await _context.Asignaturas.FindAsync(id);

            if (asignatura == null || asignatura.Eliminado)
                return NotFound(new { mensaje = "Asignatura no encontrada." });

            return Ok(asignatura);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al consultar la asignatura.",
                    detalle = ex.Message
                }
            );
        }
    }

    
    [HttpPost]
    [ProducesResponseType(typeof(AsignaturaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AsignaturaDto>> PostAsignatura([FromBody] AsignaturaCreateDto dto)
    {
        if (dto == null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var asignatura = dto.ToEntity();

            asignatura.FechaSistema = DateTime.UtcNow;
            asignatura.FechaActualizacion = null;
            asignatura.Eliminado = false;

            _context.Asignaturas.Add(asignatura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAsignatura),
                new { id = asignatura.Id },
                asignatura.ToDto()
            );
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al guardar la asignatura en la base de datos.",
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
                    mensaje = "Error inesperado al crear la asignatura.",
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
    public async Task<IActionResult> PutAsignatura([FromBody] AsignaturaDto asignaturaDto)
    {
        if (asignaturaDto == null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        try
        {
            var existing = await _context.Asignaturas.FindAsync(asignaturaDto.Id);

            if (existing == null || existing.Eliminado)
                return NotFound(new { mensaje = "Asignatura no encontrada." });

            // Actualizar campos editables
            existing.Nombre = asignaturaDto.Nombre;
            existing.Codigo = asignaturaDto.Codigo;
            existing.MaxClasesSemana = asignaturaDto.MaxClasesSemana;

            existing.UserUpdate = string.IsNullOrWhiteSpace(asignaturaDto.UserUpdate)
                ? "api"
                : asignaturaDto.UserUpdate;

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
                    mensaje = "Error al actualizar la asignatura en la base de datos.",
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
                    mensaje = "Error inesperado al actualizar la asignatura.",
                    detalle = ex.Message
                }
            );
        }
    }

    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsignatura(int id)
    {
        try
        {
            var asignatura = await _context.Asignaturas.FindAsync(id);

            if (asignatura == null || asignatura.Eliminado)
                return NotFound(new { mensaje = "Asignatura no encontrada." });

            asignatura.Eliminado = true;
            asignatura.UserUpdate = "api";
            asignatura.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al eliminar la asignatura en la base de datos.",
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
                    mensaje = "Error inesperado al eliminar la asignatura.",
                    detalle = ex.Message
                }
            );
        }
    }
}
