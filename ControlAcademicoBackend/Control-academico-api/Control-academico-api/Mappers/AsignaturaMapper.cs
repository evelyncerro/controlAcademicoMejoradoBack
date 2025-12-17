using Control_academico_api.DTOs;
using Control_academico_api.Models;

namespace Control_academico_api.Mappers;

public static class AsignaturaMapper
{
    public static AsignaturaDto ToDto(this Asignatura entity)
    {
        return new AsignaturaDto
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Codigo = entity.Codigo,
            MaxClasesSemana = entity.MaxClasesSemana,
            UserUpdate = entity.UserUpdate ?? "api"
        };
    }

    public static Asignatura ToEntity(this AsignaturaCreateDto dto)
    {
        return new Asignatura
        {
            Nombre = dto.Nombre,
            Codigo = dto.Codigo,
            MaxClasesSemana = dto.MaxClasesSemana,
            UserInsert = dto.UserInsert ?? "api"
        };
    }
}
