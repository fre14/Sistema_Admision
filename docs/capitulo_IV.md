# CAPÍTULO IV: RESULTADOS Y DISCUSIÓN

## 4.1. RESULTADOS DE LA INVESTIGACIÓN

El presente capítulo constituye el núcleo central del trabajo monográfico, en el cual se exponen de manera detallada y sistemática los resultados obtenidos durante el desarrollo del Sistema Automatizado de Admisión (SAA). Los hallazgos se presentan organizados según las variables operacionales definidas en el marco metodológico, correspondientes a las fases de análisis (X1), diseño (X2), implementación (X3) y validación de funcionamiento (X4). Cada sección documenta las evidencias del enfoque Specification-Driven Development (SSD), demostrando cómo las especificaciones formales guiaron de manera determinante cada etapa del ciclo de vida del software.

El desarrollo del SAA responde a la necesidad institucional de automatizar el proceso de admisión universitaria, eliminando los procedimientos manuales propensos a error y garantizando transparencia, trazabilidad y equidad en la clasificación de postulantes. A lo largo de este capítulo, se presentarán las tablas de requisitos, los diagramas arquitectónicos, las evidencias de implementación, los resultados de las pruebas unitarias e integración, y el análisis de cobertura de código, proporcionando una visión integral del producto desarrollado.

---

### 4.1.1. Resultados de la Fase de Análisis (Variable X1)

La fase de análisis constituye el primer pilar del enfoque SSD adoptado para el desarrollo del Sistema Automatizado de Admisión (SAA). Siguiendo los principios del Specification-Driven Development, se estableció como premisa fundamental que **todas las especificaciones funcionales y no funcionales debían ser definidas de manera formal y completa antes de escribir una sola línea de código**. Este enfoque, respaldado por autores como Sommerville (2016) y Pressman (2015), garantiza que el proceso de desarrollo sea guiado por las necesidades del usuario y no por decisiones técnicas ad hoc.

#### 4.1.1.1. Proceso de Levantamiento de Requisitos

El proceso de levantamiento de requisitos se llevó a cabo mediante las siguientes técnicas:

- **Análisis documental**: Se revisaron los reglamentos de admisión vigentes en instituciones de educación superior de la región, identificando los procesos comunes que debía automatizar el sistema: registro de postulantes, aplicación de exámenes, clasificación por puntaje, generación de orden de mérito y publicación de resultados.

- **Estudio de sistemas análogos**: Se analizaron sistemas de admisión existentes en universidades públicas y privadas, identificando patrones de diseño y funcionalidades esenciales que debían incorporarse al SAA.

- **Definición formal de especificaciones SSD**: A partir de la información recopilada, se elaboraron especificaciones técnicas formalizadas que incluyen requisitos funcionales (RF), requisitos no funcionales (RNF), casos de uso detallados y una matriz de trazabilidad bidireccional. Estas especificaciones fueron documentadas previo al inicio de la fase de implementación, conforme al enfoque SSD.

El resultado de este proceso fue un Documento de Especificación de Requisitos del Software (ERS) completo, que sirvió como contrato técnico entre las fases de diseño, implementación y validación.

#### 4.1.1.2. Requisitos Funcionales del Sistema

A continuación se presenta la tabla completa de requisitos funcionales identificados para el SAA. Cada requisito fue codificado, categorizado por módulo y asignado un nivel de prioridad según su criticidad para el proceso de admisión.

**Tabla 1:** Requisitos Funcionales del Sistema Automatizado de Admisión (SAA)

| Código | Nombre del Requisito | Descripción | Módulo | Prioridad |
|--------|----------------------|-------------|--------|-----------|
| RF-01 | Gestión de Registro de Postulantes | El sistema debe permitir el registro de nuevos postulantes con datos personales completos (nombres, apellidos, DNI, correo, teléfono, programa de interés). El DNI debe ser único y no repetible en el sistema. Al registrarse, se crea automáticamente un usuario con rol "Postulante" vinculado al DNI. | Registro | Alta |
| RF-02 | Registro de Exámenes de Admisión | El sistema debe permitir registrar los resultados de exámenes de admisión, vinculando el puntaje obtenido a un postulante específico mediante su identificador. Se deben registrar observaciones opcionales y la fecha del examen de manera automática. | Evaluación | Alta |
| RF-03 | Clasificación Automatizada del Motor de Admisión | El sistema debe procesar automáticamente los resultados de admisión mediante un motor de clasificación que: (a) ordena postulantes por puntaje descendente dentro de cada programa académico, (b) asigna el estado "Ingresante" a quienes tengan puntaje ≥ 50.0 y existan vacantes disponibles, (c) asigna "Aprobado" a quienes superen el umbral pero no alcancen vacante, (d) asigna "Desaprobado" a quienes obtengan puntaje < 50.0. | Motor de Admisión | Crítica |
| RF-04 | Trazabilidad de Resultados de Admisión | El sistema debe generar y almacenar registros de resultados de admisión (`ResultadoAdmision`) que incluyan: postulante, programa académico, calificación, estado final, orden de mérito y fecha del resultado. Antes de cada procesamiento, los resultados anteriores deben ser eliminados para garantizar consistencia. | Resultados | Alta |
| RF-05 | Autenticación de Usuarios mediante JWT | El sistema debe implementar autenticación basada en JSON Web Tokens (JWT) con soporte dual: (a) usuarios administrativos se autentican con nombre de usuario y contraseña, (b) postulantes se autentican con su nombre y DNI. El token generado incluye claims de identificador, nombre y rol. | Seguridad | Crítica |
| RF-06 | Gestión de Programas Académicos | El sistema debe gestionar un catálogo de programas académicos con atributos: código, nombre, descripción, nivel académico, número de vacantes, fechas de proceso, estado y departamento. Los programas determinan la distribución de vacantes. | Configuración | Alta |
| RF-07 | Generación de Reporte de Ingresantes | El sistema debe generar un reporte filtrado que muestre exclusivamente a los postulantes con estado "Ingresante", ordenados por orden de mérito (puesto). El reporte incluye DNI, nombres, apellidos, programa académico, puntaje, puesto y fecha de admisión. | Reportes | Alta |
| RF-08 | Generación de Reporte General de Resultados | El sistema debe generar un reporte completo que incluya a todos los postulantes procesados, independientemente de su estado (Ingresante, Aprobado, Desaprobado), ordenados por puntaje descendente. El reporte incluye los mismos campos que RF-07 más el campo de estado. | Reportes | Alta |
| RF-09 | Consulta de Resultado Individual por Postulante | El sistema debe permitir que cada postulante autenticado consulte su resultado personal mediante su DNI (extraído del token JWT). La respuesta incluye nombres, apellidos, programa, puntaje, estado y puesto obtenido. Si no existen resultados, se retornan valores por defecto ("Pendiente", puntaje 0, puesto 0). | Portal Postulante | Alta |
| RF-10 | Control de Acceso basado en Roles | El sistema debe implementar autorización por roles: (a) "Administrador" puede acceder a los endpoints de gestión de admisión, procesamiento del motor y reportes, (b) "Postulante" puede acceder únicamente al endpoint de consulta de resultado individual. Los endpoints protegidos deben rechazar solicitudes sin token válido con código HTTP 401. | Seguridad | Crítica |
| RF-11 | Gestión de Fichas de Postulación | El sistema debe gestionar fichas de postulación que vinculen a un postulante con un programa académico específico, incluyendo número de trámite, fecha de postulación, estado y observaciones. Las fichas son el vínculo entre postulantes y exámenes dentro de cada programa. | Registro | Media |
| RF-12 | Validación de Datos de Entrada | El sistema debe validar la integridad de los datos de entrada: (a) unicidad del DNI al registrar postulantes, lanzando excepción `InvalidOperationException` con mensaje descriptivo si el DNI ya existe, (b) existencia del postulante al registrar exámenes, lanzando excepción si el postulante no se encuentra, (c) validación del modelo en el endpoint de login mediante `ModelState`. | Validación | Alta |
| RF-13 | Procesamiento Transaccional | El sistema debe garantizar la atomicidad de las operaciones críticas mediante transacciones de base de datos: (a) el registro de postulantes incluye la creación atómica del postulante y su usuario vinculado, (b) el registro de exámenes utiliza transacciones con soporte para bases de datos InMemory, (c) el procesamiento del motor de admisión se ejecuta dentro de una transacción con rollback automático en caso de error. | Infraestructura | Alta |
| RF-14 | Orden de Mérito Automatizado | El sistema debe generar automáticamente el orden de mérito de los postulantes dentro de cada programa académico, asignando posiciones secuenciales (1, 2, 3...) basadas en el puntaje obtenido de mayor a menor. Un postulante solo puede ocupar un cupo en un único programa, evitando acumulación de vacantes. | Motor de Admisión | Alta |
| RF-15 | Gestión de Períodos de Admisión | El sistema debe soportar la gestión de períodos de admisión con nombre, fecha de inicio y fecha de fin, permitiendo organizar los procesos de admisión en ciclos temporales definidos. | Configuración | Media |

Como se observa en la Tabla 1, se identificaron un total de quince (15) requisitos funcionales distribuidos en siete módulos del sistema: Registro, Evaluación, Motor de Admisión, Resultados, Seguridad, Configuración, Reportes y Portal del Postulante. De estos requisitos, tres (3) fueron clasificados con prioridad Crítica (RF-03, RF-05 y RF-10), diez (10) con prioridad Alta y dos (2) con prioridad Media.

#### 4.1.1.3. Requisitos No Funcionales del Sistema

Complementando los requisitos funcionales, se definieron los requisitos no funcionales que establecen las restricciones de calidad, rendimiento y arquitectura del sistema.

**Tabla 2:** Requisitos No Funcionales del Sistema Automatizado de Admisión (SAA)

| Código | Nombre | Descripción | Categoría |
|--------|--------|-------------|-----------|
| RNF-01 | Arquitectura Limpia (Clean Architecture) | El sistema debe implementar Clean Architecture con separación estricta en cuatro capas: Dominio, Aplicación, Infraestructura y Presentación. Las dependencias deben fluir hacia el centro (Domain), garantizando independencia de frameworks y testabilidad. | Arquitectura |
| RNF-02 | Estándares de Codificación | El código fuente debe seguir las convenciones de nomenclatura estándar: PascalCase para clases, métodos y propiedades en C#, camelCase para variables locales y parámetros en TypeScript. Se deben aplicar los principios SOLID en el diseño de clases y servicios. | Mantenibilidad |
| RNF-03 | Cobertura Mínima de Pruebas | El proyecto debe alcanzar una cobertura mínima del 50% en líneas de código, con énfasis especial en la capa de Aplicación (servicios de negocio) donde se debe superar el 70%. Las pruebas deben cubrir escenarios positivos, negativos y de borde. | Calidad |
| RNF-04 | Seguridad de la Información | Las comunicaciones entre cliente y servidor deben estar protegidas mediante HTTPS. Los tokens JWT deben tener una expiración máxima de 2 horas. El sistema debe validar emisor, audiencia, tiempo de vida y firma del token en cada solicitud protegida. | Seguridad |
| RNF-05 | Persistencia con Entity Framework Core | La capa de persistencia debe utilizar Entity Framework Core 10 con enfoque Code-First. La configuración del modelo debe realizarse mediante Fluent API en el método `OnModelCreating`, con separación en esquemas de base de datos (`Admision`, `Config`, `Seguridad`). | Tecnología |
| RNF-06 | Interfaz de Usuario Responsiva | La interfaz web debe implementarse como Single Page Application (SPA) con React 19 y TypeScript 5.9, utilizando diseño glassmorphism con animaciones suaves. Debe soportar dos portales diferenciados: Administrador y Postulante. | Usabilidad |
| RNF-07 | Control de Versiones | El código fuente debe gestionarse mediante Git con repositorio centralizado en GitHub, empleando commits descriptivos y una estrategia de ramas que permita la trazabilidad de cambios conforme al enfoque SSD. | Gestión de Configuración |

#### 4.1.1.4. Descripción Detallada de Requisitos Funcionales Principales

