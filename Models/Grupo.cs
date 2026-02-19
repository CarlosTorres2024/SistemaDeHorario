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

        [Required(ErrorMessage = "El nombre del grupo es obligatorio")]
        [Display(Name = "Nombre del Grupo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nivel educativo es obligatorio")]
        [Display(Name = "Nivel Educativo")]
        public string? NivelEducativo { get; set; }

        // Navegación
        public ICollection<Horario>? Horarios { get; set; }
    }
}
