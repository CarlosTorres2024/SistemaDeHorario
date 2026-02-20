using System.ComponentModel.DataAnnotations;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa la información del centro educativo.
    /// </summary>
    public class CentroEducativo
    {
        [Key]
        public int IdCentro { get; set; }

        [Required(ErrorMessage = "El nombre del centro educativo es fundamental.")]
        [Display(Name = "Nombre del Centro")]
        public string Nombre { get; set; }

        public string? Dirección { get; set; }

        public string? Teléfono { get; set; }
    }
}
