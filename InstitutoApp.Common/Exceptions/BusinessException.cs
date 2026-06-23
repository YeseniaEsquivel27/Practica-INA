namespace InstitutoApp.Common.Exceptions;

public class ExcepcionNegocio : Exception
{
    public ExcepcionNegocio(string mensaje)
        : base(mensaje)
    {
    }
}
