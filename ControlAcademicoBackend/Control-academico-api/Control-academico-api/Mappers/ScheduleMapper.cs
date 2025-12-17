using Control_academico_api.DTOs;
using Control_academico_api.Models;

namespace Control_academico_api.Mappers;

public static class ScheduleMapper
{
    public static ScheduleDto ToDto(this Schedule entity)
    {
        return new ScheduleDto
        {
            Id = entity.Id,
            Dia = entity.Dia,
            HoraInicio = entity.HoraInicio,
            HoraFin = entity.HoraFin,

            IdUsuario = entity.IdUsuario,
            NombreUsuario = entity.Usuario?.Nombre,

            IdAsignatura = entity.IdAsignatura,
            NombreAsignatura = entity.Asignatura?.Nombre
        };
    }

    public static Schedule ToEntity(this ScheduleCreateDto dto)
    {
        return new Schedule
        {
            Dia = dto.Dia,
            HoraInicio = dto.HoraInicio,
            HoraFin = dto.HoraFin,
            IdUsuario = dto.IdUsuario,
            IdAsignatura = dto.IdAsignatura,
            UserInsert = dto.UserInsert ?? "api"
        };
    }
}
