using FluentAssertions;
using SAA.Domain.Entities;
using System;
using Xunit;

namespace SAA.Tests;

/// <summary>
/// Pruebas exhaustivas para todas las entidades del dominio SAA.
/// Verifica instanciación, valores por defecto y manipulación de propiedades.
/// </summary>
public class DomainEntityTests
{
    // ==================== POSTULANTE ====================

    [Fact]
    public void Postulante_CreaInstancia_ValoresPorDefecto()
    {
        // Arrange & Act
        var postulante = new Postulante();

        // Assert
        postulante.IdPostulante.Should().Be(0);
        postulante.Nombres.Should().Be(string.Empty);
        postulante.Apellidos.Should().Be(string.Empty);
        postulante.DNI.Should().Be(string.Empty);
        postulante.Correo.Should().Be(string.Empty);
        postulante.Estado.Should().Be("Activo");
        postulante.Telefono.Should().BeNull();
        postulante.Direccion.Should().BeNull();
        postulante.FechaNacimiento.Should().BeNull();
        postulante.FechaActualizacion.Should().BeNull();
    }

    [Fact]
    public void Postulante_AsignarPropiedades_DevuelveValoresCorrectos()
    {
        // Arrange
        var fecha = new DateTime(2000, 5, 15);
        
        // Act
        var postulante = new Postulante
        {
            IdPostulante = 1,
            Nombres = "Juan Carlos",
            Apellidos = "Pérez López",
            DNI = "12345678",
            IdProgramaInteres = 5,
            Correo = "juan@test.com",
            Telefono = "999888777",
            Direccion = "Av. Principal 123",
            FechaNacimiento = fecha,
            Estado = "Inactivo",
            FechaActualizacion = DateTime.Now
        };

        // Assert
        postulante.IdPostulante.Should().Be(1);
        postulante.Nombres.Should().Be("Juan Carlos");
        postulante.Apellidos.Should().Be("Pérez López");
        postulante.DNI.Should().Be("12345678");
        postulante.IdProgramaInteres.Should().Be(5);
        postulante.Correo.Should().Be("juan@test.com");
        postulante.Telefono.Should().Be("999888777");
        postulante.Direccion.Should().Be("Av. Principal 123");
        postulante.FechaNacimiento.Should().Be(fecha);
        postulante.Estado.Should().Be("Inactivo");
        postulante.FechaActualizacion.Should().NotBeNull();
    }

    // ==================== EXAMEN ADMISION ====================

    [Fact]
    public void ExamenAdmision_CreaInstancia_ValoresPorDefecto()
    {
        // Arrange & Act
        var examen = new ExamenAdmision();

        // Assert
        examen.IdExamen.Should().Be(0);
        examen.Puntaje.Should().BeNull();
        examen.HoraInicio.Should().BeNull();
        examen.DuracionMinutos.Should().BeNull();
        examen.Sala.Should().BeNull();
        examen.Observaciones.Should().BeNull();
        examen.FechaActualizacion.Should().BeNull();
    }

    [Fact]
    public void ExamenAdmision_AsignarPuntaje_DevuelveValorCorrecto()
    {
        // Arrange & Act
        var examen = new ExamenAdmision
        {
            IdExamen = 1,
            IdPostulante = 10,
            IdFichaPostulacion = 5,
            NombreExamen = "Examen General de Admisión",
            FechaExamen = DateTime.Now,
            Puntaje = 85.50m,
            Estado = "Realizado",
            Sala = "Aula 101",
            DuracionMinutos = 120,
            HoraInicio = new TimeSpan(9, 0, 0),
            Observaciones = "Sin observaciones"
        };

        // Assert
        examen.Puntaje.Should().Be(85.50m);
        examen.NombreExamen.Should().Contain("General");
        examen.DuracionMinutos.Should().Be(120);
        examen.Sala.Should().Be("Aula 101");
        examen.HoraInicio.Should().Be(new TimeSpan(9, 0, 0));
    }

    // ==================== RESULTADO ADMISION ====================

