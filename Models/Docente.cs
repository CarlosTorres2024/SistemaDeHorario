using System.ComponentModel.DataAnnotations;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa un profesor o docente en la institución.
    /// </summary>
    public class Docente
    {
        [Key]
        public int IdDocente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Display(Name = "Especialidad")]
        public string? Especialidad { get; set; }

        [Required(ErrorMessage = "Las horas máximas son obligatorias")]
        [Range(1, 60, ErrorMessage = "Las horas deben estar entre 1 y 60")]
        [Display(Name = "Horas Máximas Semanales")]
        public int HorasMaximas { get; set; }

        // Navegación
        public ICollection<Horario>? Horarios { get; set; }
    }
}
