using Microsoft.EntityFrameworkCore;
using SAA.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

public class InfrastructureTests
{
    [Fact]
    public async Task DbContext_ModelCreation_Success()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SAADbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new SAADbContext(options);
        
        // This forces OnModelCreating to execute and EF to configure the model
        await context.Database.EnsureCreatedAsync();

        // Assert
        var model = context.Model;
        
        Assert.NotNull(model.FindEntityType(typeof(Domain.Entities.Usuario)));
        Assert.NotNull(model.FindEntityType(typeof(Domain.Entities.ProgramaAcademico)));
        Assert.NotNull(model.FindEntityType(typeof(Domain.Entities.Postulante)));
        Assert.NotNull(model.FindEntityType(typeof(Domain.Entities.FichaPostulacion)));
        Assert.NotNull(model.FindEntityType(typeof(Domain.Entities.ExamenAdmision)));
        Assert.NotNull(model.FindEntityType(typeof(Domain.Entities.ResultadoAdmision)));
    }
}