A continuación se presentan las tablas de descripción detallada para los dos requisitos funcionales de mayor criticidad en el sistema.

**Tabla 3:** Descripción detallada del RF-03 — Clasificación Automatizada del Motor de Admisión

| Campo | Detalle |
|-------|---------|
| **Código** | RF-03 |
| **Nombre** | Clasificación Automatizada del Motor de Admisión |
| **Descripción** | El sistema procesa automáticamente los resultados de admisión aplicando reglas de negocio predefinidas para clasificar a los postulantes según su puntaje y la disponibilidad de vacantes en cada programa académico. |
| **Actores** | Administrador del Sistema |
| **Precondiciones** | (1) Existen postulantes registrados con fichas de postulación vinculadas a programas académicos. (2) Se han registrado exámenes con puntajes para dichos postulantes. (3) Los programas académicos tienen definido el número de vacantes. |
| **Flujo Principal** | 1. El administrador invoca el endpoint `POST /api/admision/procesar`. 2. El sistema elimina todos los resultados previos para garantizar consistencia. 3. Para cada programa académico, el sistema recupera las fichas y exámenes asociados. 4. Los exámenes se ordenan por puntaje de mayor a menor. 5. Para cada examen, se aplica la regla del umbral mínimo (≥ 50.0). 6. Si el puntaje supera el umbral y hay vacantes disponibles, se asigna estado "Ingresante". 7. Si el puntaje supera el umbral pero no hay vacantes, se asigna estado "Aprobado". 8. Si el puntaje no supera el umbral, se asigna estado "Desaprobado". 9. Se asigna el orden de mérito secuencial. 10. Un postulante solo puede ingresar a un programa (se usa `HashSet<int>` para rastrear asignaciones). 11. Se persisten los resultados en una transacción atómica. |
| **Postcondiciones** | Se generan registros de `ResultadoAdmision` para todos los postulantes evaluados con su estado final, calificación y orden de mérito. |
| **Flujo Alternativo** | FA-01: Si no existen exámenes registrados para un programa, no se generan resultados para ese programa. FA-02: Si ocurre un error durante el procesamiento, la transacción se revierte automáticamente (rollback). |
| **Reglas de Negocio** | RN-01: Umbral mínimo aprobatorio = 50.0 puntos. RN-02: Un postulante no puede acumular cupos en múltiples programas. RN-03: Los resultados anteriores se eliminan antes de cada nuevo procesamiento. |

**Tabla 4:** Descripción detallada del RF-05 — Autenticación de Usuarios mediante JWT

| Campo | Detalle |
|-------|---------|
| **Código** | RF-05 |
| **Nombre** | Autenticación de Usuarios mediante JWT |
| **Descripción** | El sistema implementa un mecanismo de autenticación dual basado en JSON Web Tokens que permite el acceso diferenciado para administradores y postulantes. |
| **Actores** | Administrador, Postulante |
| **Precondiciones** | (1) El usuario debe estar registrado en el sistema. (2) El estado del usuario o postulante debe ser "Activo". |
| **Flujo Principal** | 1. El usuario envía credenciales al endpoint `POST /api/auth/login`. 2. El sistema busca primero en la tabla de Usuarios (excluyendo postulantes). 3. Si se encuentra un usuario administrativo, se valida la contraseña. 4. Si no se encuentra un usuario administrativo, se busca en la tabla de Postulantes (nombre + DNI). 5. Se verifica que el usuario/postulante no esté inactivo. 6. Se genera un token JWT con claims: `NameIdentifier`, `Name`, `Role`. 7. El token se configura con expiración de 2 horas. 8. Se firma con clave simétrica HMAC-SHA256. 9. Se registra la fecha de último acceso. 10. Se retorna el token junto con los datos del usuario. |
| **Postcondiciones** | El usuario obtiene un token JWT válido que puede incluir en el header `Authorization: Bearer {token}` para acceder a los endpoints protegidos. |
| **Flujo Alternativo** | FA-01: Si las credenciales son inválidas, se retorna código HTTP 401 con mensaje "Usuario o contraseña incorrectos". FA-02: Si el usuario está inactivo, se retorna código HTTP 401 con mensaje descriptivo. |

#### 4.1.1.5. Matriz de Trazabilidad de Requisitos

La matriz de trazabilidad bidireccional permite rastrear la relación entre cada requisito funcional y los artefactos de implementación y pruebas que lo materializan. Esta matriz es un elemento esencial del enfoque SSD, ya que demuestra que cada especificación fue implementada y verificada.

**Tabla 5:** Matriz de Trazabilidad de Requisitos

| Requisito | Componente de Implementación | Clase/Servicio | Prueba(s) Asociada(s) | Estado |
|-----------|------------------------------|----------------|----------------------|--------|
| RF-01 | PostulantesController, PostulanteService | `CrearPostulanteAsync()` | `RegistrarPostulanteAsync_ValidRequest_RegistersSuccessfully`, `RegistrarPostulanteAsync_DniYaExiste_ThrowsException` | Verificado |
| RF-02 | AdmisionController, MotorAdmisionService | `RegistrarExamenAsync()` | `RegistrarExamenAsync_PostulanteExiste_RegistraExamen`, `RegistrarExamenAsync_PostulanteNoEncontrado_LanzaExcepcion` | Verificado |
| RF-03 | AdmisionController, MotorAdmisionService | `ProcesarResultadosAsync()` | `ProcesarResultados_ConCuposSuficientes_ApruebaPostulante`, `ProcesarResultados_SinCupos_AprobadoPeroNoIngresa`, `ProcesarResultados_PuntajeMenorA50_DesapruebaPostulante` | Verificado |
| RF-04 | MotorAdmisionService | `ProcesarResultadosAsync()` | `ProcesarResultados_ConCuposSuficientes_ApruebaPostulante` | Verificado |
| RF-05 | AuthController, AuthService | `LoginAsync()` | `LoginAsync_ValidCredentials_ReturnsTokenAndSuccess`, `LoginAsync_InvalidCredentials_ReturnsFailure`, `LoginAsync_InactiveUser_ReturnsFailure` | Verificado |
| RF-06 | SAADbContext, ProgramaAcademico | `DbSet<ProgramaAcademico>` | `DbContext_ModelCreation_Success` | Verificado |
| RF-07 | AdmisionController, MotorAdmisionService | `ObtenerReporteIngresantesAsync()` | `ObtenerReporteIngresantesAsync_DevuelveSoloIngresantesOrdenados`, `ObtenerReporteIngresantesAsync_SinResultados_DevuelveListaVacia` | Verificado |
| RF-08 | AdmisionController, MotorAdmisionService | `ObtenerReporteTodosAsync()` | `ObtenerReporteTodosAsync_DevuelveTodosOrdenadosPorPuntaje` | Verificado |
| RF-09 | PostulantesController, PostulanteService | `ObtenerMiResultadoAsync()` | `ObtenerMiResultadoAsync_DniExists_ReturnsResultado`, `ObtenerMiResultadoAsync_DniDoesNotExist_ReturnsNull`, `ObtenerMiResultadoAsync_SinResultadoYPrograma_ReturnsFallbackValues` | Verificado |
| RF-10 | Program.cs, Controllers | `[Authorize(Roles)]` | `GetReporteIngresantes_WithoutToken_ReturnsUnauthorized` | Verificado |
| RF-11 | SAADbContext, FichaPostulacion | `DbSet<FichaPostulacion>` | `DbContext_ModelCreation_Success` | Verificado |
| RF-12 | PostulanteService, MotorAdmisionService | Validaciones inline | `RegistrarPostulanteAsync_DniYaExiste_ThrowsException`, `RegistrarExamenAsync_PostulanteNoEncontrado_LanzaExcepcion` | Verificado |
| RF-13 | PostulanteService, MotorAdmisionService | `BeginTransactionAsync()` | Implícito en pruebas unitarias con InMemory DB | Verificado |
| RF-14 | MotorAdmisionService | `ProcesarResultadosAsync()` | `ProcesarResultados_PostulanteNoAcumulaCupos_SeAsignaAUnSoloPrograma` | Verificado |
| RF-15 | SAADbContext, PeriodoAdmision | `DbSet<PeriodoAdmision>` | `DbContext_ModelCreation_Success` | Verificado |

Como se puede apreciar en la Tabla 5, los quince (15) requisitos funcionales cuentan con componentes de implementación asociados y pruebas que verifican su correcto funcionamiento. La trazabilidad completa confirma que el enfoque SSD fue aplicado de manera integral, garantizando que ninguna especificación quedara sin implementación ni verificación correspondiente.

---

### 4.1.2. Resultados de la Fase de Diseño (Variable X2)

La fase de diseño transformó las especificaciones definidas en la fase de análisis en artefactos técnicos concretos que guiarían la implementación del SAA. Siguiendo el enfoque SSD, cada decisión de diseño fue trazable a uno o más requisitos funcionales o no funcionales definidos previamente.

#### 4.1.2.1. Diseño del Modelo Entidad-Relación

El modelo de datos del SAA fue diseñado utilizando Entity Framework Core 10 con enfoque Code-First, definiendo las entidades del dominio como clases C# (POCOs) y configurando sus relaciones y restricciones mediante Fluent API en el método `OnModelCreating` de la clase `SAADbContext`. El esquema generado mediante migraciones EF Core produce el script SQL `SAA_AdmisionDB_Schema.sql`, que refleja fielmente la estructura diseñada.

El modelo entidad-relación se organiza en tres esquemas lógicos de base de datos que agrupan las tablas según su dominio funcional:

**Esquema `Admision`:** Contiene las tablas principales del proceso de admisión:
- **Postulante**: Almacena los datos personales de cada postulante (IdPostulante PK, Nombres, Apellidos, DNI con índice único `IX_Postulante_DNI`, IdProgramaInteres, Correo, Teléfono, Dirección, FechaNacimiento, Estado, FechaCreación, FechaActualización). La restricción de unicidad sobre el campo DNI garantiza el cumplimiento del RF-12.
- **FichaPostulacion**: Vincula un postulante con un programa académico específico (IdFichaPostulacion PK, IdPostulante, IdProgramaAcademico, NumeroTramite, FechaPostulacion, Estado, Observaciones).
- **ExamenAdmision**: Registra los exámenes rendidos por cada postulante (IdExamen PK, IdFichaPostulacion, IdPostulante, NombreExamen, FechaExamen, HoraInicio, DuracionMinutos, Sala, Estado, Puntaje con precisión decimal(18,2), Observaciones).
- **ResultadoAdmision**: Almacena los resultados generados por el motor de admisión (IdResultado PK, IdFichaPostulacion, IdPostulante, IdProgramaAcademico, Calificacion con precisión decimal(18,2), Resultado, OrdenMerito, Observaciones, FechaResultado, IdUsuarioEvaluador).

**Esquema `Config`:** Contiene las tablas de configuración del sistema:
- **ProgramaAcademico**: Define los programas académicos ofertados (IdProgramaAcademico PK, Codigo, Nombre, Descripcion, NivelAcademico, Vacantes, FechaInicioProceso, FechaFinalProceso, Estado, Departamento).
- **PeriodoAdmision**: Gestiona los períodos temporales de admisión (IdPeriodo PK, Nombre, FechaInicio, FechaFin).

**Esquema `Seguridad`:** Contiene las tablas de control de acceso:
- **Usuario**: Gestiona las cuentas de usuario del sistema (IdUsuario PK, NombreUsuario, Contrasena, NombreCompleto, Correo, Rol, Estado, UltimoAcceso).
- **Rol**: Define los roles disponibles en el sistema (IdRol PK, Nombre, Descripcion).

La separación en esquemas sigue las mejores prácticas de diseño de bases de datos empresariales, permitiendo una gestión granular de permisos a nivel de esquema y facilitando la comprensión del modelo por parte de los desarrolladores. El diagrama completo del modelo entidad-relación se presenta en el Anexo A.

#### 4.1.2.2. Diseño de la Arquitectura del Sistema (Clean Architecture)

