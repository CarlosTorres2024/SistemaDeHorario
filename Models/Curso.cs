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

        [Required(ErrorMessage = "Indique el nombre del curso académico.")]
        [Display(Name = "Nombre del Curso")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Seleccione o escriba la modalidad del curso.")]
        [Display(Name = "Modalidad (Inicial, Primaria, Media, Universitaria)")]
        public string Modalidad { get; set; }
    }
}
