using System.ComponentModel.DataAnnotations;

namespace InstitutoApp.DTOs;

public class MatriculaCreateDTO
{
    [Range(1, int.MaxValue)]
    public int EstudianteId { get; set; }

    [Range(1, int.MaxValue)]
    public int CursoId { get; set; }
}

public class MatriculaResponseDTO
{
    public int Id { get; set; }
    public string PeriodoAcademico { get; set; } = string.Empty;
    public DateTime FechaMatricula { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public EstudianteResumenDTO Estudiante { get; set; } = null!;
    public CursoResumenDTO Curso { get; set; } = null!;
}

public class EstudianteResumenDTO
{
    public int Id { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
}

public class CursoResumenDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