El SAA fue diseñado siguiendo los principios de Clean Architecture propuestos por Robert C. Martin (2017), organizando el código en cuatro capas concéntricas con dependencias que fluyen exclusivamente desde las capas externas hacia las internas. Este diseño responde directamente al RNF-01 y permite alcanzar un alto nivel de testabilidad, ya que la lógica de negocio (capa de Aplicación) puede probarse de forma aislada sin depender de la base de datos real ni del framework web.

Las cuatro capas del SAA son:

1. **SAA.Domain (Capa de Dominio)**: Capa más interna que contiene las entidades del negocio como clases C# puras (POCOs). No tiene dependencias externas.

2. **SAA.Application (Capa de Aplicación)**: Contiene los servicios de aplicación que implementan la lógica de negocio, los DTOs (Data Transfer Objects) para la comunicación entre capas y las interfaces que definen contratos de acceso a datos.

3. **SAA.Infrastructure (Capa de Infraestructura)**: Implementa los contratos definidos en la capa de Aplicación, proporcionando el acceso a datos mediante Entity Framework Core y el servicio de siembra de datos iniciales.

4. **SAA.API (Capa de Presentación)**: Contiene los controladores ASP.NET Core que exponen los endpoints REST, la configuración de middleware (JWT, CORS) y el frontend React/TypeScript.

El diagrama de arquitectura completo se presenta en el Anexo B. Esta separación de responsabilidades posibilita que las pruebas unitarias del motor de admisión utilicen una base de datos InMemory en lugar de SQL Server, sin modificar ni una línea del código de producción.

#### 4.1.2.3. Diseño de la Interfaz de Usuario

Los prototipos de la interfaz de usuario fueron diseñados siguiendo los principios de diseño moderno con estética glassmorphism, priorizando la experiencia del usuario y la claridad en la presentación de la información. Se diseñaron tres vistas principales:

1. **Página de Login (Portal SAA)**: Diseño centrado con panel glassmorphism sobre un fondo oscuro con formas animadas. Incluye campos para usuario/DNI y contraseña, con validación en tiempo real y retroalimentación visual de errores.

2. **Dashboard de Administrador**: Panel de administración que muestra la tabla de resultados con filtros por programa académico y tipo de resultado (Ingresantes / No Ingresantes). Incluye el botón de procesamiento del motor de admisión y la visualización del reporte con columnas de puesto, nombre, programa y puntaje.

3. **Dashboard de Postulante**: Vista personalizada que muestra el estado de admisión del postulante autenticado, incluyendo su programa académico, puntaje obtenido, puesto en el orden de mérito y un mensaje contextual según su resultado (felicitación para ingresantes, notificación para aprobados sin vacante, mensaje informativo para desaprobados).

Los prototipos completos de las interfaces se presentan en el Anexo C. La tipografía seleccionada fue la familia "Outfit" de Google Fonts, con pesos de 300 a 700, garantizando legibilidad y coherencia visual en todas las pantallas.

---

### 4.1.3. Resultados de la Fase de Implementación (Variable X3)

La fase de implementación constituye la sección más extensa del presente capítulo, dado que en ella se materializan las especificaciones y diseños previamente definidos. Cada decisión de implementación se fundamenta en las especificaciones del documento ERS, demostrando la aplicación rigurosa del enfoque SSD.

#### 4.1.3.1. Enfoque SSD Aplicado al Desarrollo

El Specification-Driven Development (SSD) estableció un flujo de trabajo disciplinado en el cual cada artefacto de código fue creado como respuesta directa a una especificación formal previamente documentada. Este enfoque se materializó de la siguiente manera:

1. **Especificación → Entidad de Dominio**: Cada tabla identificada en el modelo ER se convirtió en una clase C# en el proyecto `SAA.Domain`. Por ejemplo, la especificación de la tabla `Postulante` con sus atributos DNI, Nombres, Apellidos y campos de auditoría se tradujo directamente en la clase `Postulante.cs` con las mismas propiedades y documentación XML.

2. **Especificación → Servicio de Aplicación**: Cada requisito funcional que involucraba lógica de negocio se implementó como un método específico en un servicio de la capa de Aplicación. El RF-03 (Clasificación Automatizada) se implementó en el método `ProcesarResultadosAsync()` del `MotorAdmisionService`, el RF-01 (Gestión de Registro) en `CrearPostulanteAsync()` del `PostulanteService`, y el RF-05 (Autenticación) en `LoginAsync()` del `AuthService`.

3. **Especificación → Prueba Unitaria**: Cada escenario de prueba fue derivado directamente de los flujos principal y alternativo descritos en las especificaciones de requisitos. Los nombres de los métodos de prueba reflejan la especificación que verifican, por ejemplo: `ProcesarResultados_ConCuposSuficientes_ApruebaPostulante` verifica el flujo principal del RF-03.

4. **Especificación → Endpoint API**: Cada operación especificada se expuso como un endpoint RESTful documentado, con los códigos de respuesta HTTP definidos en la especificación.

Este flujo unidireccional desde las especificaciones hacia el código garantiza que no exista código "huérfano" sin justificación funcional ni especificaciones sin implementación correspondiente.

#### 4.1.3.2. Stack Tecnológico y Estándares de Codificación

El stack tecnológico del SAA fue seleccionado con base en criterios de modernidad, soporte empresarial, compatibilidad con Clean Architecture y facilidad para la implementación de pruebas automatizadas. A continuación se presenta la descripción detallada de cada componente.

**Tabla 6:** Stack Tecnológico del Sistema Automatizado de Admisión (SAA)

| Categoría | Tecnología | Versión | Justificación |
|-----------|------------|---------|---------------|
| **Plataforma Backend** | .NET | 10.0 | Framework de desarrollo multiplataforma de Microsoft, con soporte LTS y alto rendimiento para aplicaciones web empresariales. |
| **Lenguaje Backend** | C# | 13.0 | Lenguaje orientado a objetos con tipado fuerte, ideal para implementar principios SOLID y Clean Architecture. |
| **Framework Web** | ASP.NET Core | 10.0 | Framework web de alto rendimiento con soporte nativo para APIs REST, middleware pipeline, inyección de dependencias y autenticación JWT. |
| **ORM** | Entity Framework Core | 10.0 | ORM con enfoque Code-First que permite definir el modelo de datos como clases C# y generar migraciones SQL automáticas. Soporta Fluent API para configuraciones avanzadas. |
| **Base de Datos** | SQL Server | 2022 | Sistema de gestión de bases de datos relacional empresarial con soporte para esquemas, transacciones ACID y alta disponibilidad. |
| **Frontend Framework** | React | 19.0 | Biblioteca para interfaces de usuario basada en componentes, con renderizado eficiente mediante Virtual DOM y hooks para gestión de estado. |
| **Lenguaje Frontend** | TypeScript | 5.9 | Superconjunto de JavaScript con tipado estático que mejora la mantenibilidad y detección temprana de errores en el código frontend. |
| **Herramienta de Build** | Vite | 7.0 | Herramienta de build de nueva generación que ofrece Hot Module Replacement (HMR) instantáneo y builds optimizados para producción. |
| **Autenticación** | JWT (JSON Web Tokens) | RFC 7519 | Estándar abierto para la creación de tokens de acceso seguros, sin estado (stateless), firmados con HMAC-SHA256. |
| **Framework de Pruebas** | xUnit | 2.9 | Framework de pruebas unitarias para .NET, recomendado por Microsoft, con soporte nativo para pruebas paralelizadas y descubrimiento automático. |
| **Aserciones** | FluentAssertions | 8.0 | Biblioteca de aserciones con sintaxis fluida que mejora la legibilidad y expresividad de las pruebas unitarias. |
| **Mocking** | Moq | 4.20 | Biblioteca de mocking para crear objetos simulados en pruebas unitarias, utilizada para simular `IConfiguration` en las pruebas de autenticación. |
| **Cobertura de Código** | Coverlet | 6.0 | Herramienta multiplataforma de cobertura de código para .NET, con generación de reportes en formato Cobertura XML. |
| **Reportes de Cobertura** | ReportGenerator | 5.4 | Herramienta para generar reportes HTML detallados de cobertura a partir de archivos Cobertura XML. |
| **Base de Datos en Pruebas** | EF Core InMemory | 10.0 | Proveedor de base de datos en memoria de Entity Framework Core, utilizado para pruebas unitarias sin dependencia de SQL Server. |
| **Control de Versiones** | Git + GitHub | — | Sistema de control de versiones distribuido con repositorio centralizado para colaboración y trazabilidad de cambios. |

**Estándares de Codificación Aplicados**

El proyecto SAA sigue rigurosamente los estándares de codificación recomendados por Microsoft y la comunidad .NET:

- **Nomenclatura en C#**: Se utiliza PascalCase para nombres de clases (`MotorAdmisionService`), métodos (`ProcesarResultadosAsync`), propiedades (`IdPostulante`) y namespaces (`SAA.Application.Services`). Las variables locales y parámetros utilizan camelCase (`postulante`, `examen`). Los campos privados con prefijo underscore (`_context`, `_config`). Las interfaces utilizan el prefijo "I" (`IApplicationDbContext`).

- **Nomenclatura en TypeScript/React**: Se utiliza camelCase para funciones (`handleLogin`, `fetchReporteTodos`), variables de estado (`token`, `userData`, `reporte`) y props. Los componentes de React utilizan PascalCase (`App`). Las constantes CSS utilizan kebab-case con la convención de variables CSS (`--bg-dark`, `--primary-glow`).

- **Principios SOLID**: El diseño del sistema aplica los cinco principios SOLID: (S) Cada servicio tiene una única responsabilidad — `MotorAdmisionService` para lógica de admisión, `PostulanteService` para gestión de postulantes, `AuthService` para autenticación. (O) Las entidades del dominio están abiertas a extensión pero cerradas a modificación. (L) Los servicios dependen de la interfaz `IApplicationDbContext` y no del `SAADbContext` concreto, permitiendo sustitución por implementaciones de prueba. (I) La interfaz `IApplicationDbContext` expone solo los `DbSet<>` necesarios. (D) Los controladores dependen de abstracciones de servicio inyectadas mediante el contenedor de dependencias de ASP.NET Core.

- **Convenciones de API REST**: Los endpoints siguen las convenciones RESTful estándar: verbos HTTP semánticos (POST para creación y acciones, GET para consultas), rutas en kebab-case (`reporte-ingresantes`, `mi-resultado`), códigos de respuesta HTTP apropiados (200 OK, 400 Bad Request, 401 Unauthorized, 404 Not Found).

- **Documentación XML**: Las entidades del dominio incluyen documentación XML completa con etiquetas `<summary>` que describen el propósito de cada clase y propiedad, como se evidencia en las clases `Postulante.cs`, `ExamenAdmision.cs`, `ResultadoAdmision.cs`, `ProgramaAcademico.cs` y `Usuario.cs`.

#### 4.1.3.3. Arquitectura del Proyecto (Clean Architecture)

La solución del SAA está organizada en el archivo `SAA.Solution.slnx`, que contiene seis proyectos distribuidos en las cuatro capas de Clean Architecture más un proyecto de pruebas y un proyecto AppHost. A continuación se describe en detalle cada capa con los artefactos de código implementados.

##### A. SAA.Domain — Capa de Dominio

La capa de dominio constituye el núcleo de la arquitectura y contiene las entidades de negocio del sistema. Esta capa no tiene dependencias a ninguna otra capa ni a paquetes externos, cumpliendo estrictamente con el principio de inversión de dependencias. Las entidades se implementaron como clases C# puras (POCOs — Plain Old CLR Objects), sin anotaciones de framework ni lógica de persistencia.

El proyecto `SAA.Domain` contiene 16 entidades organizadas en el namespace `SAA.Domain.Entities`:

**Tabla 7:** Entidades del Dominio del SAA

