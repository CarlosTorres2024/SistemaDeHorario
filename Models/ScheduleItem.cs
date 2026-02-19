namespace SistemaGestionHorarios.Models
{
    /// <summary>
    /// Modelo solicitado por el docente para la transferencia de datos (DTO) o entidad de dominio.
    /// </summary>
    public class ScheduleItem
    {
        public int Id { get; set; }

        public string SubjectName { get; set; } // Asignatura

        public string TeacherName { get; set; } // Profesor

        public string DayOfWeek { get; set; }   // Lunes, Martes, etc.

        public string StartTime { get; set; }    // Ej: 08:00

        public string EndTime { get; set; }      // Ej: 08:50

        public string RoomNumber { get; set; }   // Aula

        public string GradeLevel { get; set; }   // Grado (Ej: 4to de Media)
    }
}
