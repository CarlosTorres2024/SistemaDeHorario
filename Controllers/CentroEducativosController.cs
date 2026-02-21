using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using SistemaGestionHorarios.Models;

namespace SistemaGestionHorarios.Controllers
{
    public class CentroEducativosController : Controller
    {
        private readonly AppDbContext _context;

        public CentroEducativosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CentroEducativos
        public async Task<IActionResult> Index()
        {
            var centro = await _context.CentroEducativo.FirstOrDefaultAsync();
            if (centro == null)
            {
                return RedirectToAction(nameof(Create));
            }
            return View(centro);
        }

        // GET: CentroEducativos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CentroEducativos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,Telefono,Correo,Mision,Vision")] CentroEducativo centroEducativo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(centroEducativo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(centroEducativo);
        }

        // GET: CentroEducativos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroEducativo = await _context.CentroEducativo.FindAsync(id);
            if (centroEducativo == null)
            {
                return NotFound();
            }
            return View(centroEducativo);
        }

        // POST: CentroEducativos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Direccion,Telefono,Correo,Mision,Vision")] CentroEducativo centroEducativo)
        {
            if (id != centroEducativo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centroEducativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroEducativoExists(centroEducativo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(centroEducativo);
        }

        private bool CentroEducativoExists(int id)
        {
            return _context.CentroEducativo.Any(e => e.Id == id);
        }
    }
}
