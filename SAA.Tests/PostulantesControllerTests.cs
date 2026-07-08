using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

public class PostulantesControllerTests
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
    public async Task CrearPostulante_Valido_RetornaOk()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new PostulanteService(context);
        var controller = new PostulantesController(service);

        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Test" });
        await context.SaveChangesAsync();

        var dto = new CrearPostulanteDto
        {
            Nombres = "Test",
            Apellidos = "User",
            DNI = "12345678",
            Correo = "test@test.com",
            Contrasena = "123",
            IdProgramaInteres = 1
        };

        // Act
        var result = await controller.CrearPostulante(dto);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        
        var responseDto = okResult.Value as PostulanteResponseDto;
        responseDto.Should().NotBeNull();
        responseDto!.DNI.Should().Be("12345678");
    }

    [Fact]
    public async Task CrearPostulante_DniDuplicado_RetornaBadRequest()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new PostulanteService(context);
        var controller = new PostulantesController(service);

        context.Postulantes.Add(new Postulante { DNI = "12345678", Nombres = "Existente" });
        await context.SaveChangesAsync();

        var dto = new CrearPostulanteDto { DNI = "12345678", Nombres = "Nuevo" };

        // Act
        var result = await controller.CrearPostulante(dto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task ObtenerPostulantes_RetornaOkConLista()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new PostulanteService(context);
        var controller = new PostulantesController(service);

        context.Postulantes.Add(new Postulante { DNI = "111", Nombres = "Juan" });
        context.Postulantes.Add(new Postulante { DNI = "222", Nombres = "Maria" });
        await context.SaveChangesAsync();

        // Act
        var result = await controller.ObtenerPostulantes();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var lista = okResult!.Value as List<PostulanteResponseDto>;
        lista.Should().NotBeNull();
        lista!.Count.Should().Be(2);
    }

    [Fact]
    public async Task MiResultado_SinToken_RetornaUnauthorized()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new PostulanteService(context);
        var controller = new PostulantesController(service)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Act
        var result = await controller.MiResultado();

        // Assert
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task MiResultado_DniNoExiste_RetornaNotFound()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new PostulanteService(context);
        
        var claims = new[] { new Claim(ClaimTypes.Name, "99999999") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var controller = new PostulantesController(service)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            }
        };

        // Act
        var result = await controller.MiResultado();

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task MiResultado_Valido_RetornaOkConResultado()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new PostulanteService(context);
        
        context.ProgramasAcademicos.Add(new ProgramaAcademico { IdProgramaAcademico = 1, Nombre = "Test" });
        context.Postulantes.Add(new Postulante { IdPostulante = 1, DNI = "12345678", IdProgramaInteres = 1 });
        context.ResultadosAdmision.Add(new ResultadoAdmision { IdPostulante = 1, Resultado = "Ingresante", Calificacion = 90 });
        await context.SaveChangesAsync();
        
        var claims = new[] { new Claim(ClaimTypes.Name, "12345678") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var controller = new PostulantesController(service)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            }
        };

        // Act
        var result = await controller.MiResultado();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var dto = okResult!.Value as MiResultadoDto;
        dto.Should().NotBeNull();
        dto!.Estado.Should().Be("Ingresante");
        dto.Puntaje.Should().Be(90);
    }
}
