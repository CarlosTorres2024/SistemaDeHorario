using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    public class GruposController : Controller
    {
        private readonly AppDbContext _context;

        public GruposController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Grupos.ToListAsync());
        }

        public IActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return PartialView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGrupo,Nombre,NivelEducativo")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                // Validación de calidad de datos
                if (grupo.Nombre.Length < 1)
                {
                    ModelState.AddModelError("Nombre", "El nombre del grupo no puede estar vacío.");
                }
                else if (grupo.NivelEducativo?.Length < 3)
                {
                    ModelState.AddModelError("NivelEducativo", "El nivel educativo debe tener al menos 3 caracteres.");
                }
                // Validación de duplicado
                else if (_context.Grupos.Any(g => g.Nombre.ToLower() == grupo.Nombre.ToLower()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un grupo registrado con este nombre.");
                }
                else
                {
                    _context.Add(grupo);
                    await _context.SaveChangesAsync();
                    
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, id = grupo.IdGrupo, name = grupo.Nombre });
                    }
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(grupo);
            }
            
            return View(grupo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();
            
            return View(grupo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGrupo,Nombre,NivelEducativo")] Grupo grupo)
        {
            if (id != grupo.IdGrupo) return NotFound();

            if (ModelState.IsValid)
            {
                // Validación de calidad de datos
                if (grupo.Nombre.Length < 1)
                {
                    ModelState.AddModelError("Nombre", "El nombre del grupo no puede estar vacío.");
                }
                else if (grupo.NivelEducativo?.Length < 3)
                {
                    ModelState.AddModelError("NivelEducativo", "El nivel educativo debe tener al menos 3 caracteres.");
                }
                // Validación de duplicado
                else if (_context.Grupos.Any(g => g.Nombre.ToLower() == grupo.Nombre.ToLower() && g.IdGrupo != grupo.IdGrupo))
                {
                    ModelState.AddModelError("Nombre", "Ya existe otro grupo con este nombre.");
                }
                else
                {
                    try
                    {
                        _context.Update(grupo);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!GrupoExists(grupo.IdGrupo)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(grupo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos
                .FirstOrDefaultAsync(m => m.IdGrupo == id);
            if (grupo == null) return NotFound();

            return View(grupo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo != null)
            {
                _context.Grupos.Remove(grupo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GrupoExists(int id)
        {
            return _context.Grupos.Any(e => e.IdGrupo == id);
        }
    }
}