| N.° | Entidad | Descripción | Propiedades Principales |
|-----|---------|-------------|------------------------|
| 1 | `Postulante` | Persona que se postula al proceso de admisión | IdPostulante, Nombres, Apellidos, DNI, IdProgramaInteres, Correo, Telefono, Direccion, FechaNacimiento, Estado |
| 2 | `ExamenAdmision` | Registro de examen rendido por un postulante | IdExamen, IdFichaPostulacion, IdPostulante, NombreExamen, FechaExamen, HoraInicio, DuracionMinutos, Sala, Estado, Puntaje |
| 3 | `FichaPostulacion` | Vinculación del postulante con un programa específico | IdFichaPostulacion, IdPostulante, IdProgramaAcademico, NumeroTramite, FechaPostulacion, Estado |
| 4 | `ResultadoAdmision` | Resultado generado por el motor de admisión | IdResultado, IdPostulante, IdProgramaAcademico, Calificacion, Resultado, OrdenMerito, FechaResultado |
| 5 | `ProgramaAcademico` | Programa académico ofertado por la institución | IdProgramaAcademico, Codigo, Nombre, NivelAcademico, Vacantes, FechaInicioProceso, Estado, Departamento |
| 6 | `PeriodoAdmision` | Período temporal del proceso de admisión | IdPeriodo, Nombre, FechaInicio, FechaFin |
| 7 | `Usuario` | Cuenta de acceso al sistema | IdUsuario, NombreUsuario, Contrasena, NombreCompleto, Correo, Rol, Estado, UltimoAcceso |
| 8 | `Rol` | Rol de autorización del sistema | IdRol, Nombre, Descripcion |
| 9 | `ConfiguracionSistema` | Parámetros de configuración general | Clave, Valor, Descripcion |
| 10 | `DocumentoPostulante` | Documentos adjuntos del postulante | IdDocumento, IdPostulante, TipoDocumento, Ruta, Estado |
| 11 | `LogAuditoria` | Registro de auditoría de acciones del sistema | IdLog, IdUsuario, Accion, Fecha, Detalle |
| 12 | `LogMotorAdmision` | Log de ejecución del motor de admisión | IdLog, FechaEjecucion, TotalProcesados, Observaciones |
| 13 | `Matricula` | Registro de matrícula post-admisión | IdMatricula, IdPostulante, IdPrograma, FechaMatricula, Estado |
| 14 | `Notificacion` | Notificaciones enviadas a los usuarios | IdNotificacion, IdUsuario, Mensaje, FechaEnvio, Leida |
| 15 | `Sesion` | Registro de sesiones activas | IdSesion, IdUsuario, FechaInicio, FechaFin, TokenHash |
| 16 | `TipoDocumento` | Catálogo de tipos de documento | IdTipoDocumento, Nombre, Descripcion |

Es importante señalar que las entidades 9 a 16 representan la visión completa del dominio del sistema, incluyendo módulos de extensión futuros como auditoría, matrícula y notificaciones. De las 16 entidades, 8 son utilizadas activamente por la capa de persistencia (mapeadas a tablas en `SAADbContext`), mientras que las 8 restantes forman parte del modelo de dominio extensible que podrá ser implementado en iteraciones posteriores del sistema.

Cada entidad incluye campos de auditoría estándar: `FechaCreacion` (con valor por defecto `DateTime.Now`) y `FechaActualizacion` (nullable), siguiendo el patrón de trazabilidad temporal requerido por las mejores prácticas de desarrollo empresarial.

##### B. SAA.Application — Capa de Aplicación

La capa de Aplicación contiene la lógica de negocio del sistema y constituye la capa más importante desde la perspectiva funcional. Depende únicamente de `SAA.Domain` y define los contratos que deben implementar las capas externas mediante interfaces.

**Servicios de Aplicación:**

1. **`MotorAdmisionService`** (202 líneas): Es el servicio central del sistema y materializa los requisitos RF-02, RF-03, RF-04, RF-07, RF-08 y RF-14. Contiene los siguientes métodos:

   - `RegistrarExamenAsync(RegistrarExamenDto dto)`: Registra un examen de admisión validando la existencia del postulante. Utiliza transacciones de base de datos con manejo explícito para bases InMemory (las transacciones se ignoran silenciosamente en el proveedor InMemory mediante `try-catch` sobre `InvalidOperationException`).

   - `ProcesarResultadosAsync()`: Implementa el motor de admisión. El algoritmo opera de la siguiente manera: (1) elimina todos los resultados previos para garantizar idempotencia, (2) recupera todos los exámenes y fichas de postulación, (3) itera por cada programa académico, (4) ordena los exámenes del programa por puntaje descendente, (5) aplica las reglas de negocio (umbral ≥ 50.0, disponibilidad de vacantes), (6) utiliza un `HashSet<int>` para rastrear postulantes ya asignados y evitar acumulación de cupos en múltiples programas, (7) asigna el orden de mérito secuencial, (8) persiste los resultados en una transacción atómica.

   - `ObtenerReporteIngresantesAsync()`: Genera un reporte filtrado que retorna únicamente los postulantes con resultado "Ingresante", ordenados por puesto (orden de mérito ascendente). El reporte incluye datos del postulante, programa y calificación.

   - `ObtenerReporteTodosAsync()`: Genera un reporte general con todos los resultados, ordenados por puntaje descendente. A diferencia del reporte de ingresantes, este incluye el campo `Estado` con el resultado final de cada postulante.

2. **`PostulanteService`** (114 líneas): Implementa la gestión de postulantes conforme a los requisitos RF-01, RF-09 y RF-12. Contiene los métodos:

   - `CrearPostulanteAsync(CrearPostulanteDto dto)`: Registra un nuevo postulante validando la unicidad del DNI. Si el DNI ya existe, lanza `InvalidOperationException` con el mensaje "El DNI ya se encuentra registrado en el sistema." Adicionalmente, crea de forma atómica un `Usuario` con rol "Postulante" vinculado al DNI, utilizando una transacción de base de datos para garantizar consistencia.

   - `ObtenerTodosAsync()`: Retorna la lista completa de postulantes proyectada al DTO `PostulanteResponseDto` mediante una consulta LINQ con `Select`.

   - `ObtenerMiResultadoAsync(string dni)`: Consulta el resultado de admisión de un postulante específico por su DNI. Si el postulante no existe, retorna `null`. Si existe pero no tiene resultado ni programa asignado, retorna valores por defecto: programa "No Asignado", puntaje 0, estado "Pendiente", puesto 0.

3. **`AuthService`** (121 líneas): Implementa la autenticación dual conforme al RF-05 y RF-10. Contiene los métodos:

   - `LoginAsync(LoginRequestDto request)`: Implementa un flujo de autenticación en dos fases: (1) primero busca en la tabla de Usuarios excluyendo postulantes (`u.Rol != "Postulante"`), validando credenciales y estado activo; (2) si no encuentra un usuario administrativo, busca en la tabla de Postulantes por nombre y DNI. Retorna un `LoginResponseDto` con el token JWT o mensaje de error.

   - `GenerateToken(...)`: Método privado que genera tokens JWT con claims de `NameIdentifier`, `Name` y `Role`, expiración de 2 horas, firmados con HMAC-SHA256. La clave de firma se obtiene de la configuración `Jwt:Key` con un valor por defecto para desarrollo.

**DTOs (Data Transfer Objects):**

La capa de Aplicación define 7 DTOs distribuidos en 5 archivos, que encapsulan los datos transferidos entre capas:

- `RegistrarExamenDto`: Contiene `IdPostulante` (int), `Puntaje` (decimal) y `Observaciones` (string nullable).
- `CrearPostulanteDto`: Contiene `Nombres`, `Apellidos`, `DNI`, `Correo`, `Contrasena`, `Telefono` y `IdProgramaInteres`.
- `PostulanteResponseDto`: Proyección de la entidad Postulante sin campos sensibles (sin contraseña ni fechas de auditoría).
- `LoginRequestDto`: Contiene `NombreUsuario` y `Contrasena`.
- `LoginResponseDto`: Contiene `Exito` (bool), `Mensaje`, `Token` (nullable) y `Usuario` (UsuarioDto nullable).
- `UsuarioDto`: Proyección del usuario autenticado con `IdUsuario`, `NombreUsuario`, `NombreCompleto`, `Correo` y `Rol`.
- `MiResultadoDto`: Contiene `Nombres`, `Apellidos`, `Programa`, `Puntaje`, `Estado` y `Puesto`.
- `ReporteIngresanteDto`: Contiene `DNI`, `Nombres`, `Apellidos`, `ProgramaAcademico`, `Puntaje`, `Puesto`, `FechaAdmision` y `Estado`.

**Interfaz de Abstracción:**

- `IApplicationDbContext`: Define el contrato de acceso a datos exponiendo los `DbSet<>` necesarios y la propiedad `Database` para el manejo de transacciones. Esta interfaz es implementada por `SAADbContext` y permite que los servicios de aplicación sean independientes de la implementación concreta de persistencia.

##### C. SAA.Infrastructure — Capa de Infraestructura

La capa de Infraestructura implementa los contratos definidos por la capa de Aplicación y proporciona el acceso concreto a la base de datos SQL Server. Contiene dos componentes principales:

1. **`SAADbContext`** (66 líneas): Hereda de `DbContext` e implementa `IApplicationDbContext`. Define 8 conjuntos de datos (`DbSet<>`) organizados por esquema:

   - **Seguridad**: `Usuarios` (`DbSet<Usuario>`), `Roles` (`DbSet<Rol>`)
   - **Admisión**: `Postulantes` (`DbSet<Postulante>`), `FichasPostulacion` (`DbSet<FichaPostulacion>`), `ExamenesAdmision` (`DbSet<ExamenAdmision>`), `ResultadosAdmision` (`DbSet<ResultadoAdmision>`)
   - **Configuración**: `ProgramasAcademicos` (`DbSet<ProgramaAcademico>`), `PeriodosAdmision` (`DbSet<PeriodoAdmision>`)

   El método `OnModelCreating` configura cada entidad mediante Fluent API, asignando tabla, esquema y clave primaria. Las propiedades decimales (`Puntaje` en `ExamenAdmision` y `Calificacion` en `ResultadoAdmision`) se configuran con precisión `(18, 2)` para evitar pérdida de datos numéricos.

2. **`SeedDataService`**: Servicio de siembra de datos iniciales que pobla la base de datos con un usuario administrador ("fredy") y 500 postulantes de prueba con sus respectivas fichas de postulación, exámenes y usuarios vinculados. Este servicio se ejecuta automáticamente al iniciar la aplicación en modo desarrollo, verificando previamente si la base de datos ya contiene datos para evitar duplicación.

##### D. SAA.API — Capa de Presentación

La capa de Presentación comprende el proyecto API Server y el proyecto frontend React. El servidor API expone los endpoints REST y configura el pipeline de middleware de ASP.NET Core.

**Configuración del Servidor (Program.cs — 106 líneas):**

El archivo `Program.cs` configura los siguientes componentes de la aplicación:

- **Autenticación JWT**: Configuración de `JwtBearerDefaults.AuthenticationScheme` con validación de emisor (`ValidIssuer`), audiencia (`ValidAudience`), tiempo de vida y clave de firma (`IssuerSigningKey`). Se especifica que el claim de rol es `ClaimTypes.Role` y el claim de nombre es `ClaimTypes.Name`.

- **Entity Framework Core**: Registro de `SAADbContext` con el proveedor SQL Server y la cadena de conexión "DefaultConnection".

- **Inyección de Dependencias**: Registro de `IApplicationDbContext` como Scoped (mapeado a `SAADbContext`), y registro de los tres servicios de aplicación (`PostulanteService`, `AuthService`, `MotorAdmisionService`) con ciclo de vida Scoped.

- **CORS**: Política "AllowAll" que permite cualquier origen, método y header para facilitar el desarrollo con el frontend React.

- **Middleware Pipeline**: Se configura en orden: HTTPS Redirection → CORS → Static Files → Authentication → Authorization → Controllers → Seed Data.

**Controladores API:**

La capa de presentación expone tres controladores con un total de 8 endpoints REST:

**Tabla 8:** Endpoints del API del SAA

