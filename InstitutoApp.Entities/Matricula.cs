namespace InstitutoApp.Entities;

public class Matricula
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int CursoId { get; set; }
    public string PeriodoAcademico { get; set; } = string.Empty;
    public DateTime FechaMatricula { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public Estudiante Estudiante { get; set; } = null!;
    public Curso Curso { get; set; } = null!;
}
