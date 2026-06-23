using System.ComponentModel.DataAnnotations;

namespace InstitutoApp.DTOs;

public class CursoCreateDTO
{
    [Required]
    [MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int CupoMaximo { get; set; }

    [Required]
    [RegularExpression(
        @"^\d{4}-(I|II|III)$",
        ErrorMessage = "El período debe usar el formato AAAA-I, AAAA-II o AAAA-III.")]
    public string PeriodoAcademico { get; set; } = string.Empty;

    [Required]
    public DateTime? FechaInicioMatricula { get; set; }

    [Required]
    public DateTime? FechaCierreMatricula { get; set; }
}

public class CursoUpdateDTO : CursoCreateDTO
{
    public bool Estado { get; set; } = true;
}

public class CursoResponseDTO
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
}
