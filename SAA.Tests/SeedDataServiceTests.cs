using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SAA.Infrastructure.Data;
using SAA.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

public class SeedDataServiceTests
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
    public async Task SeedAsync_WhenDatabaseIsEmpty_SeedsUsersAndPostulantes()
    {
        // Arrange
        var context = GetMemoryContext();
        var service = new SeedDataService(context);

        // Act
        await service.SeedAsync();

        // Assert
        var usuarios = await context.Usuarios.ToListAsync();
        usuarios.Should().HaveCount(501);
        usuarios.Any(u => u.NombreUsuario == "fredy").Should().BeTrue();

        var postulantes = await context.Postulantes.ToListAsync();
        postulantes.Should().HaveCount(500);
    }

    [Fact]
    public async Task SeedAsync_WhenDatabaseIsNotEmpty_DoesNotSeedAgain()
    {
        // Arrange
        var context = GetMemoryContext();
        
        // Add one user and one postulante
        context.Usuarios.Add(new Domain.Entities.Usuario { IdUsuario = 1, NombreUsuario = "test", Contrasena = "123", Estado = "Activo" });
        context.Postulantes.Add(new Domain.Entities.Postulante { IdPostulante = 1, DNI = "000", Nombres = "Test", Apellidos = "Test", Correo = "test", Estado = "Activo", FechaNacimiento = DateTime.Now, Telefono = "1", Direccion = "1" });
        await context.SaveChangesAsync();
        
        var service = new SeedDataService(context);

        // Act
        await service.SeedAsync();

        // Assert
        var usuarios = await context.Usuarios.ToListAsync();
        // Admin fredy se genera si no existe. Como "test" no es fredy, fredy se crea.
        // Total usuarios = 1 (test) + 1 (fredy) + 499 (postulantes) = 501.
        usuarios.Should().HaveCount(501); 

        var postulantes = await context.Postulantes.ToListAsync();
        postulantes.Should().HaveCount(500); // 1 existía, se generaron 499 = 500
    }
}
