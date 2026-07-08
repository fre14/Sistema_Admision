using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
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

public class AuthControllerTests
{
    private SAADbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<SAADbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new SAADbContext(options);
    }

    private IConfiguration GetConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "SuperSecretKeyForTestingTheAuthService123!"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task Login_Valido_RetornaOkConToken()
    {
        // Arrange
        var context = GetMemoryContext();
        var config = GetConfiguration();
        var service = new AuthService(context, config);
        var controller = new AuthController(service);

        context.Usuarios.Add(new Usuario
        {
            NombreUsuario = "admin",
            Contrasena = "123456",
            Rol = "Administrador",
            Estado = "Activo"
        });
        await context.SaveChangesAsync();

        var request = new LoginRequestDto { NombreUsuario = "admin", Contrasena = "123456" };

        // Act
        var result = await controller.Login(request);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var response = okResult.Value as LoginResponseDto;
        response.Should().NotBeNull();
        response!.Token.Should().NotBeEmpty();
        response.Exito.Should().BeTrue();
    }

    [Fact]
    public async Task Login_Invalido_RetornaUnauthorized()
    {
        // Arrange
        var context = GetMemoryContext();
        var config = GetConfiguration();
        var service = new AuthService(context, config);
        var controller = new AuthController(service);

        var request = new LoginRequestDto { NombreUsuario = "noexiste", Contrasena = "bad" };

        // Act
        var result = await controller.Login(request);

        // Assert
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task Login_ModelStateInvalido_RetornaBadRequest()
    {
        // Arrange
        var context = GetMemoryContext();
        var config = GetConfiguration();
        var service = new AuthService(context, config);
        var controller = new AuthController(service);
        controller.ModelState.AddModelError("Error", "Model is invalid");

        var request = new LoginRequestDto();

        // Act
        var result = await controller.Login(request);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }
}
