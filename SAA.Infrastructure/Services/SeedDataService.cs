using SAA.Domain.Entities;
using SAA.Infrastructure.Data;

namespace SAA.Infrastructure.Services;

/// <summary>
/// Servicio para inicializar datos de prueba en la base de datos.
/// Siembra 100 preguntas del examen y respuestas por postulante.
/// </summary>
public class SeedDataService
{
    private readonly SAADbContext _context;

    public SeedDataService(SAADbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();

            // 1. Admin
            if (!_context.Usuarios.Any(u => u.NombreUsuario == "fredy"))
            {
                _context.Usuarios.Add(new Usuario
                {
                    NombreUsuario = "fredy",
                    Contrasena = "123456",
                    NombreCompleto = "Fredy - Administrador",
                    Correo = "fredy@saa.edu.pe",
                    Rol = "Administrador",
                    Estado = "Activo",
                    FechaCreacion = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }

            // 2. Programas académicos
            if (!_context.ProgramasAcademicos.Any())
            {
                _context.ProgramasAcademicos.AddRange(
                    new ProgramaAcademico { Nombre = "Ingeniería de Sistemas", Codigo = "IS01", Departamento = "Ingeniería", Estado = "Activo", Vacantes = 50 },
                    new ProgramaAcademico { Nombre = "Medicina Humana", Codigo = "MH01", Departamento = "Ciencias de la Salud", Estado = "Activo", Vacantes = 30 },
                    new ProgramaAcademico { Nombre = "Derecho", Codigo = "DE01", Departamento = "Derecho", Estado = "Activo", Vacantes = 40 }
                );
                await _context.SaveChangesAsync();
            }

            // 3. 100 preguntas del examen
            if (!_context.PreguntasExamen.Any())
            {
                var preguntas = GenerarPreguntas();
                _context.PreguntasExamen.AddRange(preguntas);
                await _context.SaveChangesAsync();
            }

            var programasDisponibles = _context.ProgramasAcademicos.ToList();
            var preguntasExamen = _context.PreguntasExamen.OrderBy(p => p.NumeroPregunta).ToList();
            var random = new Random(12345);

            // 4. Postulantes
            int postulantesActuales = _context.Postulantes.Count();
            int postulantesAGenerar = 500 - postulantesActuales;

            if (postulantesAGenerar > 0 && programasDisponibles.Count > 0)
            {
                var postulantes = new List<Postulante>();
                var usuarios = new List<Usuario>();

                for (int i = 0; i < postulantesAGenerar; i++)
                {
                    string dniBase = i == 0 ? "12345678" : (10000000 + postulantesActuales + i).ToString();
                    var prog = programasDisponibles[random.Next(programasDisponibles.Count)];

                    var p = new Postulante
                    {
                        Nombres = i == 0 ? "Prueba" : $"Postulante{postulantesActuales + i}",
                        Apellidos = i == 0 ? "Test" : $"Generado{postulantesActuales + i}",
                        DNI = dniBase,
                        IdProgramaInteres = prog.IdProgramaAcademico,
                        Correo = $"postulante{postulantesActuales + i}@example.com",
                        Telefono = "999888777",
                        Direccion = "Calle Virtual",
                        FechaNacimiento = new DateTime(2000, 1, 1).AddDays(random.Next(1, 3650)),
                        Estado = "Activo",
                        FechaCreacion = DateTime.Now
                    };
                    postulantes.Add(p);

                    usuarios.Add(new Usuario
                    {
                        NombreUsuario = dniBase,
                        Contrasena = dniBase,
                        NombreCompleto = $"{p.Nombres} {p.Apellidos}",
                        Correo = p.Correo,
                        Rol = "Postulante",
                        Estado = "Activo",
                        FechaCreacion = DateTime.Now
                    });
                }

                _context.Postulantes.AddRange(postulantes);
                _context.Usuarios.AddRange(usuarios);
                await _context.SaveChangesAsync();

                // Fichas y exámenes
                var fichas = new List<FichaPostulacion>();
                for (int i = 0; i < postulantes.Count; i++)
                {
                    fichas.Add(new FichaPostulacion
                    {
                        IdPostulante = postulantes[i].IdPostulante,
                        IdProgramaAcademico = postulantes[i].IdProgramaInteres,
                        FechaPostulacion = DateTime.Now,
                        Estado = "Registrada",
                        NumeroTramite = $"T-{postulantes[i].DNI}"
                    });
                }
                _context.FichasPostulacion.AddRange(fichas);
                await _context.SaveChangesAsync();

                // Exámenes y respuestas por pregunta
                var examenes = new List<ExamenAdmision>();
                for (int i = 0; i < fichas.Count; i++)
                {
                    var respuestasPostulante = GenerarRespuestasAleatorias(preguntasExamen, random);
                    int correctas = respuestasPostulante.Count(r => r.EsCorrecta);
                    decimal puntaje = Math.Round((decimal)correctas, 2); // puntaje = correctas sobre 100

                    var examen = new ExamenAdmision
                    {
                        IdFichaPostulacion = fichas[i].IdFichaPostulacion,
                        IdPostulante = fichas[i].IdPostulante,
                        NombreExamen = "Examen General de Admisión",
                        FechaExamen = DateTime.Now,
                        Estado = "Realizado",
                        Puntaje = puntaje,
                        FechaCreacion = DateTime.Now
                    };
                    examenes.Add(examen);
                }
                _context.ExamenesAdmision.AddRange(examenes);
                await _context.SaveChangesAsync();

                // Guardar respuestas individuales
                var todasRespuestas = new List<RespuestaPostulante>();
                for (int i = 0; i < examenes.Count; i++)
                {
                    var respuestasPost = GenerarRespuestasAleatorias(preguntasExamen, new Random(examenes[i].IdPostulante + 1));
                    foreach (var r in respuestasPost)
                    {
                        r.IdExamen = examenes[i].IdExamen;
                        r.IdPostulante = examenes[i].IdPostulante;
                    }
                    todasRespuestas.AddRange(respuestasPost);

                    // Guardar en lotes para no saturar memoria
                    if (todasRespuestas.Count >= 5000)
                    {
                        _context.RespuestasPostulante.AddRange(todasRespuestas);
                        await _context.SaveChangesAsync();
                        todasRespuestas.Clear();
                    }
                }
                if (todasRespuestas.Any())
                {
                    _context.RespuestasPostulante.AddRange(todasRespuestas);
                    await _context.SaveChangesAsync();
                }
            }

            // Asegurar postulante de prueba "12345678"
            if (!_context.Postulantes.Any(p => p.DNI == "12345678"))
            {
                var prog = _context.ProgramasAcademicos.First();
                var p = new Postulante
                {
                    Nombres = "Prueba", Apellidos = "Test", DNI = "12345678",
                    IdProgramaInteres = prog.IdProgramaAcademico,
                    Correo = "prueba@example.com", Telefono = "999888777",
                    Direccion = "Calle Virtual", FechaNacimiento = new DateTime(2000, 1, 1),
                    Estado = "Activo", FechaCreacion = DateTime.Now
                };
                _context.Postulantes.Add(p);
                await _context.SaveChangesAsync();

                var f = new FichaPostulacion { IdPostulante = p.IdPostulante, IdProgramaAcademico = p.IdProgramaInteres, FechaPostulacion = DateTime.Now, Estado = "Registrada", NumeroTramite = $"T-{p.DNI}" };
                _context.FichasPostulacion.Add(f);
                await _context.SaveChangesAsync();

                var respPrueba = GenerarRespuestasAleatorias(preguntasExamen, new Random(42));
                int correctasPrueba = respPrueba.Count(r => r.EsCorrecta);
                var examen = new ExamenAdmision { IdFichaPostulacion = f.IdFichaPostulacion, IdPostulante = p.IdPostulante, NombreExamen = "Examen General de Admisión", FechaExamen = DateTime.Now, Estado = "Realizado", Puntaje = correctasPrueba, FechaCreacion = DateTime.Now };
                _context.ExamenesAdmision.Add(examen);
                await _context.SaveChangesAsync();

                foreach (var r in respPrueba) { r.IdExamen = examen.IdExamen; r.IdPostulante = p.IdPostulante; }
                _context.RespuestasPostulante.AddRange(respPrueba);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al sembrar datos: {ex.Message}");
            throw;
        }
    }

    private static List<PreguntaExamen> GenerarPreguntas()
    {
        var areas = new[] { "Matemática", "Verbal", "Ciencias" };
        var opciones = new[] { "A", "B", "C", "D" };
        var rnd = new Random(999);
        var preguntas = new List<PreguntaExamen>();

        // Preguntas reales por área
        var banco = new (string area, string enunciado, string a, string b, string c, string d, string correcta)[]
        {
            ("Matemática","¿Cuánto es 15 × 8?","110","120","125","130","B"),
            ("Matemática","¿Raíz cuadrada de 144?","10","11","12","13","C"),
            ("Matemática","Si x + 5 = 12, ¿x = ?","5","6","7","8","C"),
            ("Matemática","¿Cuánto es 2³?","6","8","9","12","B"),
            ("Matemática","¿Cuánto es 25% de 200?","40","50","60","70","B"),
            ("Matemática","¿Cuál es el MCM de 4 y 6?","8","10","12","14","C"),
            ("Matemática","¿Área de un cuadrado de lado 5?","20","25","30","35","B"),
            ("Matemática","¿Cuánto es 3/4 + 1/4?","0.5","0.75","1","1.25","C"),
            ("Matemática","Si y = 2x y x = 3, ¿y = ?","4","5","6","7","C"),
            ("Matemática","¿Cuántos grados tiene un triángulo?","90","180","270","360","B"),
            ("Matemática","¿Cuánto es 1000 ÷ 25?","35","40","45","50","B"),
            ("Matemática","¿Perímetro de un rectángulo 4×6?","20","22","24","26","A"),
            ("Matemática","¿Cuánto es 7² - 5²?","14","20","24","30","C"),
            ("Matemática","¿Cuántos mm hay en 1 m?","10","100","1000","10000","C"),
            ("Matemática","¿El número π es aproximadamente?","3.12","3.14","3.16","3.18","B"),
            ("Matemática","¿Cuánto es log₁₀(100)?","1","2","10","20","B"),
            ("Matemática","¿Qué es un número primo?","Divisible por 2","Divisible solo por 1 y él mismo","Divisible por cualquier número","Divisible por 4","B"),
            ("Matemática","¿Cuánto es 0.5 × 0.5?","0.1","0.25","0.5","1","B"),
            ("Matemática","¿Cuánto es 12!/10!?","110","120","132","144","C"),
            ("Matemática","¿Hipotenusa de un triángulo 3-4-?","4","5","6","7","B"),
            ("Matemática","¿Cuánto es sen(90°)?","0","0.5","1","√2","C"),
            ("Matemática","¿Cuánto es cos(0°)?","0","0.5","1","−1","C"),
            ("Matemática","¿Cuánto es 2^10?","512","1024","2048","4096","B"),
            ("Matemática","¿Qué fracción es 0.75?","1/2","2/3","3/4","4/5","C"),
            ("Matemática","¿Cuánto es (a+b)² con a=2, b=3?","20","22","25","30","C"),
            ("Verbal","¿Sinónimo de 'feliz'?","Triste","Contento","Furioso","Aburrido","B"),
            ("Verbal","¿Antónimo de 'rápido'?","Ágil","Veloz","Lento","Fugaz","C"),
            ("Verbal","¿Cuál es el plural de 'luz'?","Luzs","Luces","Luses","Lucios","B"),
            ("Verbal","¿Qué figura retórica usa comparación?","Hipérbole","Metáfora","Símil","Antítesis","C"),
            ("Verbal","¿Qué tipo de oración es '¿Dónde estás?'","Declarativa","Imperativa","Interrogativa","Exclamativa","C"),
            ("Verbal","¿Qué es un sustantivo?","Acción","Cualidad","Persona o cosa","Enlace","C"),
            ("Verbal","¿Qué tipo de narrador ve todo?","Testigo","Protagonista","Omnisciente","Externo","C"),
            ("Verbal","¿Qué es un adverbio?","Modifica un verbo","Es una persona","Es un enlace","Es una acción","A"),
            ("Verbal","¿Qué es la hipérbole?","Comparación directa","Exageración","Contradicción","Personificación","B"),
            ("Verbal","¿Qué significa 'eufemismo'?","Insulto directo","Expresión suavizada","Comparación","Contradicción","B"),
            ("Verbal","¿Cuál es el género de 'mapa'?","Femenino","Masculino","Neutro","Indefinido","B"),
            ("Verbal","¿Qué es un verbo copulativo?","Une sujeto con predicado","Expresa acción","Modifica al sustantivo","Enlaza oraciones","A"),
            ("Verbal","¿Cuál es el sujeto en 'Los niños juegan'?","juegan","Los","Los niños","niños","C"),
            ("Verbal","¿Qué es el prefijo 'pre'?","Después","Antes","Durante","Sin","B"),
            ("Verbal","¿Sinónimo de 'arduo'?","Fácil","Difícil","Rápido","Sencillo","B"),
            ("Verbal","¿Qué es la aliteración?","Repetición de sonidos","Comparación","Exageración","Ironía","A"),
            ("Verbal","¿Cuál es el tiempo del verbo 'comió'?","Presente","Futuro","Pretérito","Condicional","C"),
            ("Verbal","¿Qué es la metonimia?","Nombrar algo por relación","Comparación directa","Contradicción","Personificación","A"),
            ("Verbal","¿Cuál es el antónimo de 'generoso'?","Amable","Avaro","Bondadoso","Honesto","B"),
            ("Verbal","¿Qué es el pleonasmo?","Redundancia innecesaria","Metáfora extendida","Ironía fina","Aliteración vocal","A"),
            ("Verbal","¿Qué son las conjunciones?","Verbos de acción","Palabras de enlace","Sustantivos abstractos","Adjetivos calificativos","B"),
            ("Verbal","¿Cuál es el diminutivo de 'libro'?","Librito","Librote","Librón","Librillo","A"),
            ("Verbal","¿Sinónimo de 'acérrimo'?","Moderado","Indiferente","Ferviente","Pasivo","C"),
            ("Verbal","¿Cuántos sílabas tiene 'mariposa'?","3","4","5","6","B"),
            ("Verbal","¿La palabra 'bicefálico' significa?","De dos cabezas","De una cabeza","Sin cabeza","Cabeza grande","A"),
            ("Ciencias","¿Fórmula química del agua?","H2O","HO2","H2O2","H3O","A"),
            ("Ciencias","¿Qué planeta es el más grande del sistema solar?","Saturno","Júpiter","Urano","Neptuno","B"),
            ("Ciencias","¿Cuántos huesos tiene el cuerpo humano adulto?","196","206","216","226","B"),
            ("Ciencias","¿Qué organelo produce energía en la célula?","Núcleo","Ribosoma","Mitocondria","Lisosoma","C"),
            ("Ciencias","¿Qué gas respiramos principalmente?","CO2","O2","N2","H2","C"),
            ("Ciencias","¿Qué ley describe F=ma?","Gravitación Universal","Segunda ley de Newton","Primera ley de Newton","Ley de Ohm","B"),
            ("Ciencias","¿Qué tipo de tejido recubre el cuerpo?","Muscular","Óseo","Epitelial","Nervioso","C"),
            ("Ciencias","¿Velocidad de la luz aproximada?","200.000 km/s","300.000 km/s","400.000 km/s","500.000 km/s","B"),
            ("Ciencias","¿Qué es la fotosíntesis?","Respiración animal","Producción de alimento en plantas","Digestión celular","Reproducción","B"),
            ("Ciencias","¿Número atómico del carbono?","4","6","8","12","B"),
            ("Ciencias","¿Qué es el ADN?","Proteína de transporte","Ácido nucleico con información genética","Lípido celular","Hidrato de carbono","B"),
            ("Ciencias","¿Cuál es el pH del agua pura?","5","7","8","9","B"),
            ("Ciencias","¿Qué es la homeostasis?","Movimiento celular","Equilibrio interno del organismo","División celular","Reproducción sexual","B"),
            ("Ciencias","¿Qué partícula tiene carga negativa?","Protón","Neutrón","Electrón","Fotón","C"),
            ("Ciencias","¿Qué planeta es conocido como el planeta rojo?","Venus","Marte","Mercurio","Saturno","B"),
            ("Ciencias","¿Cuántos cromosomas tiene una célula humana normal?","23","46","48","92","B"),
            ("Ciencias","¿Qué es la osmosis?","Difusión de solvente a través de membrana","Transporte activo","Endocitosis","Exocitosis","A"),
            ("Ciencias","¿Fórmula del CO2?","Dióxido de carbono","Monóxido de carbono","Óxido de carbono","Carbonato","A"),
            ("Ciencias","¿Cuál es el organelo de la síntesis proteica?","Mitocondria","Ribosoma","Núcleo","Vacuola","B"),
            ("Ciencias","¿Qué es la inercia?","Velocidad constante","Resistencia al cambio de estado de movimiento","Fuerza gravitatoria","Energía cinética","B"),
            ("Ciencias","¿De qué está compuesta la atmósfera principalmente?","Oxígeno y CO2","Nitrógeno y Oxígeno","Hidrógeno y Oxígeno","Argón y CO2","B"),
            ("Ciencias","¿Qué es la selección natural?","Mutación genética","Supervivencia de los más adaptados","Reproducción sexual","Migración de especies","B"),
            ("Ciencias","¿Qué es un ecosistema?","Solo animales","Solo plantas","Comunidad biológica y su ambiente","Solo el suelo","C"),
            ("Ciencias","¿Qué es la gravedad?","Fuerza magnética","Fuerza de atracción entre masas","Fuerza eléctrica","Fuerza nuclear","B"),
            ("Ciencias","¿Qué elemento es el más abundante en la Tierra?","Silicio","Hierro","Oxígeno","Aluminio","C"),
        };

        int idx = 0;
        for (int i = 1; i <= 100; i++)
        {
            var (area, enunciado, a, b, c, d, correcta) = banco[idx % banco.Length];
            idx++;
            preguntas.Add(new PreguntaExamen
            {
                NumeroPregunta = i,
                Area = area,
                Enunciado = $"P{i}: {enunciado}",
                OpcionA = a,
                OpcionB = b,
                OpcionC = c,
                OpcionD = d,
                RespuestaCorrecta = correcta
            });
        }
        return preguntas;
    }

    private static List<RespuestaPostulante> GenerarRespuestasAleatorias(
        List<PreguntaExamen> preguntas, Random rnd)
    {
        var opciones = new[] { "A", "B", "C", "D" };
        var respuestas = new List<RespuestaPostulante>();

        foreach (var preg in preguntas)
        {
            // 55% de probabilidad de acertar (para que puntaje promedio sea ~55)
            string resp = rnd.NextDouble() < 0.55
                ? preg.RespuestaCorrecta
                : opciones[rnd.Next(4)];

            respuestas.Add(new RespuestaPostulante
            {
                IdPregunta = preg.IdPregunta,
                RespuestaSeleccionada = resp,
                EsCorrecta = resp == preg.RespuestaCorrecta
            });
        }
        return respuestas;
    }
}
