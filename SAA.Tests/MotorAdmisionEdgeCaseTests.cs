using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SAA.Application.DTOs;
using SAA.Application.Services;
using SAA.Domain.Entities;
using SAA.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

/// <summary>
/// Pruebas de casos límite y escenarios especiales del Motor de Admisión.
/// Complementa MotorAdmisionTests.cs con escenarios edge-case.
/// </summary>
public class MotorAdmisionEdgeCaseTests
{
    private SAADbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<SAADbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new SAADbContext(options);
    }

    [Fact]
    public async Task ProcesarResultados_BaseDeDatosVacia_NoLanzaExcepcion()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        // Act
        var act = () => service.ProcesarResultadosAsync();

        // Assert
        await act.Should().NotThrowAsync();
        var resultados = await context.ResultadosAdmision.ToListAsync();
        resultados.Should().BeEmpty();
    }

    [Fact]
    public async Task ProcesarResultados_PuntajeExactoDe50_ApruebaPostulante()
    {
        // Arrange - Boundary test: exactamente 50.0 debe aprobar
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 10 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 50.0m });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado = await context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == 1);
        resultado.Should().NotBeNull();
        resultado!.Resultado.Should().Be("Ingresante");
    }

    [Fact]
    public async Task ProcesarResultados_PuntajeJustoDebajoDelLimite_Desaprueba()
    {
        // Arrange - 49.99 debe desaprobar
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 10 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 49.99m });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado = await context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == 1);
        resultado.Should().NotBeNull();
        resultado!.Resultado.Should().Be("Desaprobado");
    }

    [Fact]
    public async Task ProcesarResultados_CeroVacantes_TodosAprobadosPeroNoIngresantes()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Medicina", Vacantes = 0 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 95.0m });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado = await context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == 1);
        resultado.Should().NotBeNull();
        resultado!.Resultado.Should().Be("Aprobado"); // Alto puntaje pero 0 vacantes
    }

    [Fact]
    public async Task ProcesarResultados_MultiplesProgramas_ClasificaCorrectamente()
    {
        // Arrange - Dos programas, postulantes distribuidos
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 1 });
        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 2, Nombre = "Medicina", Vacantes = 1 });

        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "AAA", IdProgramaInteres = 1 });
        context.Postulantes.Add(new Postulante { IdPostulante = 2, DNI = "BBB", IdProgramaInteres = 2 });

        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 2, IdPostulante = 2, IdProgramaAcademico = 2 });

        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 70 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 2, IdPostulante = 2, IdFichaPostulacion = 2, Puntaje = 80 });

        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert - Ambos deben ser ingresantes en sus respectivos programas
        var resultados = await context.ResultadosAdmision.ToListAsync();
        resultados.Should().HaveCount(2);
        resultados.Should().OnlyContain(r => r.Resultado == "Ingresante");
    }

    [Fact]
    public async Task ProcesarResultados_OrdenMerito_SeAsignaSecuencialmente()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 5 });

        for (int i = 1; i <= 3; i++)
        {
            context.Postulantes.Add(new Postulante { IdPostulante = i, DNI = $"DNI{i}", IdProgramaInteres = 1 });
            context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = i, IdPostulante = i, IdProgramaAcademico = 1 });
            context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = i, IdPostulante = i, IdFichaPostulacion = i, Puntaje = (100 - i * 10) });
        }
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultados = await context.ResultadosAdmision.OrderBy(r => r.OrdenMerito).ToListAsync();
        resultados.Should().HaveCount(3);
        resultados[0].OrdenMerito.Should().Be(1);
        resultados[1].OrdenMerito.Should().Be(2);
        resultados[2].OrdenMerito.Should().Be(3);
        resultados[0].Calificacion.Should().BeGreaterThan(resultados[1].Calificacion.Value);
    }

    [Fact]
    public async Task ProcesarResultados_FechaResultado_SeEstablece()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);
        var antesDeProcesar = DateTime.Now.AddSeconds(-1);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 5 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 75 });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado = await context.ResultadosAdmision.FirstAsync();
        resultado.FechaResultado.Should().BeAfter(antesDeProcesar);
    }

    [Fact]
    public async Task ProcesarResultados_Reprocesar_ReemplazaResultadosAnteriores()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 5 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 75 });
        await context.SaveChangesAsync();

        // Act - Procesar dos veces
        await service.ProcesarResultadosAsync();
        await service.ProcesarResultadosAsync();

        // Assert - Debe haber solo 1 resultado (no duplicados)
        var resultados = await context.ResultadosAdmision.Where(r => r.IdPostulante == 1).ToListAsync();
        resultados.Should().HaveCount(1);
    }

    [Fact]
    public async Task ProcesarResultados_PuntajePerfecto100_EsIngresante()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 1 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 100.0m });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado = await context.ResultadosAdmision.FirstAsync();
        resultado.Resultado.Should().Be("Ingresante");
        resultado.Calificacion.Should().Be(100.0m);
        resultado.OrdenMerito.Should().Be(1);
    }
}
