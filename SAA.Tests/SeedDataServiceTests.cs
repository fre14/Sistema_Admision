using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using SAA.Infrastructure.Data;
using SAA.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

/// <summary>
/// Pruebas para SeedDataService, asegurando que los datos iniciales
/// (Admin, Programas, Postulantes de prueba) se siembren correctamente.
/// </summary>
public class SeedDataServiceTests
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
    public async Task SeedAsync_BaseDeDatosVacia_CreaUsuarioAdmin()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new SeedDataService(context);

        // Act
        await service.SeedAsync();

        // Assert
        var admin = await context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == "fredy");
        admin.Should().NotBeNull();
        admin!.Rol.Should().Be("Administrador");
    }

    [Fact]
    public async Task SeedAsync_BaseDeDatosVacia_CreaProgramasAcademicos()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new SeedDataService(context);

        // Act
        await service.SeedAsync();

        // Assert
        var programas = await context.ProgramasAcademicos.ToListAsync();
        programas.Should().HaveCountGreaterOrEqualTo(3);
        programas.Should().Contain(p => p.Nombre.Contains("Ingeniería de Sistemas"));
        programas.Should().Contain(p => p.Nombre.Contains("Medicina Humana"));
        programas.Should().Contain(p => p.Nombre.Contains("Derecho"));
    }

    [Fact]
    public async Task SeedAsync_BaseDeDatosVacia_CreaUsuarioPruebaEspecifico()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new SeedDataService(context);

        // Act
        await service.SeedAsync();

        // Assert
        var prueba = await context.Postulantes.FirstOrDefaultAsync(p => p.DNI == "12345678");
        prueba.Should().NotBeNull();
        prueba!.Nombres.Should().Be("Prueba");
    }

    [Fact]
    public async Task SeedAsync_YaEjecutado_NoDuplicaDatos()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new SeedDataService(context);
        
        // Ejecutar por primera vez
        await service.SeedAsync();
        var adminCount1 = await context.Usuarios.CountAsync(u => u.NombreUsuario == "fredy");
        
        // Act - Ejecutar por segunda vez
        await service.SeedAsync();
        var adminCount2 = await context.Usuarios.CountAsync(u => u.NombreUsuario == "fredy");

        // Assert
        adminCount1.Should().Be(1);
        adminCount2.Should().Be(1); // No debe duplicarse
    }
}
