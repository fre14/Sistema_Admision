using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SAA.Application.Services;
using SAA.Domain.Entities;
using SAA.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

public class MotorAdmisionTests
{
    private SAADbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<SAADbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            
        var context = new SAADbContext(options);
        return context;
    }

    [Fact]
    public async Task ProcesarResultados_ConCuposSuficientes_ApruebaPostulante()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 2 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 60 });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado = await context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == 1);
        Assert.NotNull(resultado);
        Assert.Equal("Ingresante", resultado.Resultado);
    }

    [Fact]
    public async Task ProcesarResultados_SinCupos_AprobadoPeroNoIngresa()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 1 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.Postulantes.Add(new Postulante { IdPostulante = 2, DNI = "222", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 2, IdPostulante = 2, IdProgramaAcademico = 1 });
        
        // Postulante 2 tiene más puntaje, debe ganar el único cupo
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 60 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 2, IdPostulante = 2, IdFichaPostulacion = 2, Puntaje = 80 });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado1 = await context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == 1);
        var resultado2 = await context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == 2);
        
        Assert.NotNull(resultado1);
        Assert.NotNull(resultado2);
        
        Assert.Equal("Aprobado", resultado1.Resultado);
        Assert.Equal("Ingresante", resultado2.Resultado);
    }

    [Fact]
    public async Task ProcesarResultados_PuntajeMenorA50_DesapruebaPostulante()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas", Vacantes = 10 });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 40 });
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultado = await context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == 1);
        Assert.NotNull(resultado);
        Assert.Equal("Desaprobado", resultado.Resultado);
    }

    [Fact]
    public async Task ObtenerReporteIngresantesAsync_DevuelveSoloIngresantesOrdenados()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas" });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", Nombres = "Juan", Apellidos = "Perez", IdProgramaInteres = 1 });
        context.Postulantes.Add(new Postulante { IdPostulante = 2, DNI = "222", Nombres = "Maria", Apellidos = "Gomez", IdProgramaInteres = 1 });
        context.Postulantes.Add(new Postulante { IdPostulante = 3, DNI = "333", Nombres = "Pedro", Apellidos = "Torres", IdProgramaInteres = 1 });
        
        context.ResultadosAdmision.Add(new ResultadoAdmision { IdResultado = 1, IdPostulante = 1, IdProgramaAcademico = 1, Calificacion = 60, Resultado = "Ingresante", OrdenMerito = 2, FechaResultado = DateTime.Now });
        context.ResultadosAdmision.Add(new ResultadoAdmision { IdResultado = 2, IdPostulante = 2, IdProgramaAcademico = 1, Calificacion = 80, Resultado = "Ingresante", OrdenMerito = 1, FechaResultado = DateTime.Now });
        context.ResultadosAdmision.Add(new ResultadoAdmision { IdResultado = 3, IdPostulante = 3, IdProgramaAcademico = 1, Calificacion = 40, Resultado = "Desaprobado", OrdenMerito = 3, FechaResultado = DateTime.Now });
        
        await context.SaveChangesAsync();

        // Act
        var reporte = await service.ObtenerReporteIngresantesAsync();

        // Assert
        Assert.NotNull(reporte);
        Assert.Equal(2, reporte.Count);
        
        // Verifica el ordenamiento por puesto
        Assert.Equal("Maria", reporte[0].Nombres);
        Assert.Equal(80, reporte[0].Puntaje);
        Assert.Equal(1, reporte[0].Puesto);

        Assert.Equal("Juan", reporte[1].Nombres);
        Assert.Equal(60, reporte[1].Puntaje);
        Assert.Equal(2, reporte[1].Puesto);
    }

    [Fact]
    public async Task ObtenerReporteTodosAsync_DevuelveTodosOrdenadosPorPuntaje()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Sistemas" });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", Nombres = "Juan", Apellidos = "Perez", IdProgramaInteres = 1 });
        context.Postulantes.Add(new Postulante { IdPostulante = 2, DNI = "222", Nombres = "Maria", Apellidos = "Gomez", IdProgramaInteres = 1 });
        
        context.ResultadosAdmision.Add(new ResultadoAdmision { IdResultado = 1, IdPostulante = 1, IdProgramaAcademico = 1, Calificacion = 40, Resultado = "Desaprobado", FechaResultado = DateTime.Now });
        context.ResultadosAdmision.Add(new ResultadoAdmision { IdResultado = 2, IdPostulante = 2, IdProgramaAcademico = 1, Calificacion = 80, Resultado = "Ingresante", FechaResultado = DateTime.Now });
        
        await context.SaveChangesAsync();

        // Act
        var reporte = await service.ObtenerReporteTodosAsync();

        // Assert
        Assert.NotNull(reporte);
        Assert.Equal(2, reporte.Count);
        
        Assert.Equal("Maria", reporte[0].Nombres);
        Assert.Equal("Ingresante", reporte[0].Estado);

        Assert.Equal("Juan", reporte[1].Nombres);
        Assert.Equal("Desaprobado", reporte[1].Estado);
    }

    [Fact]
    public async Task ProcesarResultados_PostulanteNoAcumulaCupos_SeAsignaAUnSoloPrograma()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Medicina", Vacantes = 1 });
        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 2, Nombre = "Sistemas", Vacantes = 1 });
        
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        
        // El postulante tiene fichas en dos programas distintos
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 2, IdPostulante = 1, IdProgramaAcademico = 2 });
        
        // Da el examen para ambos programas con nota perfecta
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 1, IdPostulante = 1, IdFichaPostulacion = 1, Puntaje = 100 });
        context.ExamenesAdmision.Add(new ExamenAdmision { IdExamen = 2, IdPostulante = 1, IdFichaPostulacion = 2, Puntaje = 90 });
        
        await context.SaveChangesAsync();

        // Act
        await service.ProcesarResultadosAsync();

        // Assert
        var resultados = await context.ResultadosAdmision.Where(r => r.IdPostulante == 1).ToListAsync();
        
        // Debería haberse guardado un solo resultado (en el primer programa evaluado que es Medicina)
        Assert.Single(resultados);
        Assert.Equal(1, resultados[0].IdProgramaAcademico);
        Assert.Equal("Ingresante", resultados[0].Resultado);
    }

    [Fact]
    public async Task RegistrarExamenAsync_PostulanteNoEncontrado_LanzaExcepcion()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);
        
        var dto = new Application.DTOs.RegistrarExamenDto 
        { 
            IdPostulante = 999, 
            Puntaje = 100 
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.RegistrarExamenAsync(dto));
    }

    [Fact]
    public async Task RegistrarExamenAsync_PostulanteExiste_RegistraExamen()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);
        
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        await context.SaveChangesAsync();

        var dto = new Application.DTOs.RegistrarExamenDto 
        { 
            IdPostulante = 1, 
            Puntaje = 85.5m,
            Observaciones = "Ninguna"
        };

        // Act
        await service.RegistrarExamenAsync(dto);

        // Assert
        var examen = await context.ExamenesAdmision.FirstOrDefaultAsync(e => e.IdPostulante == 1);
        Assert.NotNull(examen);
        Assert.Equal(85.5m, examen.Puntaje);
        Assert.Equal("Ninguna", examen.Observaciones);
    }

    [Fact]
    public async Task ObtenerReporteIngresantesAsync_SinResultados_DevuelveListaVacia()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        // Act
        var reporte = await service.ObtenerReporteIngresantesAsync();

        // Assert
        Assert.NotNull(reporte);
        Assert.Empty(reporte);
    }

    [Fact]
    public async Task ObtenerReporteTodosAsync_SinResultados_DevuelveListaVacia()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);

        // Act
        var reporte = await service.ObtenerReporteTodosAsync();

        // Assert
        Assert.NotNull(reporte);
        Assert.Empty(reporte);
    }
}
