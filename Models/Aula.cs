using System.ComponentModel.DataAnnotations;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa un espacio físico donde se imparten clases.
    /// </summary>
    public class Aula
    {
        [Key]
        public int IdAula { get; set; }

        [Required(ErrorMessage = "El nombre o número es obligatorio")]
        [Display(Name = "Nombre o Número")]
        public string NombreNumero { get; set; }

        [Required]
        [Range(1, 200, ErrorMessage = "La capacidad debe ser válida")]
        public int Capacidad { get; set; }

        // Navegación
        public ICollection<Horario>? Horarios { get; set; }
    }
}
