using System.ComponentModel.DataAnnotations;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa una materia o asignatura que se imparte.
    /// </summary>
    public class Asignatura
    {
        [Key]
        public int IdAsignatura { get; set; }

        [Required(ErrorMessage = "Por favor, indique el nombre de la asignatura.")]
        [Display(Name = "Nombre de la Asignatura")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe especificar la cantidad de horas semanales.")]
        [Range(1, 20, ErrorMessage = "Las horas semanales deben estar entre 1 y 20.")]
        [Display(Name = "Horas Semanales")]
        public int HorasSemanales { get; set; }

        // Navegación
        public ICollection<Horario>? Horarios { get; set; }
    }
}
