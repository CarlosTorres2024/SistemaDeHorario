using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    public class AulasController : Controller
    {
        private readonly AppDbContext _context;

        public AulasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Aulas.ToListAsync());
        }

        public IActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return PartialView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAula,NombreNumero,Capacidad")] Aula aula)
        {
            if (ModelState.IsValid)
            {
                // Validación de calidad de datos
                if (aula.NombreNumero.Length < 3)
                {
                    ModelState.AddModelError("NombreNumero", "El nombre del aula debe tener al menos 3 caracteres.");
                }
                // Validación de duplicado
                else if (_context.Aulas.Any(a => a.NombreNumero.ToLower() == aula.NombreNumero.ToLower()))
                {
                    ModelState.AddModelError("NombreNumero", "Ya existe un aula registrada con este nombre.");
                }
                else
                {
                    _context.Add(aula);
                    await _context.SaveChangesAsync();
                    
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, id = aula.IdAula, name = aula.NombreNumero });
                    }
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(aula);
            }
            
            return View(aula);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null) return NotFound();
            return View(aula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAula,NombreNumero,Capacidad")] Aula aula)
        {
            if (id != aula.IdAula) return NotFound();

            if (ModelState.IsValid)
            {
                // Validación de calidad de datos
                if (aula.NombreNumero.Length < 3)
                {
                    ModelState.AddModelError("NombreNumero", "El nombre del aula debe tener al menos 3 caracteres.");
                }
                // Validación de duplicado
                else if (_context.Aulas.Any(a => a.NombreNumero.ToLower() == aula.NombreNumero.ToLower() && a.IdAula != aula.IdAula))
                {
                    ModelState.AddModelError("NombreNumero", "Ya existe otra aula con este nombre.");
                }
                else
                {
                    try
                    {
                        _context.Update(aula);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AulaExists(aula.IdAula)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(aula);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var aula = await _context.Aulas
                .FirstOrDefaultAsync(m => m.IdAula == id);
            if (aula == null) return NotFound();

            return View(aula);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            if (aula != null)
            {
                _context.Aulas.Remove(aula);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.IdAula == id);
        }
    }
}
