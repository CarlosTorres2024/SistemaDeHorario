using System.ComponentModel.DataAnnotations;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa un curso académico.
    /// </summary>
    public class Curso
    {
        [Key]
        public int IdCurso { get; set; }

        [Required]
        [Display(Name = "Nombre del Curso")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Modalidad (Inicial, Primaria, Media, Universitaria)")]
        public string Modalidad { get; set; }
    }
}
