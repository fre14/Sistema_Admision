using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SAA.API.Server.Controllers;
using SAA.Application.DTOs;
using SAA.Application.Services;
using SAA.Domain.Entities;
using SAA.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

public class AdmisionControllerTests
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
    public async Task RegistrarExamen_Valido_RetornaOk()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);
        var controller = new AdmisionController(service);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Test" });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "111", IdProgramaInteres = 1 });
        context.FichasPostulacion.Add(new FichaPostulacion { IdFichaPostulacion = 1, IdPostulante = 1, IdProgramaAcademico = 1 });
        await context.SaveChangesAsync();

        var dto = new RegistrarExamenDto
        {
            IdPostulante = 1,
            Puntaje = 85.5m,
            Observaciones = "Test"
        };

        // Act
        var result = await controller.RegistrarExamen(dto);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ProcesarResultados_EjecutaCorrectamente_RetornaOk()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);
        var controller = new AdmisionController(service);

        // Act
        var result = await controller.ProcesarResultados();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ObtenerReporteIngresantes_EjecutaCorrectamente_RetornaOk()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);
        var controller = new AdmisionController(service);

        // Act
        var result = await controller.ObtenerReporteIngresantes();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        
        var lista = okResult.Value as List<ReporteIngresanteDto>;
        lista.Should().NotBeNull();
    }

    [Fact]
    public async Task ObtenerReporteTodos_EjecutaCorrectamente_RetornaOk()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new MotorAdmisionService(context);
        var controller = new AdmisionController(service);

        // Act
        var result = await controller.ObtenerReporteTodos();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }
}