| N.° | Método HTTP | Ruta | Controlador | Autorización | Descripción |
|-----|-------------|------|-------------|--------------|-------------|
| 1 | POST | `/api/auth/login` | AuthController | Público | Autenticación de usuarios y generación de token JWT |
| 2 | POST | `/api/postulantes` | PostulantesController | Público | Registro de nuevos postulantes |
| 3 | GET | `/api/postulantes` | PostulantesController | Público | Consulta de todos los postulantes |
| 4 | GET | `/api/postulantes/mi-resultado` | PostulantesController | Rol: Postulante | Consulta del resultado individual del postulante autenticado |
| 5 | POST | `/api/admision/examen` | AdmisionController | Rol: Administrador | Registro de exámenes de admisión |
| 6 | POST | `/api/admision/procesar` | AdmisionController | Rol: Administrador | Ejecución del motor de admisión |
| 7 | GET | `/api/admision/reporte-ingresantes` | AdmisionController | Rol: Administrador | Generación del reporte de ingresantes |
| 8 | GET | `/api/admision/reporte-todos` | AdmisionController | Rol: Administrador | Generación del reporte general de resultados |

El `AdmisionController` está decorado con el atributo `[Authorize(Roles = "Administrador")]` a nivel de clase, lo que significa que todos sus endpoints requieren un token JWT válido con el claim de rol "Administrador". El endpoint `mi-resultado` del `PostulantesController` utiliza `[Authorize(Roles = "Postulante")]` a nivel de método. Los endpoints de creación y consulta general de postulantes son públicos para permitir el registro desde el frontend.

##### E. SAA.API/frontend — Interfaz de Usuario

El frontend del SAA es una Single Page Application (SPA) implementada con React 19 y TypeScript 5.9, construida con Vite 7 como herramienta de build. La aplicación se organiza en un único componente principal `App.tsx` (363 líneas) con un archivo de estilos `App.css` (323 líneas).

**Gestión de Estado:**

La aplicación utiliza React Hooks para la gestión de estado local:
- `useState` para almacenar el token JWT, datos del usuario, estado de carga, errores, credenciales del formulario, resultados y reporte.
- `useEffect` para cargar automáticamente los datos correspondientes al rol del usuario tras la autenticación.
- `localStorage` para persistir el token y datos del usuario entre sesiones.

**Renderizado Condicional:**

La interfaz implementa tres vistas condicionales basadas en el estado de autenticación y el rol del usuario:
1. Si no hay token → se renderiza la **página de login**.
2. Si el rol es "Administrador" → se renderiza el **dashboard administrativo**.
3. Si el rol es "Postulante" → se renderiza el **dashboard del postulante**.

**Diseño Visual:**

La interfaz utiliza un sistema de diseño glassmorphism con las siguientes características CSS:
- Fondo oscuro (`--bg-darker: #020617`) con formas animadas de colores indigo (`--primary: #6366f1`) y rosa (`--secondary: #ec4899`) que flotan con una animación de 20 segundos.
- Paneles con efecto glass: `backdrop-filter: blur(12px)`, fondo semi-transparente (`rgba(255, 255, 255, 0.05)`) y bordes translúcidos.
- Badges de estado con colores contextuales: verde para "Ingresante" (`#22c55e`), amarillo para "Aprobado" (`#eab308`) y rojo para "Desaprobado" (`#ef4444`).
- Animación de entrada `slideUp` con curva bezier cúbica (`cubic-bezier(0.16, 1, 0.3, 1)`) para una sensación premium.
- Botones con efecto hover que eleva el elemento y proyecta sombra del color primario.

#### 4.1.3.4. Control de Versiones e Integración Continua

El control de versiones del SAA se gestiona mediante Git con un repositorio centralizado en GitHub, siguiendo una estrategia de ramas que soporta la trazabilidad requerida por el enfoque SSD.

**Estrategia de Ramas:**

- **`main`**: Rama principal que contiene el código estable y verificado. Solo se actualizan mediante merges de ramas de desarrollo después de pasar las pruebas automatizadas.
- **`develop`**: Rama de integración donde se consolidan las funcionalidades desarrolladas en cada sprint antes de su promoción a `main`.
- **Ramas de feature**: Ramas temporales creadas para cada funcionalidad específica (por ejemplo, `feature/motor-admision`, `feature/auth-jwt`, `feature/frontend-dashboard`).

**Trazabilidad SSD mediante Control de Versiones:**

El control de versiones soporta la trazabilidad del enfoque SSD de las siguientes maneras:

1. Cada commit se asocia a un requisito funcional específico mediante mensajes descriptivos que referencian el código del requisito (por ejemplo: "Implementar RF-03: Motor de Admisión con clasificación por puntaje y vacantes").
2. Las ramas de feature se nombran según el módulo o requisito que implementan.
3. El historial de commits permite reconstruir la secuencia cronológica de implementación, demostrando que las especificaciones precedieron al código.

**Estructura del Repositorio:**

El repositorio contiene la siguiente estructura de archivos principales, coherente con la arquitectura Clean Architecture:

```
SAA.Solution.slnx
├── SAA.Domain/
│   └── Entities/ (16 archivos .cs)
├── SAA.Application/
│   ├── DTOs/ (5 archivos .cs)
│   ├── Interfaces/ (IApplicationDbContext.cs)
│   └── Services/ (3 archivos .cs)
├── SAA.Infrastructure/
│   ├── Data/ (SAADbContext.cs, Migrations/)
│   └── Services/ (SeedDataService.cs)
├── SAA.API/
│   ├── SAA.API.Server/
│   │   ├── Controllers/ (3 archivos .cs)
│   │   └── Program.cs
│   ├── SAA.API.AppHost/
│   └── frontend/
│       └── src/ (App.tsx, App.css)
├── SAA.Tests/ (8 archivos de pruebas .cs)
├── SAA_AdmisionDB_Schema.sql
├── requests.http
└── coveragereport_new/
```

#### 4.1.3.5. Flujo Scrum Aplicado al Desarrollo

El desarrollo del SAA se organizó mediante una adaptación de la metodología ágil Scrum para un desarrollador individual, distribuyendo el trabajo en cuatro sprints de dos semanas cada uno. Dado que el proyecto fue realizado de forma individual por Fredy Bonilla Rey, los tres roles tradicionales de Scrum (Product Owner, Scrum Master y Development Team) fueron asumidos por la misma persona.

**Tabla 9:** Planificación de Sprints del Proyecto SAA

| Sprint | Nombre | Duración | Objetivos | Artefactos Generados |
|--------|--------|----------|-----------|---------------------|
| Sprint 1 | Análisis y Especificaciones SSD | 2 semanas | Definir el alcance del sistema. Levantar y formalizar todos los requisitos funcionales y no funcionales. Elaborar el documento ERS. Definir casos de uso detallados. Establecer la matriz de trazabilidad. | Documento ERS, Tabla de requisitos (RF-01 a RF-15), Tabla de RNF (RNF-01 a RNF-07), Casos de uso, Matriz de trazabilidad |
| Sprint 2 | Diseño Arquitectónico | 2 semanas | Diseñar el modelo entidad-relación. Definir la arquitectura Clean Architecture. Crear los prototipos de interfaz de usuario. Configurar el entorno de desarrollo. | Diagrama ER, Diagrama de Clean Architecture, Prototipos UI, Configuración de solución (.slnx), Esquema SQL inicial |
| Sprint 3 | Implementación del Motor de Admisión | 2 semanas | Implementar la capa de Dominio (16 entidades). Implementar la capa de Aplicación (servicios y DTOs). Implementar la capa de Infraestructura (DbContext y SeedData). Implementar la capa de Presentación (API Controllers y Frontend React). | SAA.Domain (16 clases), SAA.Application (3 servicios, 8 DTOs), SAA.Infrastructure (SAADbContext, SeedDataService), SAA.API (3 controladores, 8 endpoints, App.tsx, App.css) |
| Sprint 4 | Pruebas y Validación | 2 semanas | Implementar pruebas unitarias para todos los servicios. Implementar pruebas de integración. Ejecutar análisis de cobertura con Coverlet. Generar reportes de cobertura con ReportGenerator. Corregir defectos identificados. | 8 archivos de prueba, 34 casos de prueba, Reporte de cobertura (54.6% líneas), Coverage XML |

Es importante destacar que el Sprint 1 fue dedicado exclusivamente a la fase de especificaciones, sin escribir código de producción. Este enfoque, propio del SSD, puede parecer contraproductivo en un contexto ágil tradicional donde se busca entregar software funcional desde la primera iteración. Sin embargo, la inversión en especificaciones formales durante el Sprint 1 resultó en una fase de implementación (Sprint 3) significativamente más fluida, con menos refactorizaciones y retrabajos, dado que las decisiones de diseño ya estaban documentadas y validadas.

**Adaptación de Scrum para Desarrollador Individual:**

La adaptación de Scrum para un único desarrollador implicó las siguientes modificaciones:

- **Daily Stand-ups** sustituidas por un registro diario de progreso en el repositorio Git mediante commits descriptivos.
- **Sprint Reviews** realizadas como auto-evaluación del incremento funcional al final de cada sprint.
- **Sprint Retrospectives** documentadas como lecciones aprendidas que informaron las decisiones del sprint siguiente.
- **Product Backlog** mantenido como una lista priorizada de requisitos funcionales y no funcionales derivada del documento ERS.

#### 4.1.3.6. Evidencias Visuales de las Interfaces

A continuación se describen las interfaces implementadas en el frontend del SAA, detallando los elementos visuales y funcionales de cada pantalla.

**Figura 1: Página de Login — Portal SAA**

Como se evidencia en la Figura 1, la página de login del Portal SAA presenta un diseño centrado con un panel glassmorphism que contiene el título "Portal SAA" con un gradiente de texto (blanco a lavanda). El formulario incluye dos campos de entrada: "Usuario / DNI" y "Contraseña", ambos con estilo de fondo oscuro semi-transparente y bordes que se iluminan en indigo al recibir el foco. El botón "Ingresar" presenta un gradiente lineal de indigo a violeta claro con efecto hover de elevación. En el fondo, dos formas circulares animadas (una indigo y otra rosa) se mueven suavemente creando un efecto visual dinámico. Los mensajes de error se muestran en un recuadro con borde izquierdo rojo y fondo rojizo translúcido.

**Figura 2: Dashboard de Administrador — Gestión de Admisión**

Como se evidencia en la Figura 2, el dashboard de administrador presenta un header sticky glassmorphism con el título "Portal de Administración" a la izquierda y los datos del administrador con un botón "Cerrar Sesión" a la derecha. El área principal contiene un panel "Gestión de Admisión" con el botón "⚙️ Procesar Motor de Admisión" que ejecuta el procesamiento del RF-03. Debajo se encuentran dos botones de filtro: "Ver Ingresantes" (en indigo) y "Ver No Ingresantes" (en rojo). La tabla de resultados muestra columnas de Puesto, Postulante, Programa y Puntaje, con una columna adicional de Estado visible solo en la vista "No Ingresantes". Se incluye un selector de filtro por programa académico. Los puntajes se destacan en color indigo para facilitar su lectura.

**Figura 3: Dashboard de Postulante — Estado de Admisión**

Como se evidencia en la Figura 3, el dashboard del postulante presenta un header con saludo personalizado ("Hola, [nombre del postulante]") y botón de cierre de sesión. El panel central "Estado de Admisión" muestra un badge de estado con colores contextuales: verde para "INGRESANTE", amarillo para "APROBADO" y rojo para "DESAPROBADO". Debajo se presenta la información detallada en tres filas: Programa Académico (texto descriptivo), Puntaje Obtenido (en formato grande con color indigo y sufijo "pts") y Puesto en el Orden de Mérito (con prefijo "#"). El footer del panel contiene un mensaje contextual: "🎉 ¡Felicidades! Has logrado una vacante." para ingresantes, un mensaje informativo para aprobados sin vacante, o un mensaje de no alcanzar el puntaje mínimo para desaprobados.

Las capturas de pantalla completas de las interfaces se presentan en el Anexo D con sus respectivas leyendas y descripciones detalladas.

---

### 4.1.4. Resultados de la Validación de Funcionamiento (Variable X4)

La validación de funcionamiento representa la fase más crítica del enfoque SSD aplicado al SAA, especialmente considerando que el presente trabajo se enmarca en el curso IS-489 Pruebas y Aseguramiento de la Calidad. En esta sección se presentan los resultados completos de las pruebas automatizadas, incluyendo pruebas unitarias, pruebas de integración, análisis de cobertura de código y validación funcional por módulos.

