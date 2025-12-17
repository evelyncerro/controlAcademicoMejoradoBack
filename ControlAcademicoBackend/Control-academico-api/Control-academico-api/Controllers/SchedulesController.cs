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
public class SchedulesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SchedulesController(AppDbContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Schedule>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
    {
        try
        {
            var schedules = await _context.Schedules
                .Where(s => !s.Eliminado)
                .ToListAsync();

            return Ok(schedules);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al obtener los horarios.",
                    detalle = ex.Message
                }
            );
        }
    }

    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Schedule), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Schedule>> GetSchedule(int id)
    {
        try
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null || schedule.Eliminado)
                return NotFound(new { mensaje = "Horario no encontrado." });

            return Ok(schedule);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al consultar el horario.",
                    detalle = ex.Message
                }
            );
        }
    }

    
    [HttpPost]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ScheduleDto>> PostSchedule([FromBody] ScheduleCreateDto dto)
    {
        if (dto == null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var schedule = dto.ToEntity();


            schedule.FechaSistema = DateTime.UtcNow;
            schedule.FechaActualizacion = null;
            schedule.Eliminado = false;
            schedule.UserInsert = string.IsNullOrWhiteSpace(schedule.UserInsert)
                ? "api"
                : schedule.UserInsert;

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSchedule),
                new { id = schedule.Id },
                schedule.ToDto()
            );
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al guardar el horario en la base de datos.",
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
                    mensaje = "Error inesperado al crear el horario.",
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
    public async Task<IActionResult> PutSchedule([FromBody] ScheduleDto scheduleDto)
    {
        if (scheduleDto == null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        try
        {
            var existing = await _context.Schedules.FindAsync(scheduleDto.Id);

            if (existing == null || existing.Eliminado)
                return NotFound(new { mensaje = "Horario no encontrado." });

            // Actualizar campos editables
            existing.Dia = scheduleDto.Dia;
            existing.HoraInicio = scheduleDto.HoraInicio;
            existing.HoraFin = scheduleDto.HoraFin;
            existing.IdUsuario = scheduleDto.IdUsuario;
            existing.IdAsignatura = scheduleDto.IdAsignatura;

            existing.UserUpdate = string.IsNullOrWhiteSpace(scheduleDto.UserUpdate)
                ? "api"
                : scheduleDto.UserUpdate;

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
                    mensaje = "Error al actualizar el horario en la base de datos.",
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
                    mensaje = "Error inesperado al actualizar el horario.",
                    detalle = ex.Message
                }
            );
        }
    }

    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        try
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null || schedule.Eliminado)
                return NotFound(new { mensaje = "Horario no encontrado." });

            schedule.Eliminado = true;
            schedule.UserUpdate = "api";
            schedule.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error al eliminar el horario en la base de datos.",
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
                    mensaje = "Error inesperado al eliminar el horario.",
                    detalle = ex.Message
                }
            );
        }
    }
}
