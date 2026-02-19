using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    public class CentrosController : Controller
    {
        private readonly AppDbContext _context;

        public CentrosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CentrosEducativos.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCentro,Nombre,Dirección,Teléfono")] CentroEducativo centro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(centro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(centro);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var centro = await _context.CentrosEducativos.FindAsync(id);
            if (centro == null) return NotFound();
            return View(centro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCentro,Nombre,Dirección,Teléfono")] CentroEducativo centro)
        {
            if (id != centro.IdCentro) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroExists(centro.IdCentro)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(centro);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var centro = await _context.CentrosEducativos
                .FirstOrDefaultAsync(m => m.IdCentro == id);
            if (centro == null) return NotFound();

            return View(centro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var centro = await _context.CentrosEducativos.FindAsync(id);
            if (centro != null)
            {
                _context.CentrosEducativos.Remove(centro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CentroExists(int id)
        {
            return _context.CentrosEducativos.Any(e => e.IdCentro == id);
        }
    }
}
