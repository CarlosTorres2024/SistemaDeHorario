using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<CentroEducativo> CentroEducativo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales si fueran necesarias
            modelBuilder.Entity<Horario>()
                .HasOne(h => h.Docente)
                .WithMany(d => d.Horarios)
                .HasForeignKey(h => h.IdDocente);

             modelBuilder.Entity<Horario>()
                .HasOne(h => h.Asignatura)
                .WithMany(a => a.Horarios)
                .HasForeignKey(h => h.IdAsignatura);
        }
    }
}