    [Fact]
    public void ResultadoAdmision_CreaInstancia_ValoresPorDefecto()
    {
        // Arrange & Act
        var resultado = new ResultadoAdmision();

        // Assert
        resultado.IdResultado.Should().Be(0);
        resultado.Resultado.Should().Be(string.Empty);
        resultado.Calificacion.Should().BeNull();
        resultado.OrdenMerito.Should().BeNull();
        resultado.Observaciones.Should().BeNull();
        resultado.IdUsuarioEvaluador.Should().BeNull();
        resultado.FechaActualizacion.Should().BeNull();
    }

    [Fact]
    public void ResultadoAdmision_Ingresante_PropiedadesCorrectas()
    {
        // Arrange & Act
        var resultado = new ResultadoAdmision
        {
            IdResultado = 1,
            IdPostulante = 10,
            IdProgramaAcademico = 3,
            Calificacion = 92.5m,
            Resultado = "Ingresante",
            OrdenMerito = 1,
            FechaResultado = DateTime.Now,
            IdUsuarioEvaluador = 1
        };

        // Assert
        resultado.Resultado.Should().Be("Ingresante");
        resultado.Calificacion.Should().Be(92.5m);
        resultado.OrdenMerito.Should().Be(1);
        resultado.IdUsuarioEvaluador.Should().Be(1);
    }

    [Fact]
    public void ResultadoAdmision_Desaprobado_PropiedadesCorrectas()
    {
        // Arrange & Act
        var resultado = new ResultadoAdmision
        {
            Resultado = "Desaprobado",
            Calificacion = 35.0m,
            OrdenMerito = 15,
            Observaciones = "No alcanzó puntaje mínimo"
        };

        // Assert
        resultado.Resultado.Should().Be("Desaprobado");
        resultado.Calificacion.Should().BeLessThan(50m);
        resultado.Observaciones.Should().Contain("puntaje mínimo");
    }

    // ==================== PROGRAMA ACADEMICO ====================

    [Fact]
    public void ProgramaAcademico_CreaInstancia_ConVacantes()
    {
        // Arrange & Act
        var programa = new ProgramaAcademico
        {
            IdProgramaAcademico = 1,
            Codigo = "IS01",
            Nombre = "Ingeniería de Sistemas",
            Descripcion = "Programa de ingeniería enfocado en sistemas de información",
            NivelAcademico = "Pregrado",
            Vacantes = 50,
            Estado = "Activo",
            Departamento = "Ingeniería"
        };

        // Assert
        programa.Nombre.Should().Be("Ingeniería de Sistemas");
        programa.Vacantes.Should().Be(50);
        programa.Codigo.Should().Be("IS01");
        programa.NivelAcademico.Should().Be("Pregrado");
    }

    [Fact]
    public void ProgramaAcademico_SinVacantes_EsNull()
    {
        // Arrange & Act
        var programa = new ProgramaAcademico();

        // Assert
        programa.Vacantes.Should().BeNull();
        programa.Descripcion.Should().BeNull();
        programa.NivelAcademico.Should().BeNull();
        programa.Departamento.Should().BeNull();
        programa.FechaInicioProceso.Should().BeNull();
        programa.FechaFinalProceso.Should().BeNull();
    }

    // ==================== USUARIO ====================

    [Fact]
    public void Usuario_CreaInstancia_ValoresPorDefecto()
    {
        // Arrange & Act
        var usuario = new Usuario();

        // Assert
        usuario.IdUsuario.Should().Be(0);
        usuario.UltimoAcceso.Should().BeNull();
        usuario.FechaActualizacion.Should().BeNull();
    }

    [Fact]
    public void Usuario_Administrador_PropiedadesCorrectas()
    {
        // Arrange & Act
        var usuario = new Usuario
        {
            IdUsuario = 1,
            NombreUsuario = "admin",
            Contrasena = "hash_seguro",
            NombreCompleto = "Administrador del Sistema",
            Correo = "admin@saa.edu.pe",
            Rol = "Administrador",
            Estado = "Activo",
            FechaCreacion = DateTime.Now,
            UltimoAcceso = DateTime.Now
        };

        // Assert
        usuario.Rol.Should().Be("Administrador");
        usuario.Estado.Should().Be("Activo");
        usuario.UltimoAcceso.Should().NotBeNull();
    }

