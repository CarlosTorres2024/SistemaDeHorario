using System.ComponentModel.DataAnnotations;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa un grupo de estudiantes.
    /// </summary>
    public class Grupo
    {
        [Key]
        public int IdGrupo { get; set; }

        [Required(ErrorMessage = "Por favor, asigne un nombre al grupo.")]
        [Display(Name = "Nombre del Grupo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nivel educativo es un dato requerido.")]
        [Display(Name = "Nivel Educativo")]
        public string? NivelEducativo { get; set; }

        // Navegación
        public ICollection<Horario>? Horarios { get; set; }
    }
}
