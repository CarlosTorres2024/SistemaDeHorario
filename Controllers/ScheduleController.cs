using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/schedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleItem>>> GetSchedule()
        {
            var horarios = await _context.Horarios
                .Include(h => h.Asignatura)
                .Include(h => h.Docente)
                .Include(h => h.Aula)
                .Include(h => h.Grupo)
                .ToListAsync();

            var items = horarios.Select(h => new ScheduleItem
            {
                Id = h.IdHorario,
                SubjectName = h.Asignatura?.Nombre ?? "N/A",
                TeacherName = h.Docente?.Nombre ?? "N/A",
                DayOfWeek = h.DiaSemana,
                StartTime = h.HoraInicio.ToString(@"hh\:mm"),
                EndTime = h.HoraFin.ToString(@"hh\:mm"),
                RoomNumber = h.Aula?.NombreNumero ?? "N/A",
                GradeLevel = h.Grupo?.Nombre ?? "N/A"
            });

            return Ok(items);
        }

        // POST: api/schedule
        [HttpPost]
        public async Task<ActionResult<ScheduleItem>> CreateSchedule(ScheduleItem item)
        {
            // Nota: El modelo del profesor es plano (strings). Mapeamos a nuestras entidades relacionadas.
            var docente = await _context.Docentes.FirstOrDefaultAsync(d => d.Nombre == item.TeacherName);
            var asignatura = await _context.Asignaturas.FirstOrDefaultAsync(a => a.Nombre == item.SubjectName);
            var aula = await _context.Aulas.FirstOrDefaultAsync(a => a.NombreNumero == item.RoomNumber);
            var grupo = await _context.Grupos.FirstOrDefaultAsync(g => g.Nombre == item.GradeLevel);

            if (docente == null || asignatura == null)
            {
                return BadRequest("No se encontró el Docente o la Asignatura especificada.");
            }

            var horario = new Horario
            {
                IdDocente = docente.IdDocente,
                IdAsignatura = asignatura.IdAsignatura,
                IdAula = aula?.IdAula,
                IdGrupo = grupo?.IdGrupo,
                DiaSemana = item.DayOfWeek,
                HoraInicio = TimeSpan.Parse(item.StartTime),
                HoraFin = TimeSpan.Parse(item.EndTime)
            };

            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();

            item.Id = horario.IdHorario;
            return CreatedAtAction(nameof(GetSchedule), new { id = item.Id }, item);
        }
    }
}