    [Fact]
    public void Usuario_Postulante_RolCorrecto()
    {
        // Arrange & Act
        var usuario = new Usuario
        {
            NombreUsuario = "12345678",
            Rol = "Postulante",
            Estado = "Activo"
        };

        // Assert
        usuario.Rol.Should().Be("Postulante");
        usuario.NombreUsuario.Should().HaveLength(8);
    }

    // ==================== FICHA POSTULACION ====================

    [Fact]
    public void FichaPostulacion_CreaInstancia_PropiedadesCorrectas()
    {
        // Arrange & Act
        var ficha = new FichaPostulacion
        {
            IdFichaPostulacion = 1,
            IdPostulante = 10,
            IdProgramaAcademico = 3,
            NumeroTramite = "T-12345678",
            FechaPostulacion = DateTime.Now,
            Estado = "Registrada"
        };

        // Assert
        ficha.NumeroTramite.Should().StartWith("T-");
        ficha.Estado.Should().Be("Registrada");
        ficha.IdPostulante.Should().Be(10);
        ficha.Observaciones.Should().BeNull();
    }

    // ==================== ROL ====================

    [Fact]
    public void Rol_CreaInstancia_PropiedadesCorrectas()
    {
        // Arrange & Act
        var rol = new Rol
        {
            IdRol = 1,
            Nombre = "Administrador",
            Descripcion = "Rol con acceso total al sistema"
        };

        // Assert
        rol.Nombre.Should().Be("Administrador");
        rol.Descripcion.Should().Contain("acceso total");
    }

    // ==================== PERIODO ADMISION ====================

    [Fact]
    public void PeriodoAdmision_CreaInstancia_FechasCorrectas()
    {
        // Arrange
        var inicio = new DateTime(2026, 3, 1);
        var fin = new DateTime(2026, 6, 30);

        // Act
        var periodo = new PeriodoAdmision
        {
            IdPeriodo = 1,
            Nombre = "Admisión 2026-I",
            FechaInicio = inicio,
            FechaFin = fin
        };

        // Assert
        periodo.Nombre.Should().Contain("2026");
        periodo.FechaInicio.Should().BeBefore(periodo.FechaFin);
    }

    // ==================== ENTIDADES ADICIONALES ====================

    [Fact]
    public void Matricula_CreaInstancia_PropiedadesAccesibles()
    {
        // Arrange & Act
        var matricula = new Matricula();
        var type = matricula.GetType();

        // Assert
        type.Name.Should().Be("Matricula");
        type.GetProperties().Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Notificacion_CreaInstancia_PropiedadesAccesibles()
    {
        // Arrange & Act
        var notificacion = new Notificacion();
        var type = notificacion.GetType();

        // Assert
        type.Name.Should().Be("Notificacion");
        type.GetProperties().Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void ConfiguracionSistema_CreaInstancia_PropiedadesAccesibles()
    {
        // Arrange & Act
        var config = new ConfiguracionSistema();
        var type = config.GetType();

        // Assert
        type.Name.Should().Be("ConfiguracionSistema");
        type.GetProperties().Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void DocumentoPostulante_CreaInstancia_PropiedadesAccesibles()
    {
        // Arrange & Act
        var doc = new DocumentoPostulante();
        var type = doc.GetType();

        // Assert
        type.Name.Should().Be("DocumentoPostulante");
        type.GetProperties().Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void LogAuditoria_CreaInstancia_PropiedadesAccesibles()
    {
        // Arrange & Act
        var log = new LogAuditoria();
        var type = log.GetType();

        // Assert
        type.Name.Should().Be("LogAuditoria");
        type.GetProperties().Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void TipoDocumento_CreaInstancia_PropiedadesAccesibles()
    {
        // Arrange & Act
        var tipo = new TipoDocumento();
        var typeInfo = tipo.GetType();

        // Assert
        typeInfo.Name.Should().Be("TipoDocumento");
        typeInfo.GetProperties().Length.Should().BeGreaterThan(0);
    }
}