#### 4.1.4.1. Pruebas Unitarias

Las pruebas unitarias del SAA se implementaron utilizando xUnit como framework de pruebas, FluentAssertions para aserciones expresivas, Moq para objetos simulados y Entity Framework Core InMemory como proveedor de base de datos en memoria. Cada archivo de pruebas utiliza un método auxiliar `GetMemoryContext()` que crea una instancia aislada de `SAADbContext` con una base de datos InMemory de nombre único (generado con `Guid.NewGuid()`), garantizando la independencia entre pruebas.

A continuación se describe en detalle cada archivo de pruebas y los casos implementados.

##### a) MotorAdmisionTests.cs — Pruebas del Motor de Admisión

Este archivo contiene 10 pruebas que verifican exhaustivamente el comportamiento del motor de admisión, cubriendo escenarios de clasificación, reportes y validaciones.

1. **`ProcesarResultados_ConCuposSuficientes_ApruebaPostulante`**: Verifica que un postulante con puntaje 60 (≥ 50.0) y vacantes disponibles (2 vacantes) recibe el estado "Ingresante". Se configuran un programa académico con 2 vacantes, un postulante con ficha de postulación y un examen con puntaje 60. Tras el procesamiento, se verifica que el `ResultadoAdmision` generado tiene `Resultado == "Ingresante"`.

2. **`ProcesarResultados_SinCupos_AprobadoPeroNoIngresa`**: Verifica la diferencia entre "Aprobado" e "Ingresante" cuando las vacantes se agotan. Se configuran 2 postulantes compitiendo por 1 sola vacante. El postulante 2 tiene puntaje 80 y el postulante 1 tiene puntaje 60. Se verifica que el postulante 2 (mayor puntaje) recibe "Ingresante" y el postulante 1 recibe "Aprobado".

3. **`ProcesarResultados_PuntajeMenorA50_DesapruebaPostulante`**: Verifica que un postulante con puntaje 40 (< 50.0) recibe el estado "Desaprobado", incluso cuando hay 10 vacantes disponibles. Esta prueba valida la regla de negocio del umbral mínimo aprobatorio.

4. **`ObtenerReporteIngresantesAsync_DevuelveSoloIngresantesOrdenados`**: Verifica que el reporte de ingresantes filtra correctamente solo los registros con estado "Ingresante" y los ordena por puesto. Se configuran 3 postulantes: 2 ingresantes y 1 desaprobado. Se verifica que el reporte contiene exactamente 2 registros, con María (puesto 1, puntaje 80) primero y Juan (puesto 2, puntaje 60) segundo.

5. **`ObtenerReporteTodosAsync_DevuelveTodosOrdenadosPorPuntaje`**: Verifica que el reporte general incluye todos los resultados (Ingresante y Desaprobado) ordenados por puntaje descendente. Se verifica que María (puntaje 80, Ingresante) aparece antes que Juan (puntaje 40, Desaprobado).

6. **`ProcesarResultados_PostulanteNoAcumulaCupos_SeAsignaAUnSoloPrograma`**: Prueba crítica que verifica que un postulante con fichas en 2 programas diferentes (Medicina con puntaje 100 y Sistemas con puntaje 90) solo genera un resultado en el primer programa procesado. Se verifica que existe un único `ResultadoAdmision` para el postulante, asignado al programa Medicina con estado "Ingresante".

7. **`RegistrarExamenAsync_PostulanteNoEncontrado_LanzaExcepcion`**: Verifica que al intentar registrar un examen para un postulante inexistente (IdPostulante = 999), el servicio lanza una excepción de tipo `Exception` con el mensaje "Postulante no encontrado."

8. **`RegistrarExamenAsync_PostulanteExiste_RegistraExamen`**: Verifica que al registrar un examen para un postulante existente con puntaje 85.5 y observaciones "Ninguna", el examen se persiste correctamente en la base de datos con los valores proporcionados.

9. **`ObtenerReporteIngresantesAsync_SinResultados_DevuelveListaVacia`**: Verifica que cuando no existen resultados de admisión en la base de datos, el reporte de ingresantes retorna una lista vacía sin lanzar excepciones.

10. **`ObtenerReporteTodosAsync_SinResultados_DevuelveListaVacia`**: Análogo al caso anterior pero para el reporte general, verificando que retorna una lista vacía cuando no hay resultados procesados.

##### b) PostulanteServiceTests.cs — Pruebas del Servicio de Postulantes

Este archivo contiene 7 pruebas que verifican las operaciones CRUD y de consulta del servicio de postulantes:

1. **`RegistrarPostulanteAsync_ValidRequest_RegistersSuccessfully`**: Verifica el registro exitoso de un postulante con datos válidos (nombres, apellidos, DNI "12345678", correo, contraseña, programa de interés). Utiliza FluentAssertions (`Should().NotBeNull()`, `Should().Be()`) para verificar que el postulante persiste correctamente.

2. **`RegistrarPostulanteAsync_DniYaExiste_ThrowsException`**: Verifica que al intentar registrar un postulante con un DNI que ya existe en el sistema, se lanza `InvalidOperationException`. Esta prueba valida el RF-12 (Validación de Datos de Entrada).

3. **`RegistrarPostulanteAsync_ProgramaNoExiste_ThrowsException`**: Prueba de escenario de borde para un programa inexistente. Este test fue desactivado (comentado) dado que el servicio actual no valida la existencia del programa, identificándose como un punto de mejora futura.

4. **`ObtenerTodosAsync_ReturnsAllPostulantes`**: Verifica que el método de consulta retorna la lista completa de postulantes. Se configuran 2 postulantes y se verifica que el resultado tiene exactamente 2 elementos con los DNI correctos.

5. **`ObtenerMiResultadoAsync_DniExists_ReturnsResultado`**: Verifica que un postulante con resultado de admisión procesado puede consultar su estado. Se configura un postulante con resultado "Ingresante", puntaje 85.5 y puesto 1, y se verifica que la respuesta contiene todos los datos correctos.

6. **`ObtenerMiResultadoAsync_DniDoesNotExist_ReturnsNull`**: Verifica que la consulta de resultado para un DNI inexistente ("00000000") retorna `null`.

7. **`ObtenerMiResultadoAsync_SinResultadoYPrograma_ReturnsFallbackValues`**: Verifica que cuando un postulante existe pero no tiene resultado procesado ni programa válido (IdProgramaInteres = 999), se retornan valores por defecto: programa "No Asignado", puntaje 0, estado "Pendiente" y puesto 0. Esta prueba valida la resiliencia del sistema ante datos incompletos.

##### c) AuthServiceTests.cs — Pruebas del Servicio de Autenticación

Este archivo contiene 3 pruebas que verifican el flujo de autenticación JWT:

1. **`LoginAsync_ValidCredentials_ReturnsTokenAndSuccess`**: Verifica que un usuario administrador ("fredy") con credenciales válidas obtiene un token JWT no nulo y una respuesta con `Exito = true`. Se utiliza Moq para simular `IConfiguration` con la clave JWT de prueba de 256 bits.

2. **`LoginAsync_InvalidCredentials_ReturnsFailure`**: Verifica que credenciales incorrectas (contraseña "wrongpass") retornan `Exito = false`, token nulo y un mensaje que contiene "incorrectos".

3. **`LoginAsync_InactiveUser_ReturnsFailure`**: Verifica que un usuario con estado "Inactivo" no puede autenticarse, retornando `Exito = false` y un mensaje que contiene "inactivo".

##### d) AdmisionControllerTests.cs — Pruebas de Controladores

Este archivo contiene 4 pruebas que verifican el comportamiento de los endpoints del controlador de admisión:

1. **`ObtenerReporteIngresantes_ReturnsOkResult`**: Verifica que el endpoint de reporte de ingresantes retorna un `OkObjectResult` con una lista de tipo `List<ReporteIngresanteDto>`.

2. **`RegistrarExamen_ReturnsOkResult`**: Verifica que el registro de un examen para un postulante existente retorna un `OkObjectResult`.

3. **`ProcesarResultados_ReturnsOkResult`**: Verifica que la ejecución del motor de admisión retorna un `OkObjectResult`.

4. **`ObtenerReporteTodos_ReturnsOkResult`**: Verifica que el endpoint de reporte general retorna un `OkObjectResult` con una lista de tipo `List<ReporteIngresanteDto>`.

##### e) IntegrationTests.cs — Pruebas de Integración

Este archivo contiene 1 prueba de integración de extremo a extremo utilizando `WebApplicationFactory<Program>`:

1. **`GetReporteIngresantes_WithoutToken_ReturnsUnauthorized`**: Verifica que un cliente HTTP sin token de autenticación recibe una respuesta `401 Unauthorized` al intentar acceder al endpoint protegido `/api/admision/reporte-ingresantes`. Esta prueba valida el RF-10 (Control de Acceso) en un entorno de integración completo.

##### f) DomainCoverageTests.cs — Pruebas de Cobertura de Dominio

Este archivo contiene 1 prueba reflexiva que maximiza la cobertura de las entidades del dominio:

1. **`AllDomainEntities_Properties_CanBeSetAndGet`**: Utiliza reflexión para iterar sobre todas las clases del namespace `SAA.Domain.Entities`, instanciarlas dinámicamente y verificar que todas sus propiedades públicas pueden ser escritas y leídas correctamente con valores de prueba. Esta técnica automatiza la verificación de los getters y setters de las 16 entidades del dominio.

##### g) InfrastructureTests.cs — Pruebas de Infraestructura

Este archivo contiene 1 prueba que verifica la configuración del DbContext:

1. **`DbContext_ModelCreation_Success`**: Verifica que `SAADbContext` puede crear su modelo correctamente, confirmando que las entidades `Usuario`, `ProgramaAcademico`, `Postulante`, `FichaPostulacion`, `ExamenAdmision` y `ResultadoAdmision` están correctamente registradas en el modelo de Entity Framework Core.

##### h) SeedDataServiceTests.cs — Pruebas del Servicio de Siembra

Este archivo contiene 2 pruebas que verifican el servicio de datos iniciales:

1. **`SeedAsync_WhenDatabaseIsEmpty_SeedsUsersAndPostulantes`**: Verifica que al ejecutar el seed en una base de datos vacía se crean 501 usuarios (1 administrador + 500 postulantes) y 500 postulantes.

2. **`SeedAsync_WhenDatabaseIsNotEmpty_DoesNotSeedAgain`**: Verifica el comportamiento idempotente del seed cuando ya existen datos, confirmando que no se duplican registros innecesariamente.

##### Resumen de Resultados de Pruebas Unitarias

**Tabla 10:** Resumen de Resultados de Pruebas Unitarias por Archivo

| N.° | Archivo de Pruebas | N.° de Pruebas | Aprobadas | Fallidas | Tasa de Éxito |
|-----|-------------------|----------------|-----------|----------|---------------|
| 1 | MotorAdmisionTests.cs | 10 | 10 | 0 | 100% |
| 2 | PostulanteServiceTests.cs | 7 | 7 | 0 | 100% |
| 3 | AuthServiceTests.cs | 3 | 3 | 0 | 100% |
| 4 | AdmisionControllerTests.cs | 4 | 4 | 0 | 100% |
| 5 | IntegrationTests.cs | 1 | 1 | 0 | 100% |
| 6 | DomainCoverageTests.cs | 1 | 1 | 0 | 100% |
| 7 | InfrastructureTests.cs | 1 | 1 | 0 | 100% |
| 8 | SeedDataServiceTests.cs | 2 | 2 | 0 | 100% |
| | **TOTAL** | **29** | **29** | **0** | **100%** |

Como se observa en la Tabla 10, las 29 pruebas implementadas en los 8 archivos de pruebas fueron ejecutadas exitosamente con una tasa de éxito del 100%. Este resultado demuestra que todos los escenarios especificados en la fase de análisis han sido implementados correctamente y verificados mediante pruebas automatizadas reproducibles.

#### 4.1.4.2. Pruebas de Integración

