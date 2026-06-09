using SAA.Domain.Entities;
using SAA.Infrastructure.Data;

namespace SAA.Infrastructure.Services;

/// <summary>
/// Servicio para inicializar datos de prueba en la base de datos
/// </summary>
public class SeedDataService
{
    private readonly SAADbContext _context;

    public SeedDataService(SAADbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Siembra datos iniciales (usuario admin y postulantes de prueba)
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            // Aplicar migraciones pendientes
            await _context.Database.EnsureCreatedAsync();

            // 1. Crear usuario administrador si no existe
            if (!_context.Usuarios.Any(u => u.NombreUsuario == "fredy"))
            {
                var usuarioAdmin = new Usuario
                {
                    NombreUsuario = "fredy",
                    Contrasena = "123456", // En producción usar hash
                    NombreCompleto = "Fredy - Administrador",
                    Correo = "fredy@saa.edu.pe",
                    Rol = "Administrador",
                    Estado = "Activo",
                    FechaCreacion = DateTime.Now
                };

                _context.Usuarios.Add(usuarioAdmin);
                await _context.SaveChangesAsync();
            }

            // 2. Crear o asegurar programas académicos con Vacantes específicas
            if (!_context.ProgramasAcademicos.Any())
            {
                var programas = new List<ProgramaAcademico>
                {
                    new ProgramaAcademico { Nombre = "Ingeniería de Sistemas", Codigo = "IS01", Departamento = "Ingeniería", Estado = "Activo", Vacantes = 50 },
                    new ProgramaAcademico { Nombre = "Medicina Humana", Codigo = "MH01", Departamento = "Ciencias de la Salud", Estado = "Activo", Vacantes = 30 },
                    new ProgramaAcademico { Nombre = "Derecho", Codigo = "DE01", Departamento = "Derecho", Estado = "Activo", Vacantes = 40 }
                };
                _context.ProgramasAcademicos.AddRange(programas);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Si existen pero no tienen vacantes asignadas
                var programas = _context.ProgramasAcademicos.ToList();
                bool modified = false;
                foreach (var p in programas)
                {
                    if (!p.Vacantes.HasValue || p.Vacantes == 0)
                    {
                        if (p.Nombre.Contains("Ingeniería")) p.Vacantes = 50;
                        else if (p.Nombre.Contains("Medicina")) p.Vacantes = 30;
                        else if (p.Nombre.Contains("Derecho")) p.Vacantes = 40;
                        else p.Vacantes = 30;
                        modified = true;
                    }
                }
                if (modified) await _context.SaveChangesAsync();
            }

            var programasDisponibles = _context.ProgramasAcademicos.ToList();
            var random = new Random(12345); // Fixed seed for reproducibility

            // 3. Crear 500 postulantes de prueba si faltan
            int postulantesActuales = _context.Postulantes.Count();
            int postulantesAGenerar = 500 - postulantesActuales;

            if (postulantesAGenerar > 0 && programasDisponibles.Count > 0)
            {
                var postulantes = new List<Postulante>();
                var usuarios = new List<Usuario>();
                var fichas = new List<FichaPostulacion>();
                var examenes = new List<ExamenAdmision>();

                for (int i = 0; i < postulantesAGenerar; i++)
                {
                    string dniBase = i == 0 ? "12345678" : (10000000 + postulantesActuales + i).ToString();
                    var prog = programasDisponibles[random.Next(programasDisponibles.Count)];
                    
                    var p = new Postulante
                    {
                        Nombres = i == 0 ? "Prueba" : $"Postulante{i}",
                        Apellidos = i == 0 ? "Test" : $"Generado{i}",
                        DNI = dniBase,
                        IdProgramaInteres = prog.IdProgramaAcademico,
                        Correo = $"postulante{i}@example.com",
                        Telefono = "999888777",
                        Direccion = "Calle Virtual",
                        FechaNacimiento = new DateTime(2000, 1, 1).AddDays(random.Next(1, 3650)),
                        Estado = "Activo",
                        FechaCreacion = DateTime.Now
                    };
                    postulantes.Add(p);

                    // Para el usuario
                    usuarios.Add(new Usuario
                    {
                        NombreUsuario = dniBase,
                        Contrasena = dniBase, // Contraseña igual al DNI
                        NombreCompleto = $"Postulante{i} Generado{i}",
                        Correo = p.Correo,
                        Rol = "Postulante",
                        Estado = "Activo",
                        FechaCreacion = DateTime.Now
                    });
                }

                _context.Postulantes.AddRange(postulantes);
                _context.Usuarios.AddRange(usuarios);
                await _context.SaveChangesAsync();

                // Ahora crear fichas y exámenes
                for (int i = 0; i < postulantes.Count; i++)
                {
                    var p = postulantes[i];
                    var f = new FichaPostulacion
                    {
                        IdPostulante = p.IdPostulante,
                        IdProgramaAcademico = p.IdProgramaInteres,
                        FechaPostulacion = DateTime.Now,
                        Estado = "Registrada",
                        NumeroTramite = $"T-{p.DNI}"
                    };
                    fichas.Add(f);
                }
                _context.FichasPostulacion.AddRange(fichas);
                await _context.SaveChangesAsync();

                for (int i = 0; i < fichas.Count; i++)
                {
                    var f = fichas[i];
                    // Score random para asegurar que algunos aprueben (>50) y otros no, con varianza.
                    decimal score = Math.Round((decimal)(random.NextDouble() * 100), 2);
                    
                    var e = new ExamenAdmision
                    {
                        IdFichaPostulacion = f.IdFichaPostulacion,
                        IdPostulante = f.IdPostulante,
                        NombreExamen = "Examen General",
                        FechaExamen = DateTime.Now,
                        Estado = "Realizado",
                        Puntaje = score,
                        FechaCreacion = DateTime.Now
                    };
                    examenes.Add(e);
                }
                _context.ExamenesAdmision.AddRange(examenes);
                await _context.SaveChangesAsync();
            }

            // Ensure specific user "Prueba" exists for testing
            if (!_context.Postulantes.Any(p => p.DNI == "12345678"))
            {
                var prog = _context.ProgramasAcademicos.FirstOrDefault() ?? new ProgramaAcademico { Nombre = "Ingeniería de Sistemas", Codigo = "IS01", Departamento = "Ing", Estado = "Activo", Vacantes = 50 };
                if (prog.IdProgramaAcademico == 0)
                {
                    _context.ProgramasAcademicos.Add(prog);
                    await _context.SaveChangesAsync();
                }

                var p = new Postulante
                {
                    Nombres = "Prueba",
                    Apellidos = "Test",
                    DNI = "12345678",
                    IdProgramaInteres = prog.IdProgramaAcademico,
                    Correo = "prueba@example.com",
                    Telefono = "999888777",
                    Direccion = "Calle Virtual",
                    FechaNacimiento = new DateTime(2000, 1, 1),
                    Estado = "Activo",
                    FechaCreacion = DateTime.Now
                };
                _context.Postulantes.Add(p);
                await _context.SaveChangesAsync();

                var f = new FichaPostulacion
                {
                    IdPostulante = p.IdPostulante,
                    IdProgramaAcademico = p.IdProgramaInteres,
                    FechaPostulacion = DateTime.Now,
                    Estado = "Registrada",
                    NumeroTramite = $"T-{p.DNI}"
                };
                _context.FichasPostulacion.Add(f);
                await _context.SaveChangesAsync();

                var e = new ExamenAdmision
                {
                    IdFichaPostulacion = f.IdFichaPostulacion,
                    IdPostulante = p.IdPostulante,
                    NombreExamen = "Examen General",
                    FechaExamen = DateTime.Now,
                    Estado = "Realizado",
                    Puntaje = 85.5m, // Good score
                    FechaCreacion = DateTime.Now
                };
                _context.ExamenesAdmision.Add(e);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al sembrar datos: {ex.Message}");
            throw;
        }
    }
}
