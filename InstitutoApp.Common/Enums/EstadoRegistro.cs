namespace InstitutoApp.Common.Enums;

public enum EstadoRegistro
{
    Inactivo = 0,
    Activo = 1
}

public enum ResultadoCreacionMatricula
{
    Creada,
    Duplicada,
    CupoCompleto,
    LimiteCursosAlcanzado
}
