using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstitutoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicBusinessRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Matriculas",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<string>(
                name: "PeriodoAcademico",
                table: "Matriculas",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "2026-I");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Estudiantes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Cursos",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCierreMatricula",
                table: "Cursos",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "DATEADD(day, 365, SYSUTCDATETIME())");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioMatricula",
                table: "Cursos",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<string>(
                name: "PeriodoAcademico",
                table: "Cursos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "2026-I");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "PeriodoAcademico",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "FechaCierreMatricula",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "FechaInicioMatricula",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "PeriodoAcademico",
                table: "Cursos");
        }
    }
}
