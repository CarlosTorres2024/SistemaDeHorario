using System.ComponentModel.DataAnnotations;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa la información del centro educativo.
    /// </summary>
    public class CentroEducativo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del centro es obligatorio.")]
        [Display(Name = "Nombre del Centro")]
        public string Nombre { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string? Telefono { get; set; }

        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string? Correo { get; set; }

        [Display(Name = "Misión")]
        public string? Mision { get; set; }

        [Display(Name = "Visión")]
        public string? Vision { get; set; }
    }
}
