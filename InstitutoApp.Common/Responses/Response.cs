namespace InstitutoApp.Common.Respuestas;

public class Respuesta<T>
{
    public bool EsExitosa { get; init; }
    public string Mensaje { get; init; } = string.Empty;
    public T? Datos { get; init; }

    public static Respuesta<T> Correcta(T datos, string mensaje = "Operación realizada correctamente.")
    {
        return new Respuesta<T>
        {
            EsExitosa = true,
            Mensaje = mensaje,
            Datos = datos
        };
    }

    public static Respuesta<T> Incorrecta(string mensaje)
    {
        return new Respuesta<T>
        {
            EsExitosa = false,
            Mensaje = mensaje
        };
    }
}