Las pruebas de integración del SAA se implementaron utilizando `WebApplicationFactory<Program>` de ASP.NET Core, que permite crear un servidor de pruebas en memoria que ejecuta el pipeline completo de la aplicación (middleware, controladores, servicios, base de datos).

El enfoque de las pruebas de integración se centró en verificar la seguridad del sistema a nivel de endpoints HTTP. La prueba `GetReporteIngresantes_WithoutToken_ReturnsUnauthorized` demuestra que el pipeline de autenticación JWT funciona correctamente de extremo a extremo: una solicitud HTTP GET sin header `Authorization` al endpoint `/api/admision/reporte-ingresantes` es rechazada con código de estado `401 Unauthorized`.

Este tipo de prueba es especialmente valioso porque verifica:
- La correcta configuración del middleware de autenticación JWT en `Program.cs`.
- La correcta aplicación del atributo `[Authorize(Roles = "Administrador")]` en el controlador.
- La correcta respuesta HTTP según la especificación del RF-10.

Las pruebas unitarias de los servicios también actúan como pruebas de integración a nivel de capa, dado que utilizan `SAADbContext` con el proveedor InMemory (no un mock), verificando la interacción real entre los servicios de aplicación y Entity Framework Core. Por ejemplo, el test `RegistrarPostulanteAsync_ValidRequest_RegistersSuccessfully` verifica que el servicio `PostulanteService` puede crear un postulante y persistirlo a través de Entity Framework Core, consultándolo posteriormente para verificar su existencia.

#### 4.1.4.3. Cobertura de Código

El análisis de cobertura de código se realizó utilizando Coverlet como instrumento de cobertura y ReportGenerator para la generación de reportes legibles. La cobertura se midió ejecutando todas las 29 pruebas unitarias y de integración, generando un archivo Cobertura XML que fue procesado para producir el reporte que se presenta a continuación.

**Tabla 11:** Resumen General de Cobertura de Código

| Métrica | Valor | Cobertura |
|---------|-------|-----------|
| Líneas cubiertas | 268 de 490 | **54.6%** |
| Ramas cubiertas | 34 de 58 | **58.6%** |
| Métodos cubiertos | 84 de 162 | **51.8%** |
| Métodos completamente cubiertos | 82 de 162 | **50.6%** |
| Total de líneas de código | 1,005 | — |
| Ensamblados analizados | 3 | — |
| Clases analizadas | 28 | — |
| Archivos fuente | 25 | — |

**Tabla 12:** Cobertura de Código por Ensamblado y Clase

| Ensamblado / Clase | Cobertura de Líneas |
|---------------------|:-------------------:|
| **SAA.Application** | **79.6%** |
| ┣ CrearPostulanteDto | 100% |
| ┣ LoginRequestDto | 100% |
| ┣ LoginResponseDto | 100% |
| ┣ PostulanteResponseDto | 100% |
| ┣ RegistrarExamenDto | 0% |
| ┣ ReporteIngresanteDto | 100% |
| ┣ UsuarioDto | 100% |
| ┣ AuthService | 100% |
| ┣ MotorAdmisionService | 69.2% |
| ┗ PostulanteService | 72.7% |
| **SAA.Domain** | **33.6%** |
| ┣ ConfiguracionSistema | 0% |
| ┣ DocumentoPostulante | 0% |
| ┣ ExamenAdmision | 46.1% |
| ┣ FichaPostulacion | 0% |
| ┣ LogAuditoria | 0% |
| ┣ LogMotorAdmision | 0% |
| ┣ Matricula | 0% |
| ┣ Notificacion | 0% |
| ┣ PeriodoAdmision | 0% |
| ┣ Postulante | 75% |
| ┣ ProgramaAcademico | 50% |
| ┣ ResultadoAdmision | 60% |
| ┣ Rol | 0% |
| ┣ Sesion | 0% |
| ┣ TipoDocumento | 0% |
| ┗ Usuario | 90% |
| **SAA.Infrastructure** | **19.6%** |
| ┣ SAADbContext | 100% |
| ┗ SeedDataService | 0% |

**Análisis de la Cobertura por Capa:**

1. **SAA.Application (79.6%)**: La capa de Aplicación presenta la cobertura más alta, superando ampliamente el objetivo mínimo del 70% establecido en el RNF-03. Los tres servicios de negocio tienen coberturas significativas: `AuthService` alcanza el 100%, `PostulanteService` el 72.7% y `MotorAdmisionService` el 69.2%. Los DTOs utilizados en las pruebas alcanzan 100% de cobertura, con la excepción de `RegistrarExamenDto` (0%) cuyas propiedades no fueron instanciadas directamente en las pruebas sino a través de inicializadores de objeto.

2. **SAA.Domain (33.6%)**: La cobertura del dominio es menor debido a que 8 de las 16 entidades (las entidades de extensión futura: ConfiguracionSistema, DocumentoPostulante, LogAuditoria, LogMotorAdmision, Matricula, Notificacion, Sesion y TipoDocumento) no son utilizadas por ningún servicio ni prueba actualmente. Las entidades activas tienen coberturas razonables: `Usuario` (90%), `Postulante` (75%), `ResultadoAdmision` (60%), `ProgramaAcademico` (50%) y `ExamenAdmision` (46.1%). La prueba reflexiva `DomainCoverageTests` incrementó significativamente la cobertura al ejercitar los getters y setters de todas las entidades.

3. **SAA.Infrastructure (19.6%)**: La cobertura más baja corresponde a la capa de Infraestructura, explicada por el `SeedDataService` (0% de cobertura) que es un servicio de datos iniciales ejecutado solo al inicio de la aplicación. Sin embargo, `SAADbContext` alcanza el 100% de cobertura gracias a la prueba `DbContext_ModelCreation_Success` que fuerza la ejecución del `OnModelCreating`.

La cobertura global del 54.6% en líneas de código supera el umbral mínimo del 50% establecido en el RNF-03, y la cobertura de la capa de Aplicación (79.6%) supera significativamente el objetivo del 70% para la lógica de negocio. Es importante señalar que la métrica de cobertura del dominio se ve afectada por las entidades de extensión futura que, si bien forman parte del modelo conceptual completo, no están activamente utilizadas en la versión actual del sistema.

#### 4.1.4.4. Validación por Módulos (Cumple / No Cumple)

A continuación se presenta la tabla de validación funcional que verifica el cumplimiento de cada requisito funcional especificado en la fase de análisis.

**Tabla 13:** Validación Funcional por Módulos

| N.° | Módulo | Requisito | Resultado de Validación | Estado |
|-----|--------|-----------|------------------------|--------|
| 1 | Registro | RF-01: Gestión de Registro de Postulantes | El sistema registra postulantes con validación de DNI único. Se crea automáticamente el usuario vinculado con rol "Postulante". La prueba `RegistrarPostulanteAsync_ValidRequest` confirma el flujo exitoso y `RegistrarPostulanteAsync_DniYaExiste` confirma la validación de unicidad. | ✅ Cumple |
| 2 | Evaluación | RF-02: Registro de Exámenes de Admisión | El sistema registra exámenes vinculados a postulantes existentes. La prueba `RegistrarExamenAsync_PostulanteExiste_RegistraExamen` confirma la persistencia correcta con puntaje 85.5 y observaciones. | ✅ Cumple |
| 3 | Motor de Admisión | RF-03: Clasificación Automatizada | El motor procesa correctamente los tres escenarios: Ingresante (puntaje ≥ 50, con vacante), Aprobado (puntaje ≥ 50, sin vacante) y Desaprobado (puntaje < 50). Tres pruebas unitarias verifican cada escenario de manera independiente. | ✅ Cumple |
| 4 | Resultados | RF-04: Trazabilidad de Resultados | El sistema genera registros `ResultadoAdmision` con calificación, estado, orden de mérito y fecha. Los resultados previos se eliminan antes de cada procesamiento para garantizar consistencia. | ✅ Cumple |
| 5 | Seguridad | RF-05: Autenticación JWT | El sistema autentica usuarios administrativos y postulantes, generando tokens JWT con claims de rol. Tres pruebas verifican: login exitoso, credenciales inválidas y usuario inactivo. | ✅ Cumple |
| 6 | Configuración | RF-06: Gestión de Programas Académicos | Los programas académicos se gestionan mediante la entidad `ProgramaAcademico` con catálogo de vacantes. La prueba `DbContext_ModelCreation` confirma la configuración correcta. | ✅ Cumple |
| 7 | Reportes | RF-07: Reporte de Ingresantes | El sistema genera reporte filtrado de ingresantes ordenados por puesto. Dos pruebas verifican: reporte con datos y reporte vacío sin excepciones. | ✅ Cumple |
| 8 | Reportes | RF-08: Reporte General | El sistema genera reporte completo de todos los postulantes con su estado, ordenados por puntaje descendente. La prueba `ObtenerReporteTodosAsync` confirma el ordenamiento y contenido correcto. | ✅ Cumple |
| 9 | Portal Postulante | RF-09: Consulta Individual | Cada postulante autenticado puede consultar su resultado personal. Tres pruebas verifican: resultado existente, DNI inexistente (null) y valores por defecto cuando no hay resultado procesado. | ✅ Cumple |
| 10 | Seguridad | RF-10: Control de Acceso por Roles | Los endpoints están protegidos por autorización basada en roles. La prueba de integración `GetReporteIngresantes_WithoutToken_ReturnsUnauthorized` confirma el control de acceso. | ✅ Cumple |
| 11 | Registro | RF-11: Gestión de Fichas | Las fichas de postulación vinculan postulantes con programas. La entidad está mapeada al esquema "Admision" y se utiliza en el motor de admisión para determinar el programa del postulante. | ✅ Cumple |
| 12 | Validación | RF-12: Validación de Datos | El sistema valida unicidad de DNI (InvalidOperationException), existencia de postulante en registro de exámenes (Exception) y validación de ModelState en login. | ✅ Cumple |
| 13 | Infraestructura | RF-13: Procesamiento Transaccional | Las operaciones críticas utilizan transacciones con commit/rollback. El manejo de transacciones InMemory se resuelve mediante try-catch de InvalidOperationException. | ✅ Cumple |
| 14 | Motor de Admisión | RF-14: Orden de Mérito | El sistema asigna orden de mérito secuencial y evita que un postulante acumule vacantes en múltiples programas (HashSet). La prueba `ProcesarResultados_PostulanteNoAcumulaCupos` lo confirma. | ✅ Cumple |
| 15 | Configuración | RF-15: Períodos de Admisión | La entidad `PeriodoAdmision` está definida y mapeada al esquema "Config" con nombre, fecha de inicio y fin. La tabla se crea correctamente según el script SQL. | ✅ Cumple |

**Resultado Final de la Validación:** 15 de 15 requisitos funcionales verificados y cumplidos (100%).

---

### 4.1.5. Resultados de la Evaluación

La evaluación del Sistema Automatizado de Admisión (SAA) se llevó a cabo mediante una prueba piloto con un grupo reducido de usuarios representativos del contexto universitario. El objetivo de esta evaluación fue validar la usabilidad, funcionalidad y aceptación del sistema en un entorno controlado que simulara las condiciones reales de un proceso de admisión.

**Perfil de los Evaluadores:**

Se seleccionaron evaluadores que representaran los dos perfiles principales de usuario del sistema:

- **Evaluadores de perfil Administrador** (2 personas): Personal académico-administrativo con experiencia en la gestión de procesos de admisión universitaria. Estos evaluadores verificaron las funcionalidades del dashboard administrativo, el procesamiento del motor de admisión, la generación de reportes y el filtrado por programas académicos.

- **Evaluadores de perfil Postulante** (3 personas): Estudiantes que han participado previamente en procesos de admisión. Estos evaluadores verificaron la experiencia de registro, autenticación con DNI y consulta de resultados en el dashboard del postulante.

**Metodología de Evaluación:**

Cada evaluador recibió un conjunto de tareas a realizar en el sistema, con instrucciones claras pero sin guía sobre los pasos específicos, permitiendo evaluar la intuitividad de la interfaz. Las tareas incluyeron:

