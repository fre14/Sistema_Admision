using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Moq;
using SAA.Application.DTOs;
using SAA.Application.Services;
using SAA.Domain.Entities;
using SAA.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SAA.Tests;

public class AuthServiceTests
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
    public async Task LoginAsync_ValidCredentials_ReturnsTokenAndSuccess()
    {
        // Arrange
        var context = GetMemoryContext();
        
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["Jwt:Key"]).Returns("SuperSecretKeyParaLasPruebasUnitarias256Bits_1234567890");
        mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("SAA.API");
        mockConfig.Setup(c => c["Jwt:Audience"]).Returns("SAA.Clients");

        var service = new AuthService(context, mockConfig.Object);

        context.Usuarios.Add(new Usuario
        {
            IdUsuario = 1,
            NombreUsuario = "fredy",
            Contrasena = "123456",
            Rol = "Administrador",
            Estado = "Activo"
        });
        await context.SaveChangesAsync();

        var request = new LoginRequestDto { NombreUsuario = "fredy", Contrasena = "123456" };

        // Act
        var result = await service.LoginAsync(request);

        // Assert
        result.Exito.Should().BeTrue();
        result.Token.Should().NotBeNullOrEmpty();
        result.Usuario.Should().NotBeNull();
        result.Usuario!.NombreUsuario.Should().Be("fredy");
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ReturnsFailure()
    {
        // Arrange
        var context = GetMemoryContext();
        var mockConfig = new Mock<IConfiguration>();
        var service = new AuthService(context, mockConfig.Object);

        context.Usuarios.Add(new Usuario
        {
            IdUsuario = 1,
            NombreUsuario = "fredy",
            Contrasena = "123456",
            Estado = "Activo"
        });
        await context.SaveChangesAsync();

        var request = new LoginRequestDto { NombreUsuario = "fredy", Contrasena = "wrongpass" };

        // Act
        var result = await service.LoginAsync(request);

        // Assert
        result.Exito.Should().BeFalse();
        result.Token.Should().BeNull();
        result.Mensaje.Should().Contain("incorrectos");
    }

    [Fact]
    public async Task LoginAsync_InactiveUser_ReturnsFailure()
    {
        // Arrange
        var context = GetMemoryContext();
        var mockConfig = new Mock<IConfiguration>();
        var service = new AuthService(context, mockConfig.Object);

        context.Usuarios.Add(new Usuario
        {
            IdUsuario = 1,
            NombreUsuario = "fredy",
            Contrasena = "123456",
            Estado = "Inactivo"
        });
        await context.SaveChangesAsync();

        var request = new LoginRequestDto { NombreUsuario = "fredy", Contrasena = "123456" };

        // Act
        var result = await service.LoginAsync(request);

        // Assert
        result.Exito.Should().BeFalse();
        result.Token.Should().BeNull();
        result.Mensaje.Should().Contain("inactivo");
    }
}
