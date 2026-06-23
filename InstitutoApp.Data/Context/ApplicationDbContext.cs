using InstitutoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstitutoApp.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones)
        : base(opciones)
    {
    }

    public DbSet<Estudiante> Estudiantes { get; set; } = null!;
    public DbSet<Curso> Cursos { get; set; } = null!;
    public DbSet<Matricula> Matriculas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.ToTable("Estudiantes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cedula).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
            entity.Property(e => e.PrimerApellido).HasMaxLength(80).IsRequired();
            entity.Property(e => e.SegundoApellido).HasMaxLength(80);
            entity.Property(e => e.CorreoElectronico).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Telefono).HasMaxLength(25);
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.Cedula).IsUnique();
            entity.HasIndex(e => e.CorreoElectronico).IsUnique();
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.ToTable("Cursos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.PeriodoAcademico)
                .HasMaxLength(10)
                .IsRequired()
                .HasDefaultValue("2026-I");
            entity.Property(e => e.FechaInicioMatricula).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.FechaCierreMatricula).HasDefaultValueSql("DATEADD(day, 365, SYSUTCDATETIME())");
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.ToTable("Matriculas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PeriodoAcademico)
                .HasMaxLength(10)
                .IsRequired()
                .HasDefaultValue("2026-I");
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => new { e.EstudianteId, e.CursoId }).IsUnique();
            entity.HasOne(e => e.Estudiante)
                .WithMany(e => e.Matriculas)
                .HasForeignKey(e => e.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Curso)
                .WithMany(e => e.Matriculas)
                .HasForeignKey(e => e.CursoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
