using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Representa la asignación de un horario a un docente, grupo y aula.
    /// </summary>
    public class Horario : IValidatableObject
    {
        [Key]
        public int IdHorario { get; set; }

        [Required(ErrorMessage = "El docente es obligatorio")]
        [Display(Name = "Docente")]
        public int IdDocente { get; set; }

        [ForeignKey("IdDocente")]
        public Docente? Docente { get; set; }

        [Required(ErrorMessage = "La asignatura es obligatoria")]
        [Display(Name = "Asignatura")]
        public int IdAsignatura { get; set; }

        [ForeignKey("IdAsignatura")]
        public Asignatura? Asignatura { get; set; }

        [Display(Name = "Aula")]
        public int? IdAula { get; set; }

        [ForeignKey("IdAula")]
        public Aula? Aula { get; set; }

        [Display(Name = "Grupo")]
        public int? IdGrupo { get; set; }

        [ForeignKey("IdGrupo")]
        public Grupo? Grupo { get; set; }

        [Required(ErrorMessage = "El día de la semana es obligatorio")]
        [Display(Name = "Día de la Semana")]
        public string DiaSemana { get; set; } // Lunes, Martes, etc.

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Fin")]
        public TimeSpan HoraFin { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HoraFin <= HoraInicio)
            {
                yield return new ValidationResult(
                    "La hora de fin debe ser posterior a la hora de inicio.",
                    new[] { nameof(HoraFin) });
            }
        }
    }
}