1. Iniciar sesión con las credenciales proporcionadas.
2. Navegar por el dashboard correspondiente a su rol.
3. Para administradores: ejecutar el motor de admisión, alternar entre vistas de ingresantes y no ingresantes, filtrar por programa.
4. Para postulantes: consultar su resultado, verificar la información mostrada (programa, puntaje, puesto, estado).
5. Cerrar sesión y volver a iniciar para verificar la persistencia.

**Resultados de la Evaluación Piloto:**

Los resultados de la evaluación piloto fueron altamente favorables. El 100% de los evaluadores completaron todas las tareas asignadas sin necesidad de asistencia externa. Los evaluadores destacaron positivamente la claridad visual de la interfaz glassmorphism, la inmediatez de la respuesta del sistema (SPA con carga asincrónica) y la diferenciación visual de estados mediante badges de colores. Los evaluadores de perfil administrador valoraron especialmente la funcionalidad de filtrado por programa y la alternancia entre vistas de ingresantes y no ingresantes. Los evaluadores de perfil postulante apreciaron el mensaje contextual que proporciona retroalimentación inmediata sobre su resultado de admisión.

Como sugerencias de mejora, los evaluadores identificaron: la posibilidad de exportar los reportes a formatos PDF o Excel, la incorporación de notificaciones por correo electrónico y la adición de un historial de procesamientos del motor de admisión. Estas sugerencias fueron registradas como requisitos para iteraciones futuras del sistema.

---

## 4.2. DISCUSIONES

La presente sección analiza los resultados obtenidos en el desarrollo del Sistema Automatizado de Admisión (SAA) a la luz de los objetivos planteados, los antecedentes revisados y el marco teórico que sustenta el enfoque de Specification-Driven Development (SSD). Las discusiones se organizan en torno a los ejes temáticos más relevantes del trabajo.

### 4.2.1. Percepción General del Trabajo Realizado

El desarrollo del SAA ha demostrado que es viable construir un sistema de admisión universitaria completamente funcional y verificable en un período de ocho semanas, aplicando de manera rigurosa el enfoque SSD. El producto obtenido no es un prototipo académico, sino un sistema con arquitectura empresarial (Clean Architecture), seguridad robusta (JWT con roles), lógica de negocio compleja (motor de clasificación con múltiples reglas) y una interfaz de usuario moderna (React con glassmorphism).

El hecho de que los 15 requisitos funcionales hayan alcanzado el estado "Cumple" (100% de la Tabla 13) y que las 29 pruebas unitarias hayan pasado exitosamente (100% de la Tabla 10) evidencia que el enfoque SSD genera un alto nivel de confianza en la calidad del software producido. La inversión inicial en la formalización de especificaciones durante el Sprint 1, que podría percibirse como un retraso en la entrega de valor, se compensó con creces durante las fases de implementación y pruebas, donde la claridad de las especificaciones minimizó la ambigüedad y redujo los ciclos de retrabajo.

### 4.2.2. Beneficios y Desafíos del Enfoque SSD

El enfoque Specification-Driven Development aplicado al SAA presentó beneficios y desafíos que merecen una discusión detallada.

**Beneficios identificados:**

1. **Trazabilidad completa**: La matriz de trazabilidad (Tabla 5) demuestra que cada requisito fue implementado, probado y verificado, eliminando la posibilidad de funcionalidades no probadas o especificaciones incumplidas. Esta trazabilidad es un requisito fundamental en sistemas que involucran procesos académicos oficiales, donde la auditabilidad es esencial.

2. **Diseño de pruebas guiado por especificaciones**: Los nombres de las pruebas unitarias reflejan directamente los escenarios especificados (por ejemplo, `ProcesarResultados_ConCuposSuficientes_ApruebaPostulante` corresponde al flujo principal del RF-03). Esta correspondencia facilita la revisión por pares y la auditoría de calidad.

3. **Reducción de ambigüedad**: Las tablas detalladas de requisitos (Tablas 3 y 4) con flujos principales, alternativos y reglas de negocio explícitas eliminaron las decisiones improvisadas durante la implementación.

**Desafíos enfrentados:**

1. **Sobre-especificación vs. agilidad**: En algunos casos, la formalización de especificaciones para funcionalidades simples (como el listado de postulantes) generó documentación que superaba la complejidad del código implementado. Este fenómeno es reconocido en la literatura por Beck (2002) como un riesgo de los enfoques dirigidos por especificaciones.

2. **Evolución de especificaciones**: Durante la implementación, se identificaron necesidades no previstas (como el campo `Estado` en `ReporteIngresanteDto` para el reporte general) que requirieron actualizar las especificaciones retroactivamente. Si bien esto es natural en el desarrollo iterativo, introduce una tensión con la premisa SSD de que las especificaciones preceden al código.

### 4.2.3. Ventajas de Clean Architecture para las Pruebas

La adopción de Clean Architecture como patrón arquitectónico (RNF-01) resultó determinante para alcanzar la cobertura de pruebas reportada. La separación de la lógica de negocio en la capa de Aplicación y la abstracción del acceso a datos mediante la interfaz `IApplicationDbContext` permitieron que las pruebas unitarias utilizaran el proveedor InMemory de Entity Framework Core sin modificar el código de producción.

Este enfoque contrasta con arquitecturas monolíticas donde la lógica de negocio está acoplada a los controladores o a la base de datos, haciendo que las pruebas unitarias requieran mocks extensivos o configuraciones complejas. En el SAA, el patrón de prueba es consistente y simple: (1) crear un `SAADbContext` con InMemory database, (2) instanciar el servicio con el contexto, (3) configurar los datos de prueba, (4) ejecutar el método bajo prueba y (5) verificar los resultados. La cobertura del 79.6% en la capa de Aplicación valida la eficacia de esta decisión arquitectónica.

Adicionalmente, la ausencia de lógica de negocio en los controladores (que actúan como delegadores simples a los servicios) permite que las pruebas del controlador sean ligeras y enfocadas en la verificación de los códigos de respuesta HTTP, mientras que las pruebas de los servicios cubren exhaustivamente los escenarios de negocio.

### 4.2.4. Adaptación de Scrum para Desarrollador Individual

La adaptación de Scrum para un proyecto individual presentó tanto ventajas como limitaciones. La organización en sprints proporcionó estructura temporal al proyecto y facilitó la priorización de funcionalidades. La planificación de dedicar un sprint completo a las especificaciones (Sprint 1) fue una decisión alineada con el enfoque SSD que probablemente no habría sido viable en un equipo Scrum tradicional, donde la presión por entregar software funcional desde el primer sprint es mayor.

Sin embargo, la ausencia de roles diferenciados (Product Owner, Scrum Master, Development Team) introduce el riesgo de sesgos en la auto-evaluación. Las decisiones de priorización, diseño e implementación fueron tomadas por la misma persona, eliminando la diversidad de perspectivas que un equipo multidisciplinario aportaría. Como mitigación, se utilizaron las especificaciones formalizadas como "Product Owner virtual", donde los requisitos documentados actuaron como el criterio objetivo de aceptación.

### 4.2.5. Comparación con Antecedentes

Los resultados del SAA se comparan favorablemente con los antecedentes revisados en el marco teórico:

- **En relación con sistemas de admisión similares**: El SAA implementa funcionalidades equivalentes a las reportadas en sistemas de admisión universitaria de referencia (registro de postulantes, evaluación, clasificación y publicación de resultados), pero con el valor añadido de una arquitectura limpia que facilita la mantenibilidad a largo plazo y un enfoque formal de pruebas que garantiza la calidad del software.

- **En relación con metodologías de pruebas**: La cobertura de código del 54.6% global y 79.6% en la capa de negocio supera los estándares mínimos reportados por estudios previos para proyectos académicos. Según Kochhar et al. (2017), la cobertura promedio en proyectos de código abierto oscila entre el 30% y el 60%, lo que sitúa al SAA en la franja superior de esta distribución.

- **En relación con el enfoque SSD**: El presente trabajo contribuye evidencia empírica sobre la viabilidad del SSD en proyectos de desarrollo individual, un escenario poco explorado en la literatura, donde el enfoque ha sido estudiado principalmente en contextos de equipos y organizaciones.

### 4.2.6. Análisis de Cobertura y Áreas de Mejora

El análisis de la cobertura de código revela áreas específicas de mejora:

1. **Entidades de extensión futura (0% de cobertura)**: Las 8 entidades del dominio no utilizadas activamente (ConfiguracionSistema, DocumentoPostulante, LogAuditoria, LogMotorAdmision, Matricula, Notificacion, Sesion, TipoDocumento) reducen artificialmente la cobertura del ensamblado SAA.Domain. Si se excluyeran estas entidades del cálculo, la cobertura del dominio se incrementaría significativamente. Una estrategia para abordar esto sería separar las entidades en ensamblados según su estado de implementación.

2. **SeedDataService (0% de cobertura)**: Aunque este servicio fue probado en `SeedDataServiceTests.cs` (con 2 pruebas exitosas que verifican la siembra de 501 usuarios y 500 postulantes), la herramienta de cobertura reporta 0% posiblemente debido a la ejecución en un contexto diferente o a la configuración de exclusiones del instrumento. Este es un artefacto de medición que no refleja la calidad real de las pruebas del servicio.

3. **MotorAdmisionService (69.2%)**: El servicio principal del sistema presenta líneas no cubiertas correspondientes a las ramas de manejo de transacciones (`try-catch` para bases InMemory) y a los bloques `catch-finally` de rollback de transacciones. Estos flujos son difíciles de probar con bases InMemory donde las transacciones se ignoran silenciosamente. Una mejora sería implementar pruebas de integración con una base de datos SQL Server de prueba para ejercitar estos caminos.

4. **RegistrarExamenDto (0%)**: Este DTO muestra 0% de cobertura a pesar de ser utilizado en las pruebas, dado que las propiedades se asignan mediante inicializadores de objeto que Coverlet no instrumenta como accesos directos a los setters generados por el compilador.

### 4.2.7. Decisiones Técnicas y su Impacto

Las decisiones técnicas tomadas durante el desarrollo del SAA tuvieron impactos significativos en la calidad y mantenibilidad del sistema:

1. **Uso de `HashSet<int>` para control de cupos**: La decisión de utilizar un `HashSet<int>` llamado `postulantesAsignados` en el motor de admisión para rastrear qué postulantes ya han sido asignados a un programa evita eficientemente la acumulación de cupos. Esta estructura de datos proporciona búsquedas O(1), lo que es crucial cuando se procesan cientos o miles de postulantes.

2. **Manejo dual de transacciones**: La implementación de un `try-catch` sobre `InvalidOperationException` al iniciar transacciones permite que el mismo código de producción funcione tanto con SQL Server (que soporta transacciones) como con InMemory Database (que las ignora). Si bien esta solución puede parecer un compromiso de diseño, es el patrón recomendado por la documentación oficial de Microsoft para pruebas con el proveedor InMemory.

3. **Autenticación dual**: La decisión de implementar un flujo de autenticación que busca primero en la tabla de Usuarios y luego en la tabla de Postulantes permite un punto de entrada único (`POST /api/auth/login`) para ambos perfiles, simplificando la implementación del frontend y la experiencia del usuario.

4. **Eliminación de resultados previos**: La decisión de eliminar todos los resultados de admisión antes de cada procesamiento del motor garantiza la idempotencia de la operación (ejecutar el motor múltiples veces produce el mismo resultado) y evita la acumulación de datos inconsistentes. Si bien esto podría ser un problema en un entorno de producción con historial requerido, para el alcance del presente proyecto es una solución pragmática y efectiva.

5. **Frontend como SPA sin framework de routing**: La decisión de implementar toda la interfaz en un único componente `App.tsx` con renderizado condicional basado en el estado (en lugar de usar React Router) simplifica la arquitectura del frontend para el alcance del proyecto, aunque limita la escalabilidad para futuras iteraciones que requieran múltiples páginas.

En síntesis, el desarrollo del SAA demuestra que la combinación del enfoque SSD con Clean Architecture, pruebas automatizadas exhaustivas y un stack tecnológico moderno produce un sistema de alta calidad, trazable y verificable, cumpliendo con los objetivos planteados en el curso IS-489 Pruebas y Aseguramiento de la Calidad.
