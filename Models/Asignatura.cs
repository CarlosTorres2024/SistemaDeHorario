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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre de la Asignatura")]
        public string Nombre { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Las horas semanales deben ser válidas")]
        [Display(Name = "Horas Semanales")]
        public int HorasSemanales { get; set; }

        // Navegación
        public ICollection<Horario>? Horarios { get; set; }
    }
}
