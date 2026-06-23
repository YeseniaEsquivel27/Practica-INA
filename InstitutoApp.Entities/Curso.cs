namespace InstitutoApp.Entities;

public class Curso
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CupoMaximo { get; set; }
    public string PeriodoAcademico { get; set; } = string.Empty;
    public DateTime FechaInicioMatricula { get; set; }
    public DateTime FechaCierreMatricula { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
