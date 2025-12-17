using Control_academico_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Control_academico_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Asignatura> Asignaturas => Set<Asignatura>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
}
