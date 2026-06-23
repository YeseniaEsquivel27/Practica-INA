namespace InstitutoApp.Entities;

public class Estudiante
{
    public int Id { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string PrimerApellido { get; set; } = string.Empty;
    public string? SegundoApellido { get; set; }
    public string CorreoElectronico { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
