using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAA.API.Server.Controllers;
using SAA.Application.DTOs;
using SAA.Application.Services;
using SAA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SAA.Tests;

public class AdmisionControllerTests
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
    public async Task ObtenerReporteIngresantes_ReturnsOkResult()
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
        okResult!.Value.Should().BeAssignableTo<List<ReporteIngresanteDto>>();
    }

    [Fact]
    public async Task RegistrarExamen_ReturnsOkResult()
    {
        // Arrange
        var context = GetMemoryContext();
        context.Postulantes.Add(new Domain.Entities.Postulante { IdPostulante = 1, DNI = "123", IdProgramaInteres = 1 });
        await context.SaveChangesAsync();

        var service = new MotorAdmisionService(context);
        var controller = new AdmisionController(service);

        var dto = new RegistrarExamenDto { IdPostulante = 1, Puntaje = 90 };

        // Act
        var result = await controller.RegistrarExamen(dto);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
    }

    [Fact]
    public async Task ProcesarResultados_ReturnsOkResult()
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
    }

    [Fact]
    public async Task ObtenerReporteTodos_ReturnsOkResult()
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
        okResult!.Value.Should().BeAssignableTo<List<ReporteIngresanteDto>>();
    }
}
