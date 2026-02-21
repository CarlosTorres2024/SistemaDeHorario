using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones CRUD de Docentes.
    /// </summary>
    public class DocentesController : Controller
    {
        private readonly AppDbContext _context;

        public DocentesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Muestra el listado de docentes.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Docentes.AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Muestra el formulario para crear un nuevo docente.
        /// </summary>
        public IActionResult Create()
        {
            ViewBag.Asignaturas = _context.Asignaturas.AsNoTracking().OrderBy(a => a.Nombre).ToList();
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return PartialView();
            return View();
        }

        /// <summary>
        /// Procesa la creación de un docente.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDocente,Nombre,Especialidad,HorasMaximas")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                // Validación de calidad de datos
                if (docente.Nombre.Length < 3)
                {
                    ModelState.AddModelError("Nombre", "El nombre debe tener al menos 3 caracteres.");
                }
                // Validación de duplicado
                else if (_context.Docentes.Any(d => d.Nombre.ToLower() == docente.Nombre.ToLower()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un docente registrado con este nombre.");
                }
                else
                {
                    _context.Add(docente);
                    await _context.SaveChangesAsync();
                    
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, id = docente.IdDocente, name = docente.Nombre });
                    }
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            
            ViewBag.Asignaturas = _context.Asignaturas.AsNoTracking().OrderBy(a => a.Nombre).ToList();
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(docente);
            }
            
            return View(docente);
        }

        /// <summary>
        /// Muestra el formulario para editar un docente existente.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null) return NotFound();
            
            ViewBag.Asignaturas = _context.Asignaturas.AsNoTracking().OrderBy(a => a.Nombre).ToList();
            return View(docente);
        }

        /// <summary>
        /// Procesa la edición de un docente.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDocente,Nombre,Especialidad,HorasMaximas")] Docente docente)
        {
            if (id != docente.IdDocente) return NotFound();

            if (ModelState.IsValid)
            {
                // Validación de calidad de datos
                if (docente.Nombre.Length < 3)
                {
                    ModelState.AddModelError("Nombre", "El nombre debe tener al menos 3 caracteres.");
                }
                // Validación de duplicado
                else if (_context.Docentes.Any(d => d.Nombre.ToLower() == docente.Nombre.ToLower() && d.IdDocente != docente.IdDocente))
                {
                    ModelState.AddModelError("Nombre", "Ya existe otro docente con este nombre.");
                }
                else
                {
                    try
                    {
                        _context.Update(docente);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DocenteExists(docente.IdDocente)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(docente);
        }

        /// <summary>
        /// Muestra la confirmación para eliminar un docente.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var docente = await _context.Docentes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdDocente == id);
            if (docente == null) return NotFound();

            return View(docente);
        }

        /// <summary>
        /// Procesa la eliminación de un docente.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DocenteExists(int id)
        {
            return _context.Docentes.Any(e => e.IdDocente == id);
        }
    }
}
