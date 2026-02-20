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

        [Required(ErrorMessage = "Es necesario asignar un nombre o número al aula.")]
        [Display(Name = "Nombre o Número")]
        public string NombreNumero { get; set; }

        [Required(ErrorMessage = "La capacidad del aula es requerida.")]
        [Range(1, 200, ErrorMessage = "La capacidad debe ser un número entre 1 y 200.")]
        public int Capacidad { get; set; }

        // Navegación
        public ICollection<Horario>? Horarios { get; set; }
    }
}
