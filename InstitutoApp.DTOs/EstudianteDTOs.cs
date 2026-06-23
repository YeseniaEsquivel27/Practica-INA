using System.ComponentModel.DataAnnotations;

namespace InstitutoApp.DTOs;

public class EstudianteCreateDTO
{
    [Required]
    [MaxLength(11)]
    [RegularExpression(
        @"^[1-9]-?\d{4}-?\d{4}$",
        ErrorMessage = "La cédula debe tener el formato costarricense: 9 dígitos, con un primer dígito entre 1 y 9. Se permiten guiones (ejemplo: 2-0717-0578).")]
    public string Cedula { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string PrimerApellido { get; set; } = string.Empty;

    [MaxLength(80)]
    public string? SegundoApellido { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Phone]
    [MaxLength(25)]
    public string? Telefono { get; set; }
}

public class EstudianteUpdateDTO : EstudianteCreateDTO
{
    public bool Estado { get; set; } = true;
}

public class EstudianteResponseDTO
{
    public int Id { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
}
