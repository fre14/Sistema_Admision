using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SAA.Application.DTOs;
using SAA.Application.Interfaces;
using SAA.Application.Services;
using SAA.Domain.Entities;
using SAA.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

/// <summary>
/// Pruebas del DbContext para verificar la configuración de Entity Framework Core.
/// Valida que SAADbContext implementa correctamente IApplicationDbContext
/// y que las configuraciones de Fluent API están bien definidas.
/// </summary>
public class DbContextTests
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
    public void SAADbContext_ImplementaIApplicationDbContext()
    {
        // Arrange & Act
        var context = GetMemoryContext();

        // Assert
        context.Should().BeAssignableTo<IApplicationDbContext>();
    }

    [Fact]
    public void SAADbContext_TieneTodasLasDbSetProperties()
    {
        // Arrange & Act
        var context = GetMemoryContext();

        // Assert
        context.Usuarios.Should().NotBeNull();
        context.Roles.Should().NotBeNull();
        context.Postulantes.Should().NotBeNull();
        context.FichasPostulacion.Should().NotBeNull();
        context.ExamenesAdmision.Should().NotBeNull();
        context.ResultadosAdmision.Should().NotBeNull();
        context.ProgramasAcademicos.Should().NotBeNull();
        context.PeriodosAdmision.Should().NotBeNull();
    }

    [Fact]
    public async Task SAADbContext_PuedeGuardarYRecuperarPostulante()
    {
        // Arrange
        var context = GetMemoryContext();
        var postulante = new Postulante
        {
            Nombres = "Test",
            Apellidos = "Entity",
            DNI = "99999999",
            Correo = "test@test.com",
            IdProgramaInteres = 1,
            Estado = "Activo"
        };

        // Act
        context.Postulantes.Add(postulante);
        await context.SaveChangesAsync();

        // Assert
        var recuperado = await context.Postulantes.FirstOrDefaultAsync(p => p.DNI == "99999999");
        recuperado.Should().NotBeNull();
        recuperado!.Nombres.Should().Be("Test");
        recuperado.IdPostulante.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task SAADbContext_PuedeGuardarEntidadesRelacionadas()
    {
        // Arrange
        var context = GetMemoryContext();

        var programa = new ProgramaAcademico { Nombre = "Sistemas", Codigo = "IS01", Estado = "Activo", Vacantes = 50 };
        context.ProgramasAcademicos.Add(programa);
        await context.SaveChangesAsync();

        var postulante = new Postulante { Nombres = "Juan", DNI = "111", IdProgramaInteres = programa.IdProgramaAcademico };
        context.Postulantes.Add(postulante);
        await context.SaveChangesAsync();

        var ficha = new FichaPostulacion
        {
            IdPostulante = postulante.IdPostulante,
            IdProgramaAcademico = programa.IdProgramaAcademico,
            NumeroTramite = "T-111",
            FechaPostulacion = DateTime.Now,
            Estado = "Registrada"
        };
        context.FichasPostulacion.Add(ficha);
        await context.SaveChangesAsync();

        // Act
        var fichaRecuperada = await context.FichasPostulacion.FirstAsync();

        // Assert
        fichaRecuperada.IdPostulante.Should().Be(postulante.IdPostulante);
        fichaRecuperada.IdProgramaAcademico.Should().Be(programa.IdProgramaAcademico);
    }

    [Fact]
    public async Task SAADbContext_DatabaseFacade_EsAccesible()
    {
        // Arrange
        var context = GetMemoryContext();
        IApplicationDbContext interfaceContext = context;

        // Act & Assert
        interfaceContext.Database.Should().NotBeNull();
        var canConnect = await interfaceContext.Database.CanConnectAsync();
        canConnect.Should().BeTrue();
    }

    [Fact]
    public async Task SAADbContext_SaveChangesAsync_RetornaNumeroDeEntidadesAfectadas()
    {
        // Arrange
        var context = GetMemoryContext();
        context.Postulantes.Add(new Postulante { DNI = "AAA", Nombres = "Test1" });
        context.Postulantes.Add(new Postulante { DNI = "BBB", Nombres = "Test2" });

        // Act
        var affected = await context.SaveChangesAsync();

        // Assert
        affected.Should().Be(2);
    }
}
