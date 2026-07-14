using Microsoft.EntityFrameworkCore;
using SAA.Domain.Entities;
using SAA.Infrastructure.Data;

namespace SAA.Infrastructure.Services;

/// <summary>
/// Servicio de siembra de datos. Crea tablas nuevas si no existen (compatible con BD existente).
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

            // ── Crear tablas nuevas si la BD ya existía (EnsureCreated no las crea) ──
            await CrearTablasNuevasAsync();

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
                _context.PreguntasExamen.AddRange(GenerarPreguntas());
                await _context.SaveChangesAsync();
            }

            var programas = _context.ProgramasAcademicos.ToList();
            var preguntas = _context.PreguntasExamen.OrderBy(p => p.NumeroPregunta).ToList();
            var rnd = new Random(12345);

            // 4. Postulantes con nombres reales
            int actuales = _context.Postulantes.Count();
            int aGenerar = 500 - actuales;

            if (aGenerar > 0 && programas.Any())
            {
                var postulantes = new List<Postulante>();
                var usuarios = new List<Usuario>();

                for (int i = 0; i < aGenerar; i++)
                {
                    bool esPrimero = (i == 0 && actuales == 0);
                    string dni = esPrimero ? "12345678" : GenerarDni(actuales + i, rnd);
                    var prog = programas[rnd.Next(programas.Count)];
                    bool esMujer = rnd.NextDouble() < 0.48;
                    string nombre = esPrimero ? "Prueba" : ObtenerNombre(esMujer, rnd);
                    string apellidos = esPrimero ? "Test" : $"{ObtenerApellido(rnd)} {ObtenerApellido(rnd)}";

                    var p = new Postulante
                    {
                        Nombres = nombre,
                        Apellidos = apellidos,
                        DNI = dni,
                        IdProgramaInteres = prog.IdProgramaAcademico,
                        Correo = $"{nombre.ToLower().Replace(" ", ".")}.{apellidos.Split(' ')[0].ToLower()}@gmail.com",
                        Telefono = $"9{rnd.Next(10000000, 99999999)}",
                        Direccion = ObtenerDireccion(rnd),
                        FechaNacimiento = new DateTime(1998, 1, 1).AddDays(rnd.Next(0, 2920)),
                        Estado = "Activo",
                        FechaCreacion = DateTime.Now
                    };
                    postulantes.Add(p);
                    usuarios.Add(new Usuario
                    {
                        NombreUsuario = dni,
                        Contrasena = dni,
                        NombreCompleto = $"{nombre} {apellidos}",
                        Correo = p.Correo,
                        Rol = "Postulante",
                        Estado = "Activo",
                        FechaCreacion = DateTime.Now
                    });
                }

                _context.Postulantes.AddRange(postulantes);
                _context.Usuarios.AddRange(usuarios);
                await _context.SaveChangesAsync();

                // Fichas
                var fichas = postulantes.Select(p => new FichaPostulacion
                {
                    IdPostulante = p.IdPostulante,
                    IdProgramaAcademico = p.IdProgramaInteres,
                    FechaPostulacion = DateTime.Now.AddDays(-rnd.Next(1, 60)),
                    Estado = "Registrada",
                    NumeroTramite = $"T-{p.DNI}"
                }).ToList();
                _context.FichasPostulacion.AddRange(fichas);
                await _context.SaveChangesAsync();

                // Exámenes
                var examenes = new List<ExamenAdmision>();
                for (int i = 0; i < fichas.Count; i++)
                {
                    var respsPrev = GenerarRespuestas(preguntas, new Random(fichas[i].IdPostulante + 7));
                    int correctas = respsPrev.Count(r => r.EsCorrecta);
                    examenes.Add(new ExamenAdmision
                    {
                        IdFichaPostulacion = fichas[i].IdFichaPostulacion,
                        IdPostulante = fichas[i].IdPostulante,
                        NombreExamen = "Examen General de Admisión",
                        FechaExamen = DateTime.Now.AddDays(-rnd.Next(0, 30)),
                        Estado = "Realizado",
                        Puntaje = correctas,
                        FechaCreacion = DateTime.Now
                    });
                }
                _context.ExamenesAdmision.AddRange(examenes);
                await _context.SaveChangesAsync();

                // Respuestas por pregunta (en lotes)
                var todasResps = new List<RespuestaPostulante>();
                for (int i = 0; i < examenes.Count; i++)
                {
                    var resps = GenerarRespuestas(preguntas, new Random(examenes[i].IdPostulante + 7));
                    foreach (var r in resps) { r.IdExamen = examenes[i].IdExamen; r.IdPostulante = examenes[i].IdPostulante; }
                    todasResps.AddRange(resps);
                    if (todasResps.Count >= 5000)
                    {
                        _context.RespuestasPostulante.AddRange(todasResps);
                        await _context.SaveChangesAsync();
                        todasResps.Clear();
                    }
                }
                if (todasResps.Any())
                {
                    _context.RespuestasPostulante.AddRange(todasResps);
                    await _context.SaveChangesAsync();
                }
            }

            // Asegurar postulante "12345678" de prueba
            if (!_context.Postulantes.Any(p => p.DNI == "12345678"))
            {
                var prog = _context.ProgramasAcademicos.First();
                var p = new Postulante
                {
                    Nombres = "Carlos Alberto", Apellidos = "Quispe Mamani",
                    DNI = "12345678", IdProgramaInteres = prog.IdProgramaAcademico,
                    Correo = "carlos.quispe@gmail.com", Telefono = "987654321",
                    Direccion = "Jr. Ayacucho 245, Huamanga", Estado = "Activo",
                    FechaNacimiento = new DateTime(2001, 5, 14), FechaCreacion = DateTime.Now
                };
                _context.Postulantes.Add(p);

                if (!_context.Usuarios.Any(u => u.NombreUsuario == "12345678"))
                {
                    _context.Usuarios.Add(new Usuario
                    {
                        NombreUsuario = "12345678", Contrasena = "12345678",
                        NombreCompleto = "Carlos Alberto Quispe Mamani",
                        Correo = p.Correo, Rol = "Postulante", Estado = "Activo", FechaCreacion = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();

                var f = new FichaPostulacion
                {
                    IdPostulante = p.IdPostulante, IdProgramaAcademico = p.IdProgramaInteres,
                    FechaPostulacion = DateTime.Now, Estado = "Registrada", NumeroTramite = $"T-{p.DNI}"
                };
                _context.FichasPostulacion.Add(f);
                await _context.SaveChangesAsync();

                var resps = GenerarRespuestas(preguntas, new Random(42));
                int correctasPrueba = resps.Count(r => r.EsCorrecta);
                var examen = new ExamenAdmision
                {
                    IdFichaPostulacion = f.IdFichaPostulacion, IdPostulante = p.IdPostulante,
                    NombreExamen = "Examen General de Admisión", FechaExamen = DateTime.Now,
                    Estado = "Realizado", Puntaje = correctasPrueba, FechaCreacion = DateTime.Now
                };
                _context.ExamenesAdmision.Add(examen);
                await _context.SaveChangesAsync();

                foreach (var r in resps) { r.IdExamen = examen.IdExamen; r.IdPostulante = p.IdPostulante; }
                _context.RespuestasPostulante.AddRange(resps);
                await _context.SaveChangesAsync();
            }

            // Si postulante 12345678 existe pero no tiene respuestas, generarlas
            else if (!_context.RespuestasPostulante.Any())
            {
                await GenerarRespuestasParaExistentesAsync(preguntas);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Seed] Error: {ex.Message}");
            throw;
        }
    }

    // ─────────────────────────────────────────────────────────────
    // Crear tablas nuevas si no existen (para BD ya existente en Somee)
    // ─────────────────────────────────────────────────────────────
    private async Task CrearTablasNuevasAsync()
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_SCHEMA = 'Admision' AND TABLE_NAME = 'PreguntaExamen'
                )
                BEGIN
                    CREATE TABLE [Admision].[PreguntaExamen] (
                        [IdPregunta]       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [NumeroPregunta]   INT NOT NULL,
                        [Area]             NVARCHAR(100) NOT NULL,
                        [Enunciado]        NVARCHAR(500) NOT NULL,
                        [OpcionA]          NVARCHAR(300) NOT NULL,
                        [OpcionB]          NVARCHAR(300) NOT NULL,
                        [OpcionC]          NVARCHAR(300) NOT NULL,
                        [OpcionD]          NVARCHAR(300) NOT NULL,
                        [RespuestaCorrecta] NVARCHAR(1) NOT NULL
                    )
                END");

            await _context.Database.ExecuteSqlRawAsync(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_SCHEMA = 'Admision' AND TABLE_NAME = 'RespuestaPostulante'
                )
                BEGIN
                    CREATE TABLE [Admision].[RespuestaPostulante] (
                        [IdRespuesta]              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [IdExamen]                 INT NOT NULL,
                        [IdPostulante]             INT NOT NULL,
                        [IdPregunta]               INT NOT NULL,
                        [RespuestaSeleccionada]    NVARCHAR(1) NOT NULL,
                        [EsCorrecta]               BIT NOT NULL
                    )
                END");

            Console.WriteLine("[Seed] Tablas PreguntaExamen y RespuestaPostulante verificadas/creadas.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Seed] Aviso al crear tablas: {ex.Message}");
            // No lanzar — puede ser InMemory DB que no soporta raw SQL
        }
    }

    private async Task GenerarRespuestasParaExistentesAsync(List<PreguntaExamen> preguntas)
    {
        var examenes = _context.ExamenesAdmision.ToList();
        var lote = new List<RespuestaPostulante>();
        foreach (var ex in examenes)
        {
            var resps = GenerarRespuestas(preguntas, new Random(ex.IdPostulante + 7));
            int correctas = resps.Count(r => r.EsCorrecta);
            ex.Puntaje = correctas;
            foreach (var r in resps) { r.IdExamen = ex.IdExamen; r.IdPostulante = ex.IdPostulante; }
            lote.AddRange(resps);
            if (lote.Count >= 5000)
            {
                _context.RespuestasPostulante.AddRange(lote);
                await _context.SaveChangesAsync();
                lote.Clear();
            }
        }
        if (lote.Any()) { _context.RespuestasPostulante.AddRange(lote); await _context.SaveChangesAsync(); }
        await _context.SaveChangesAsync();
    }

    // ─────────────────────────────────────────────────────────────
    // Banco de 100 preguntas reales
    // ─────────────────────────────────────────────────────────────
    private static List<PreguntaExamen> GenerarPreguntas()
    {
        var banco = new (string area, string enunciado, string a, string b, string c, string d, string ok)[]
        {
            ("Matemática","¿Cuánto es 15 × 8?","110","120","125","130","B"),
            ("Matemática","¿Raíz cuadrada de 144?","10","11","12","13","C"),
            ("Matemática","Si x + 5 = 12, entonces x =","5","6","7","8","C"),
            ("Matemática","¿Cuánto es 2³?","6","8","9","12","B"),
            ("Matemática","¿Cuánto es el 25% de 200?","40","50","60","70","B"),
            ("Matemática","¿Cuál es el MCM de 4 y 6?","8","10","12","14","C"),
            ("Matemática","¿Área de un cuadrado de lado 5?","20","25","30","35","B"),
            ("Matemática","¿Cuánto es 3/4 + 1/4?","0.5","0.75","1","1.25","C"),
            ("Matemática","Si y = 2x y x = 3, entonces y =","4","5","6","7","C"),
            ("Matemática","¿Cuántos grados hay en un triángulo?","90","180","270","360","B"),
            ("Matemática","¿Cuánto es 1000 ÷ 25?","35","40","45","50","B"),
            ("Matemática","¿Perímetro de un rectángulo 4×6?","20","22","24","26","A"),
            ("Matemática","¿Cuánto es 7² − 5²?","14","20","24","30","C"),
            ("Matemática","¿Cuántos mm hay en 1 metro?","10","100","1000","10000","C"),
            ("Matemática","El número π es aproximadamente","3.12","3.14","3.16","3.18","B"),
            ("Matemática","¿Cuánto es log₁₀(100)?","1","2","10","20","B"),
            ("Matemática","Un número primo es divisible solamente por","1 y 2","2 y él mismo","1 y él mismo","cualquier número","C"),
            ("Matemática","¿Cuánto es 0.5 × 0.5?","0.1","0.25","0.5","1","B"),
            ("Matemática","La hipotenusa de un triángulo 3-4-? es","4","5","6","7","B"),
            ("Matemática","¿Cuánto es sen(90°)?","0","0.5","1","√2","C"),
            ("Matemática","¿Cuánto es cos(0°)?","0","0.5","1","−1","C"),
            ("Matemática","¿Cuánto es 2¹⁰?","512","1024","2048","4096","B"),
            ("Matemática","¿Qué fracción equivale a 0.75?","1/2","2/3","3/4","4/5","C"),
            ("Matemática","Con a=2 y b=3, ¿cuánto es (a+b)²?","20","22","25","30","C"),
            ("Matemática","¿Cuántos lados tiene un hexágono?","4","5","6","7","C"),
            ("Matemática","¿Cuánto es la factorial de 5 (5!)?","60","100","120","150","C"),
            ("Matemática","¿Qué es una progresión aritmética?","Serie donde cada término se multiplica","Serie donde la diferencia es constante","Serie de números primos","Serie de cuadrados perfectos","B"),
            ("Matemática","¿Cuánto es √(169)?","11","12","13","14","C"),
            ("Matemática","¿Cuál es el MCD de 12 y 18?","3","4","6","9","C"),
            ("Matemática","¿Cuánto es 3² + 4²?","20","21","25","30","C"),
            ("Matemática","Si se aumenta 200 en un 15%, el resultado es","220","225","230","235","C"),
            ("Matemática","¿Cuánto es tan(45°)?","0","0.5","1","√2","C"),
            ("Matemática","¿Cuántos ángulos rectos tiene un rectángulo?","2","3","4","5","C"),
            ("Matemática","Un ángulo obtuso mide entre","0° y 90°","90° y 180°","180° y 270°","270° y 360°","B"),
            ("Matemática","¿Cuánto es el volumen de un cubo de lado 3?","9","18","27","36","C"),
            ("Verbal","¿Sinónimo de 'feliz'?","Triste","Contento","Furioso","Aburrido","B"),
            ("Verbal","¿Antónimo de 'rápido'?","Ágil","Veloz","Lento","Fugaz","C"),
            ("Verbal","¿Cuál es el plural de 'luz'?","Luzs","Luces","Luses","Lucios","B"),
            ("Verbal","¿Qué figura retórica es una comparación usando 'como'?","Hipérbole","Metáfora","Símil","Antítesis","C"),
            ("Verbal","¿Qué tipo de oración es '¿Dónde estás?'","Declarativa","Imperativa","Interrogativa","Exclamativa","C"),
            ("Verbal","Un sustantivo nombra a una","Acción","Cualidad","Persona, cosa o lugar","Enlace","C"),
            ("Verbal","El narrador omnisciente es aquel que","Solo narra lo que ve","Conoce todo incluso pensamientos","Es el protagonista","Está fuera de la historia","B"),
            ("Verbal","Un adverbio modifica principalmente a","Un sustantivo","Un verbo, adjetivo u otro adverbio","Un pronombre","Una conjunción","B"),
            ("Verbal","La hipérbole consiste en","Una comparación directa","Una exageración con fin expresivo","Una contradicción","Una personificación","B"),
            ("Verbal","Un eufemismo es una expresión que","Insulta directamente","Suaviza algo desagradable","Compara dos cosas","Contradice una idea","B"),
            ("Verbal","¿Cuál es el género gramatical de 'mapa'?","Femenino","Masculino","Neutro","Indefinido","B"),
            ("Verbal","Un verbo copulativo une al","Sujeto con el predicado nominal","Objeto directo con el verbo","Adverbio con el adjetivo","Sujeto con el objeto","A"),
            ("Verbal","¿Cuál es el sujeto en 'Los niños juegan'?","juegan","Los","Los niños","niños","C"),
            ("Verbal","El prefijo 'pre-' significa","Después","Antes","Durante","Sin","B"),
            ("Verbal","¿Sinónimo de 'arduo'?","Fácil","Difícil","Rápido","Sencillo","B"),
            ("Verbal","La aliteración es la repetición de","Palabras iguales","Sonidos similares en un texto","Ideas contradictorias","Finales de verso","B"),
            ("Verbal","¿Cuál es el tiempo verbal de 'comió'?","Presente","Futuro","Pretérito perfecto simple","Condicional","C"),
            ("Verbal","La metonimia consiste en nombrar algo","Por una relación de contigüidad","Usando comparación directa","Con exageración","Atribuyendo vida a objetos","A"),
            ("Verbal","¿Cuál es el antónimo de 'generoso'?","Amable","Avaro","Bondadoso","Honesto","B"),
            ("Verbal","El pleonasmo es una figura que usa","Una redundancia innecesaria","Una metáfora extendida","Una ironía fina","Una aliteración","A"),
            ("Verbal","Las conjunciones son palabras que","Expresan acción","Enlazan oraciones o palabras","Nombran personas","Modifican adjetivos","B"),
            ("Verbal","¿Cuál es el diminutivo de 'libro'?","Librito","Librote","Librón","Librillo","A"),
            ("Verbal","¿Qué significa el prefijo 'bi-'?","Uno","Dos","Tres","Cuatro","B"),
            ("Verbal","¿Cuántas sílabas tiene 'mariposa'?","3","4","5","6","B"),
            ("Verbal","¿Qué es una oración compuesta?","La que tiene un solo verbo","La que tiene dos o más verbos","La que no tiene sujeto","La que solo tiene sujeto","B"),
            ("Verbal","¿Cuál es el superlativo de 'bueno'?","Más bueno","Muy bueno","Óptimo","Buenísimo","C"),
            ("Verbal","Un oxímoron une dos conceptos","Semejantes","Contradictorios","Sinónimos","Irrelevantes","B"),
            ("Verbal","¿Qué tiempo verbal expresa una acción futura probable?","Pretérito","Futuro simple","Condicional","Presente","C"),
            ("Verbal","El sujeto de una oración es quien","Realiza o recibe la acción","Solo recibe la acción","Describe al verbo","Enlaza oraciones","A"),
            ("Ciencias","¿Fórmula química del agua?","H2O","HO2","H2O2","H3O","A"),
            ("Ciencias","¿Qué planeta es el más grande del sistema solar?","Saturno","Júpiter","Urano","Neptuno","B"),
            ("Ciencias","¿Cuántos huesos tiene el cuerpo humano adulto?","196","206","216","226","B"),
            ("Ciencias","¿Qué organelo produce energía en la célula?","Núcleo","Ribosoma","Mitocondria","Lisosoma","C"),
            ("Ciencias","¿Qué gas se necesita para la respiración celular?","CO2","O2","N2","H2","B"),
            ("Ciencias","F = m × a corresponde a la","Ley de gravitación","2.ª ley de Newton","1.ª ley de Newton","Ley de Ohm","B"),
            ("Ciencias","¿Qué tejido recubre la superficie del cuerpo?","Muscular","Óseo","Epitelial","Nervioso","C"),
            ("Ciencias","La velocidad de la luz en el vacío es aproximadamente","200 000 km/s","300 000 km/s","400 000 km/s","500 000 km/s","B"),
            ("Ciencias","La fotosíntesis es el proceso por el que las plantas","Respiran CO2","Producen alimento usando luz solar","Digieren nutrientes","Se reproducen","B"),
            ("Ciencias","¿Número atómico del carbono (C)?","4","6","8","12","B"),
            ("Ciencias","El ADN es un ácido nucleico que contiene la","Energía celular","Información genética","Membrana celular","Proteína de transporte","B"),
            ("Ciencias","¿Cuál es el pH del agua pura?","5","7","8","9","B"),
            ("Ciencias","La homeostasis es el mecanismo que mantiene el","Movimiento celular","Equilibrio interno del organismo","División celular","Crecimiento óseo","B"),
            ("Ciencias","¿Qué partícula subatómica tiene carga negativa?","Protón","Neutrón","Electrón","Fotón","C"),
            ("Ciencias","¿Qué planeta es conocido como el planeta rojo?","Venus","Marte","Mercurio","Saturno","B"),
            ("Ciencias","¿Cuántos cromosomas tiene una célula humana normal?","23","46","48","92","B"),
            ("Ciencias","La ósmosis es el movimiento del","Soluto hacia la membrana","Solvente a través de membrana semipermeable","Gas hacia el interior","Ácido hacia la base","B"),
            ("Ciencias","CO2 es el símbolo del","Monóxido de carbono","Dióxido de carbono","Carbono puro","Carbonato","B"),
            ("Ciencias","¿Qué organelo se encarga de la síntesis de proteínas?","Mitocondria","Ribosoma","Núcleo","Vacuola","B"),
            ("Ciencias","La inercia es la resistencia de un cuerpo a","Cambiar su estado de movimiento","Atraer masas","Conducir electricidad","Refractar luz","A"),
            ("Ciencias","¿Qué gas es el más abundante en la atmósfera terrestre?","Oxígeno","Argón","Nitrógeno","CO2","C"),
            ("Ciencias","La selección natural fue propuesta por","Lamarck","Darwin","Mendel","Pasteur","B"),
            ("Ciencias","Un ecosistema incluye a los seres vivos y","Solo el agua","Solo el suelo","Su ambiente físico","Solo el aire","C"),
            ("Ciencias","La gravedad es una fuerza de","Atracción eléctrica","Repulsión magnética","Atracción entre masas","Repulsión eléctrica","C"),
            ("Ciencias","El elemento más abundante en la corteza terrestre es","Silicio","Hierro","Oxígeno","Aluminio","C"),
            ("Ciencias","¿Qué tipo de enlace comparte electrones entre átomos?","Iónico","Covalente","Metálico","Hidrógeno","B"),
            ("Ciencias","La mitosis es el proceso de","Reproducción sexual","División celular que da células idénticas","Síntesis de ADN","Respiración anaeróbica","B"),
            ("Ciencias","¿Cuál es la unidad de fuerza en el SI?","Joule","Newton","Pascal","Watt","B"),
            ("Ciencias","El efecto invernadero es causado principalmente por","O2 y N2","CO2 y CH4","H2 y He","Ar y Ne","B"),
            ("Ciencias","¿Qué es la cadena alimentaria?","Ciclo del agua","Secuencia de organismos que se alimentan entre sí","Ciclo del carbono","Proceso de fotosíntesis","B"),
            ("Ciencias","¿Cuántas cámaras tiene el corazón humano?","2","3","4","5","C"),
        };

        var lista = new List<PreguntaExamen>();
        int idx = 0;
        for (int n = 1; n <= 100; n++)
        {
            var (area, enunciado, a, b, c, d, ok) = banco[idx % banco.Length];
            idx++;
            lista.Add(new PreguntaExamen
            {
                NumeroPregunta = n, Area = area,
                Enunciado = $"{n}. {enunciado}",
                OpcionA = a, OpcionB = b, OpcionC = c, OpcionD = d,
                RespuestaCorrecta = ok
            });
        }
        return lista;
    }

    private static List<RespuestaPostulante> GenerarRespuestas(List<PreguntaExamen> preguntas, Random rnd)
    {
        var opciones = new[] { "A", "B", "C", "D" };
        return preguntas.Select(p =>
        {
            string sel = rnd.NextDouble() < 0.57 ? p.RespuestaCorrecta : opciones[rnd.Next(4)];
            return new RespuestaPostulante
            {
                IdPregunta = p.IdPregunta,
                RespuestaSeleccionada = sel,
                EsCorrecta = sel == p.RespuestaCorrecta
            };
        }).ToList();
    }

    // ─────────────────────────────────────────────────────────────
    // Nombres y datos realistas peruanos
    // ─────────────────────────────────────────────────────────────
    private static readonly string[] NombresH = {
        "Juan Carlos","Pedro Luis","Carlos Alberto","José Manuel","Miguel Ángel","Luis Eduardo",
        "Jorge Andrés","David Alejandro","Roberto Emilio","Ángel Gabriel","Fernando Rafael",
        "Rodrigo Fabián","Sebastián Ignacio","Cristian Paul","Jhon Fredy","Erick Daniel",
        "Kevin Alexander","Bryan Alonso","Diego Armando","Raúl Antonio","Omar Iván","Víctor Hugo",
        "César Augusto","Edgard Arturo","Harold Enrique","Wilmer Josué","Nilson Rubén","Brayam Jesús",
        "Yonatan Elías","Nixon Rolando","Frank Humberto","Elvis Gonzalo","Saúl Dámaso","Teófilo Jaime",
        "Isidro Ladislao","Zenón Cirilo","Aquilino Fidel","Celestino Moisés","Primitivo Exequiel"
    };
    private static readonly string[] NombresM = {
        "María del Carmen","Ana Lucía","Rosa Elena","Carmen Luz","Sandra Patricia","Lucía Fernanda",
        "Patricia Isabel","Elena Sofía","Mónica Beatriz","Susana Verónica","Paola Andrea","Vanesa Milagros",
        "Yesenia Rocío","Karina Esperanza","Melissa Judith","Claudia Marisol","Fiorella Xiomara",
        "Stefany Maribel","Liseth Noemí","Mayte Solange","Nilda Esperanza","Delia Angélica",
        "Hilda Perpetua","Celestina Rosaura","Benedicta Filomena","Jovita Zenaida","Hermelinda Flor",
        "Natividad Graciela","Prudencia Felícitas","Agripina Dolores","Maximiliana Cruz"
    };
    private static readonly string[] Apellidos = {
        "Quispe","Mamani","García","López","Torres","Flores","Rodríguez","Gonzales","Huanca",
        "Ccopa","Ccorimanya","Sulca","Palomino","Huamaní","Vargas","Cruz","Mendoza","Paucar",
        "Chipana","Cárdenas","Ramos","Gutiérrez","Morales","Paredes","Fernández","Apaza","Condori",
        "Sánchez","Reyes","Rojas","Castro","Llanos","Navarro","Ore","Ponce","Lazo","Asto",
        "Cáceres","Chávez","Herrera","Medina","Peña","Salinas","Tapia","Vega","Zapata",
        "Aquino","Bravo","Cabrera","Díaz","Espinoza","Figueroa","Guerrero","Huarancca",
        "Inca","Juárez","Laupa","Mayta","Nieto","Olarte","Pilco","Quispirimac","Rivas",
        "Samaniego","Tello","Uribe","Valdivia","Yana","Zenteno"
    };
    private static readonly string[] Distritos = {
        "Jr. Ayacucho","Av. Independencia","Jr. 28 de Julio","Jr. Libertad","Av. Cusco",
        "Jr. Bolognesi","Av. Universitaria","Jr. Grau","Av. Arequipa","Jr. San Martín",
        "Av. Lima","Jr. Junín","Av. Huancavelica","Jr. Tacna","Av. Ica"
    };
    private static readonly string[] Ciudades = {
        "Ayacucho","Huamanga","Wari","Quinua","Cangallo","Huanta","Puquio","Abancay","Lima","Cusco"
    };

    private static string ObtenerNombre(bool esMujer, Random rnd) =>
        esMujer ? NombresM[rnd.Next(NombresM.Length)] : NombresH[rnd.Next(NombresH.Length)];

    private static string ObtenerApellido(Random rnd) => Apellidos[rnd.Next(Apellidos.Length)];

    private static string ObtenerDireccion(Random rnd) =>
        $"{Distritos[rnd.Next(Distritos.Length)]} {rnd.Next(100, 999)}, {Ciudades[rnd.Next(Ciudades.Length)]}";

    private static string GenerarDni(int idx, Random rnd) =>
        (10000000 + idx + rnd.Next(100)).ToString().PadLeft(8, '0').Substring(0, 8);
}
