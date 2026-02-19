using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones CRUD de Asignaturas.
    /// </summary>
    public class AsignaturasController : Controller
    {
        private readonly AppDbContext _context;

        public AsignaturasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Asignaturas.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsignatura,Nombre,HorasSemanales")] Asignatura asignatura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asignatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asignatura);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var asignatura = await _context.Asignaturas.FindAsync(id);
            if (asignatura == null) return NotFound();
            return View(asignatura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsignatura,Nombre,HorasSemanales")] Asignatura asignatura)
        {
            if (id != asignatura.IdAsignatura) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asignatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsignaturaExists(asignatura.IdAsignatura)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(asignatura);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var asignatura = await _context.Asignaturas
                .FirstOrDefaultAsync(m => m.IdAsignatura == id);
            if (asignatura == null) return NotFound();

            return View(asignatura);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asignatura = await _context.Asignaturas.FindAsync(id);
            if (asignatura != null)
            {
                _context.Asignaturas.Remove(asignatura);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AsignaturaExists(int id)
        {
            return _context.Asignaturas.Any(e => e.IdAsignatura == id);
        }
    }
}
