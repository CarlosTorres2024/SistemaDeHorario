using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    public class HorariosController : Controller
    {
        private readonly AppDbContext _context;

        public HorariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Horarios
        public async Task<IActionResult> Index(int? idDocente, int? idAula, int? idGrupo)
        {
            var query = _context.Horarios
                .Include(h => h.Asignatura)
                .Include(h => h.Aula)
                .Include(h => h.Docente)
                .Include(h => h.Grupo)
                .AsNoTracking();

            // Aplicar filtros si se proporcionan
            if (idDocente.HasValue)
            {
                query = query.Where(h => h.IdDocente == idDocente.Value);
            }
            if (idAula.HasValue)
            {
                query = query.Where(h => h.IdAula == idAula.Value);
            }
            if (idGrupo.HasValue)
            {
                query = query.Where(h => h.IdGrupo == idGrupo.Value);
            }

            // Listas para los combos de filtros
            ViewData["IdDocente"] = new SelectList(await _context.Docentes.AsNoTracking().ToListAsync(), "IdDocente", "Nombre", idDocente);
            ViewData["IdAula"] = new SelectList(await _context.Aulas.AsNoTracking().ToListAsync(), "IdAula", "NombreNumero", idAula);
            ViewData["IdGrupo"] = new SelectList(await _context.Grupos.AsNoTracking().ToListAsync(), "IdGrupo", "Nombre", idGrupo);

            return View(await query.OrderBy(h => h.DiaSemana).ThenBy(h => h.HoraInicio).ToListAsync());
        }

        // GET: Horarios/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdAsignatura"] = new SelectList(await _context.Asignaturas.AsNoTracking().ToListAsync(), "IdAsignatura", "Nombre");
            ViewData["IdAula"] = new SelectList(await _context.Aulas.AsNoTracking().ToListAsync(), "IdAula", "NombreNumero");
            ViewData["IdDocente"] = new SelectList(await _context.Docentes.AsNoTracking().ToListAsync(), "IdDocente", "Nombre");
            ViewData["IdGrupo"] = new SelectList(await _context.Grupos.AsNoTracking().ToListAsync(), "IdGrupo", "Nombre");
            return View();
        }

        // POST: Horarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHorario,IdDocente,IdAsignatura,IdAula,IdGrupo,DiaSemana,HoraInicio,HoraFin")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                // 0. Validar Coherencia de Tiempo
                if (horario.HoraFin <= horario.HoraInicio)
                {
                    ModelState.AddModelError(string.Empty, "La hora de finalización debe ser posterior a la hora de inicio.");
                }
                else if ((horario.HoraFin - horario.HoraInicio).TotalMinutes < 20)
                {
                    ModelState.AddModelError(string.Empty, "La duración mínima de una sesión debe ser de 20 minutos.");
                }
                // 1. Validar Choque de Horario (Docente, Aula, Grupo)
                else 
                {
                    var conflict = await GetConflictMessageAsync(horario);
                    if (conflict != null)
                    {
                        ModelState.AddModelError(string.Empty, conflict);
                    }
                    // 2. Validar Horas Máximas del Docente
                    else if (await ExceedsMaxHoursAsync(horario))
                    {
                        var docente = await _context.Docentes.FindAsync(horario.IdDocente);
                        ModelState.AddModelError(string.Empty, $"Lo sentimos, el docente ya ha alcanzado o superaría su límite de horas semanales ({docente?.HorasMaximas} horas).");
                    }
                    else
                    {
                        _context.Add(horario);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            ViewData["IdAsignatura"] = new SelectList(await _context.Asignaturas.AsNoTracking().ToListAsync(), "IdAsignatura", "Nombre", horario.IdAsignatura);
            ViewData["IdAula"] = new SelectList(await _context.Aulas.AsNoTracking().ToListAsync(), "IdAula", "NombreNumero", horario.IdAula);
            ViewData["IdDocente"] = new SelectList(await _context.Docentes.AsNoTracking().ToListAsync(), "IdDocente", "Nombre", horario.IdDocente);
            ViewData["IdGrupo"] = new SelectList(await _context.Grupos.AsNoTracking().ToListAsync(), "IdGrupo", "Nombre", horario.IdGrupo);
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null) return NotFound();
            ViewData["IdAsignatura"] = new SelectList(await _context.Asignaturas.AsNoTracking().ToListAsync(), "IdAsignatura", "Nombre", horario.IdAsignatura);
            ViewData["IdAula"] = new SelectList(await _context.Aulas.AsNoTracking().ToListAsync(), "IdAula", "NombreNumero", horario.IdAula);
            ViewData["IdDocente"] = new SelectList(await _context.Docentes.AsNoTracking().ToListAsync(), "IdDocente", "Nombre", horario.IdDocente);
            ViewData["IdGrupo"] = new SelectList(await _context.Grupos.AsNoTracking().ToListAsync(), "IdGrupo", "Nombre", horario.IdGrupo);
            return View(horario);
        }

        // POST: Horarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHorario,IdDocente,IdAsignatura,IdAula,IdGrupo,DiaSemana,HoraInicio,HoraFin")] Horario horario)
        {
            if (id != horario.IdHorario) return NotFound();

            if (ModelState.IsValid)
            {
                // 0. Validar Coherencia de Tiempo
                if (horario.HoraFin <= horario.HoraInicio)
                {
                    ModelState.AddModelError(string.Empty, "La hora de finalización debe ser posterior a la hora de inicio.");
                }
                else if ((horario.HoraFin - horario.HoraInicio).TotalMinutes < 20)
                {
                    ModelState.AddModelError(string.Empty, "La duración mínima de una sesión debe ser de 20 minutos.");
                }
                // 1. Validar Choque de Horario
                else 
                {
                    var conflict = await GetConflictMessageAsync(horario);
                    if (conflict != null)
                    {
                        ModelState.AddModelError(string.Empty, conflict);
                    }
                    // 2. Validar Horas Máximas
                    else if (await ExceedsMaxHoursAsync(horario))
                    {
                        var docente = await _context.Docentes.FindAsync(horario.IdDocente);
                        ModelState.AddModelError(string.Empty, $"Atención: El docente superaría su límite establecido de {docente?.HorasMaximas} horas semanales.");
                    }
                    else
                    {
                        try
                        {
                            _context.Update(horario);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!await HorarioExistsAsync(horario.IdHorario)) return NotFound();
                            else throw;
                        }
                    }
                }
            }
            ViewData["IdAsignatura"] = new SelectList(await _context.Asignaturas.AsNoTracking().ToListAsync(), "IdAsignatura", "Nombre", horario.IdAsignatura);
            ViewData["IdAula"] = new SelectList(await _context.Aulas.AsNoTracking().ToListAsync(), "IdAula", "NombreNumero", horario.IdAula);
            ViewData["IdDocente"] = new SelectList(await _context.Docentes.AsNoTracking().ToListAsync(), "IdDocente", "Nombre", horario.IdDocente);
            ViewData["IdGrupo"] = new SelectList(await _context.Grupos.AsNoTracking().ToListAsync(), "IdGrupo", "Nombre", horario.IdGrupo);
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var horario = await _context.Horarios
                .AsNoTracking()
                .Include(h => h.Asignatura)
                .Include(h => h.Aula)
                .Include(h => h.Docente)
                .Include(h => h.Grupo)
                .FirstOrDefaultAsync(m => m.IdHorario == id);
            if (horario == null) return NotFound();

            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario != null)
            {
                _context.Horarios.Remove(horario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> HorarioExistsAsync(int id)
        {
            return await _context.Horarios.AnyAsync(e => e.IdHorario == id);
        }

        /// <summary>
        /// Valida si existe conflicto de horario para Docente, Aula o Grupo de forma eficiente.
        /// </summary>
        private async Task<string?> GetConflictMessageAsync(Horario newHorario)
        {
            var conflict = await _context.Horarios
                .AsNoTracking()
                .Include(h => h.Asignatura)
                .Include(h => h.Aula)
                .Include(h => h.Docente)
                .Include(h => h.Grupo)
                .Where(h => h.DiaSemana == newHorario.DiaSemana &&
                            h.IdHorario != newHorario.IdHorario &&
                            h.HoraInicio < newHorario.HoraFin &&
                            h.HoraFin > newHorario.HoraInicio)
                .Where(h => h.IdDocente == newHorario.IdDocente || 
                            (newHorario.IdAula != null && h.IdAula == newHorario.IdAula) || 
                            (newHorario.IdGrupo != null && h.IdGrupo == newHorario.IdGrupo))
                .FirstOrDefaultAsync();

            if (conflict == null) return null;

            if (conflict.IdDocente == newHorario.IdDocente)
            {
                return $"Conflicto con el Docente: {conflict.Docente?.Nombre} ya se encuentra impartiendo '{conflict.Asignatura?.Nombre}' al grupo {conflict.Grupo?.Nombre} el día {conflict.DiaSemana} de {conflict.HoraInicio:hh\\:mm} a {conflict.HoraFin:hh\\:mm}.";
            }
            
            if (newHorario.IdAula != null && conflict.IdAula == newHorario.IdAula)
            {
                return $"Conflicto con el Aula: El aula {conflict.Aula?.NombreNumero} ya está reservada para '{conflict.Asignatura?.Nombre}' (Grupo {conflict.Grupo?.Nombre}) de {conflict.HoraInicio:hh\\:mm} a {conflict.HoraFin:hh\\:mm}.";
            }
 
            if (newHorario.IdGrupo != null && conflict.IdGrupo == newHorario.IdGrupo)
            {
                return $"Conflicto con el Grupo: El grupo {conflict.Grupo?.Nombre} ya tiene programada la materia '{conflict.Asignatura?.Nombre}' en este horario ({conflict.HoraInicio:hh\\:mm} - {conflict.HoraFin:hh\\:mm}).";
            }
 
            return "Lo sentimos, existe un conflicto de horario con otra asignación ya registrada en el sistema.";
        }

        /// <summary>
        /// Valida si la carga horaria del docente supera el máximo permitido de forma asíncrona.
        /// </summary>
        private async Task<bool> ExceedsMaxHoursAsync(Horario newHorario)
        {
            var docente = await _context.Docentes
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDocente == newHorario.IdDocente);
            
            if (docente == null) return false;

            var currentHours = await _context.Horarios
                .AsNoTracking()
                .Where(h => h.IdDocente == newHorario.IdDocente && h.IdHorario != newHorario.IdHorario)
                .SumAsync(h => EF.Functions.DateDiffMinute(h.HoraInicio, h.HoraFin) / 60.0);
            
            var newDuration = (newHorario.HoraFin - newHorario.HoraInicio).TotalHours;

            return (currentHours + newDuration) > docente.HorasMaximas;
        }
    }
}
