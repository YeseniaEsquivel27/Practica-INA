using InstitutoApp.Data.Context;
using InstitutoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstitutoApp.Data.Semilla;

public static class InicializadorBaseDatos
{
    public static async Task CargarDatosInicialesAsync(ApplicationDbContext contexto)
    {
        if (!await contexto.Estudiantes.AnyAsync())
        {
            var fechaActual = DateTime.UtcNow;

            contexto.Estudiantes.AddRange(
                CrearEstudiante("101110111", "Ana", "Pérez", "Vargas", "ana.perez@innovatech.ac.cr", "8888-1001", fechaActual),
                CrearEstudiante("102220222", "Carlos", "Ramírez", "Soto", "carlos.ramirez@innovatech.ac.cr", "8888-1002", fechaActual),
                CrearEstudiante("103330333", "María", "González", "Mora", "maria.gonzalez@innovatech.ac.cr", "8888-1003", fechaActual),
                CrearEstudiante("104440444", "Diego", "Jiménez", "Rojas", "diego.jimenez@innovatech.ac.cr", "8888-1004", fechaActual),
                CrearEstudiante("105550555", "Sofía", "Castro", "León", "sofia.castro@innovatech.ac.cr", "8888-1005", fechaActual),
                CrearEstudiante("106660666", "Luis", "Alvarado", "Chaves", "luis.alvarado@innovatech.ac.cr", "8888-1006", fechaActual));

            await contexto.SaveChangesAsync();
        }

        if (!await contexto.Cursos.AnyAsync())
        {
            var fechaActual = DateTime.UtcNow;
            var fechaInicioMatricula = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var fechaCierreMatricula = new DateTime(2026, 12, 31, 23, 59, 59, DateTimeKind.Utc);

            contexto.Cursos.AddRange(
                CrearCurso("Programación Web con .NET", "Creación de API REST con ASP.NET Core.", 25, fechaInicioMatricula, fechaCierreMatricula, fechaActual),
                CrearCurso("Bases de Datos SQL Server", "Diseño relacional y consultas SQL.", 20, fechaInicioMatricula, fechaCierreMatricula, fechaActual),
                CrearCurso("Entity Framework Core", "Persistencia con Code First y migraciones.", 15, fechaInicioMatricula, fechaCierreMatricula, fechaActual),
                CrearCurso("Arquitectura de Software", "Capas, DTO y patrones de diseño.", 18, fechaInicioMatricula, fechaCierreMatricula, fechaActual));

            await contexto.SaveChangesAsync();
        }

        if (await contexto.Matriculas.AnyAsync())
        {
            return;
        }

        var estudiantes = await contexto.Estudiantes.ToDictionaryAsync(estudiante => estudiante.Cedula);
        var cursos = await contexto.Cursos.ToDictionaryAsync(curso => curso.Nombre);
        var fechaMatricula = DateTime.UtcNow;

        contexto.Matriculas.AddRange(
            CrearMatricula("101110111", "Programación Web con .NET", estudiantes, cursos, fechaMatricula),
            CrearMatricula("101110111", "Bases de Datos SQL Server", estudiantes, cursos, fechaMatricula),
            CrearMatricula("102220222", "Programación Web con .NET", estudiantes, cursos, fechaMatricula),
            CrearMatricula("102220222", "Entity Framework Core", estudiantes, cursos, fechaMatricula),
            CrearMatricula("103330333", "Bases de Datos SQL Server", estudiantes, cursos, fechaMatricula),
            CrearMatricula("103330333", "Arquitectura de Software", estudiantes, cursos, fechaMatricula),
            CrearMatricula("104440444", "Programación Web con .NET", estudiantes, cursos, fechaMatricula),
            CrearMatricula("105550555", "Entity Framework Core", estudiantes, cursos, fechaMatricula),
            CrearMatricula("105550555", "Arquitectura de Software", estudiantes, cursos, fechaMatricula),
            CrearMatricula("106660666", "Bases de Datos SQL Server", estudiantes, cursos, fechaMatricula));

        await contexto.SaveChangesAsync();
    }

    private static Estudiante CrearEstudiante(string cedula, string nombre, string primerApellido, string segundoApellido, string correoElectronico, string telefono, DateTime fechaActual)
    {
        return new Estudiante
        {
            Cedula = cedula,
            Nombre = nombre,
            PrimerApellido = primerApellido,
            SegundoApellido = segundoApellido,
            CorreoElectronico = correoElectronico,
            Telefono = telefono,
            Estado = true,
            FechaCreacion = fechaActual,
            FechaActualizacion = fechaActual
        };
    }

    private static Curso CrearCurso(string nombre, string descripcion, int cupoMaximo, DateTime fechaInicioMatricula, DateTime fechaCierreMatricula, DateTime fechaActual)
    {
        return new Curso
        {
            Nombre = nombre,
            Descripcion = descripcion,
            CupoMaximo = cupoMaximo,
            PeriodoAcademico = "2026-I",
            FechaInicioMatricula = fechaInicioMatricula,
            FechaCierreMatricula = fechaCierreMatricula,
            Estado = true,
            FechaCreacion = fechaActual,
            FechaActualizacion = fechaActual
        };
    }

    private static Matricula CrearMatricula(string cedula, string nombreCurso, Dictionary<string, Estudiante> estudiantes, Dictionary<string, Curso> cursos, DateTime fechaMatricula)
    {
        return new Matricula
        {
            EstudianteId = estudiantes[cedula].Id,
            CursoId = cursos[nombreCurso].Id,
            PeriodoAcademico = "2026-I",
            FechaMatricula = fechaMatricula,
            Estado = true,
            FechaActualizacion = fechaMatricula
        };
    }
}
