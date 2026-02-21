using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionHorarios.Data;
using Rotativa.AspNetCore;

namespace SistemaGestionHorarios.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> GenerarHorarioPDF()
        {
            // Obtener todos los datos necesarios para el reporte
            var docentes = await _context.Docentes.ToListAsync();
            var asignaturas = await _context.Asignaturas.ToListAsync();
            var grupos = await _context.Grupos.ToListAsync();
            var horarios = await _context.Horarios
                .Include(h => h.Docente)
                .Include(h => h.Asignatura)
                .Include(h => h.Aula)
                .Include(h => h.Grupo)
                .OrderBy(h => h.DiaSemana)
                .ThenBy(h => h.HoraInicio)
                .ToListAsync();

            // ViewModel anónimo o dedicado para la vista
            var modeloReporte = new 
            {
                Docentes = docentes,
                Asignaturas = asignaturas,
                Grupos = grupos,
                Horarios = horarios,
                FechaGeneracion = DateTime.Now
            };

            // Retornar la vista como PDF
            return new ViewAsPdf("HorarioPDF", modeloReporte)
            {
                FileName = $"ReporteHorarios_{DateTime.Now:yyyyMMdd}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--footer-center \"Página [page] de [toPage]\" --footer-font-size 10 --footer-spacing 10"
            };
        }
    }
}
