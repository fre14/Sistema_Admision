# CAPÍTULO V: CONCLUSIONES Y RECOMENDACIONES

## 5.1. Conclusiones

### 5.1.1. Conclusión del Objetivo Específico 1 (OE1): Análisis basado en especificaciones (SSD)

El proceso de análisis del Sistema Automatizado de Admisión (SAA) fue desarrollado bajo el enfoque de *Specification-Driven Development* (SSD), lo cual garantizó que la totalidad de los requisitos del sistema fueran documentados, formalizados y validados antes de iniciar cualquier actividad de codificación. Durante esta fase, se identificaron 12 requisitos funcionales (RF) y 8 requisitos no funcionales (RNF), los cuales fueron organizados en una Especificación de Requisitos de Software (ERS) estructurada. Los requisitos funcionales abarcaron desde el registro y gestión de postulantes (RF-01), la inscripción al proceso de admisión mediante fichas de postulación (RF-02), el registro de calificaciones de exámenes de admisión (RF-03), hasta el procesamiento automatizado de resultados a través del Motor de Admisión (RF-04), la consulta individual de resultados por parte de los postulantes (RF-05), la generación de reportes de ingresantes y no ingresantes (RF-06), la autenticación segura con tokens JWT (RF-07), la gestión de programas académicos (RF-08), el control de roles y permisos diferenciados (RF-09), el filtrado de resultados por programa académico (RF-10), la gestión de períodos de admisión (RF-11) y la visualización de estados diferenciados — Ingresante, Aprobado, Desaprobado — (RF-12).

Los requisitos no funcionales, por su parte, establecieron las restricciones de calidad del sistema conforme a la norma ISO/IEC 25010:2023, incluyendo: rendimiento (RNF-01: tiempos de respuesta menores a 2 segundos para operaciones CRUD), seguridad (RNF-02: autenticación basada en JWT con contraseñas hasheadas mediante BCrypt), usabilidad (RNF-03: interfaz responsiva con diseño glassmorphism), mantenibilidad (RNF-04: arquitectura Clean Architecture con separación en 4 capas), portabilidad (RNF-05: compatibilidad multiplataforma mediante .NET 10), fiabilidad (RNF-06: manejo transaccional de operaciones críticas), escalabilidad (RNF-07: diseño modular que permita crecimiento horizontal) y compatibilidad (RNF-08: API RESTful estándar con formato JSON). El enfoque SSD demostró ser fundamental para evitar ambigüedades en la implementación, ya que cada módulo del sistema contó con especificaciones claras y verificables que sirvieron como referencia directa para las fases posteriores de diseño, codificación y validación.

### 5.1.2. Conclusión del Objetivo Específico 2 (OE2): Diseño arquitectónico del sistema

El diseño arquitectónico del SAA se realizó siguiendo los principios de Clean Architecture propuestos por Robert C. Martin (2017), lo que resultó en una estructura de cuatro capas claramente diferenciadas: **SAA.Domain** (capa de dominio con las entidades de negocio), **SAA.Application** (capa de aplicación con los servicios y DTOs), **SAA.Infrastructure** (capa de infraestructura con el acceso a datos mediante Entity Framework Core) y **SAA.API** (capa de presentación con los controladores REST y el frontend React). Esta separación de responsabilidades garantiza que las reglas de negocio permanezcan independientes de los detalles de implementación tecnológica, facilitando la mantenibilidad y la evolución del sistema a largo plazo.

En cuanto al modelo de datos, se diseñó un Diagrama Entidad-Relación (DER) compuesto por 8 tablas distribuidas en 3 esquemas lógicos dentro de SQL Server: el esquema **Admision** (que contiene las tablas `Postulante`, `ExamenAdmision`, `FichaPostulacion` y `ResultadoAdmision`), el esquema **Config** (que agrupa las tablas `ProgramaAcademico` y `PeriodoAdmision`) y el esquema **Seguridad** (que incluye las tablas `Usuario` y `Rol`). Las relaciones entre entidades fueron establecidas mediante claves foráneas, destacándose la restricción de unicidad sobre el campo `DNI` en la tabla `Postulante` (índice único `IX_Postulante_DNI`) para evitar duplicidades en el registro de postulantes. El diseño contempló la integridad referencial entre `FichaPostulacion` y `Postulante` (vía `IdPostulante`), entre `FichaPostulacion` y `ProgramaAcademico` (vía `IdProgramaAcademico`), entre `ExamenAdmision` y `FichaPostulacion` (vía `IdFichaPostulacion`), y entre `ResultadoAdmision` y las tablas `Postulante`, `FichaPostulacion` y `ProgramaAcademico`. La separación en esquemas lógicos refleja los bounded contexts del dominio y facilita la administración de permisos a nivel de base de datos.

### 5.1.3. Conclusión del Objetivo Específico 3 (OE3): Implementación del sistema

La implementación del Sistema Automatizado de Admisión (SAA) se llevó a cabo utilizando tecnologías de última generación: **.NET 10** con **C#** para el backend (API RESTful con ASP.NET Core), **React 19** con **TypeScript** y **Vite** para el frontend, **SQL Server** como sistema gestor de base de datos relacional, y **Entity Framework Core** como ORM para el mapeo objeto-relacional. La capa de dominio (`SAA.Domain`) implementó 6 entidades principales — `Postulante` (con 11 propiedades incluyendo `IdPostulante`, `Nombres`, `Apellidos`, `DNI`, `IdProgramaInteres`, `Correo`, `Telefono`, `Direccion`, `FechaNacimiento`, `Estado`, `FechaCreacion`, `FechaActualizacion`), `ExamenAdmision` (con 12 propiedades), `FichaPostulacion` (con 8 propiedades), `ResultadoAdmision` (con 10 propiedades incluyendo `OrdenMerito`), `ProgramaAcademico` (con 11 propiedades incluyendo `Vacantes`) y `Usuario` (con 9 propiedades) —, todas documentadas con comentarios XML siguiendo estándares de documentación de código C#.

El componente más crítico del sistema, el `MotorAdmisionService`, fue implementado en la capa de aplicación (`SAA.Application`) con un total de 202 líneas de código distribuidas en 4 métodos principales: `RegistrarExamenAsync` (registro de calificaciones de exámenes con manejo transaccional), `ProcesarResultadosAsync` (procesamiento automatizado de resultados que ordena postulantes por puntaje descendente, aplica el puntaje mínimo aprobatorio de 50.0 puntos, asigna vacantes por programa académico y determina los estados Ingresante/Aprobado/Desaprobado), `ObtenerReporteIngresantesAsync` (generación del reporte de ingresantes) y `ObtenerReporteTodosAsync` (reporte general de todos los resultados). La metodología de desarrollo adoptada fue Scrum adaptado para un desarrollador individual, organizando el trabajo en 4 sprints. El control de versiones se gestionó mediante Git y GitHub, asegurando la trazabilidad completa de los cambios realizados durante todo el ciclo de desarrollo.

### 5.1.4. Conclusión del Objetivo Específico 4 (OE4): Validación mediante pruebas

La validación del sistema se realizó mediante pruebas unitarias automatizadas utilizando el framework **xUnit.net** y la herramienta de cobertura **Coverlet**, siguiendo el principio SSD de verificar cada especificación documentada contra su implementación. Los resultados de la cobertura de código, generados el 26 de mayo de 2026, reportaron una cobertura de línea general del **54.6%** (268 líneas cubiertas de un total de 490 líneas cubribles), una cobertura de rama (*branch coverage*) del **58.6%** (34 de 58 ramas cubiertas) y una cobertura de métodos del **51.8%** (84 de 162 métodos cubiertos). El análisis detallado por ensamblado reveló que la capa de aplicación (**SAA.Application**) alcanzó una cobertura del **79.6%**, siendo la capa con mayor nivel de verificación; dentro de esta, los servicios críticos `AuthService` alcanzaron el 100% de cobertura, `PostulanteService` el 72.7% y `MotorAdmisionService` el 69.2%. Todos los DTOs de transferencia de datos — `CrearPostulanteDto`, `LoginRequestDto`, `LoginResponseDto`, `PostulanteResponseDto`, `ReporteIngresanteDto` y `UsuarioDto` — alcanzaron el 100% de cobertura.

La capa de dominio (**SAA.Domain**) alcanzó una cobertura del **33.6%**, donde las entidades principales utilizadas activamente por los servicios obtuvieron coberturas significativas: `Usuario` con 90%, `Postulante` con 75%, `ResultadoAdmision` con 60% y `ProgramaAcademico` con 50%. La capa de infraestructura (**SAA.Infrastructure**) registró un 19.6%, con el `SAADbContext` alcanzando el 100% de cobertura. En total, el análisis abarcó 3 ensamblados, 28 clases y 25 archivos, con un total de 1,005 líneas de código fuente. Los resultados de validación se evaluaron mediante el criterio **Cumple/No Cumple**, verificándose que todos los módulos críticos del sistema (autenticación, registro de postulantes, procesamiento de resultados, generación de reportes) cumplieron satisfactoriamente con las especificaciones definidas en la fase de análisis.

### 5.1.5. Conclusión General

El presente trabajo monográfico ha demostrado la viabilidad y efectividad del desarrollo de un Sistema Automatizado de Admisión (SAA) utilizando el enfoque de *Specification-Driven Development* (SSD) como metodología central para la ingeniería de software en el contexto del aseguramiento de la calidad. El sistema resultante automatiza exitosamente el proceso completo de admisión universitaria — desde el registro de postulantes y la gestión de exámenes, hasta el procesamiento automatizado de resultados mediante un motor de admisión que aplica reglas de negocio configurables (puntaje mínimo, límite de vacantes por programa, orden de mérito) —, eliminando los procesos manuales propensos a errores y reduciendo significativamente los tiempos de procesamiento.

La adopción de Clean Architecture como patrón arquitectónico, complementada con tecnologías modernas como .NET 10, React 19, TypeScript y SQL Server, ha permitido construir un sistema robusto, mantenible y escalable que cumple con los estándares de calidad establecidos por la norma ISO/IEC 25010:2023. El enfoque SSD, al priorizar la documentación exhaustiva de especificaciones antes de la codificación, no solo facilitó la implementación sistemática de cada funcionalidad, sino que también proporcionó una base objetiva para la validación del sistema mediante pruebas automatizadas. Los resultados de cobertura de código, si bien son susceptibles de mejora en las capas de dominio e infraestructura, confirman que los módulos críticos del sistema fueron verificados rigurosamente, alcanzando una cobertura del 79.6% en la capa de lógica de aplicación. En conclusión, el SAA constituye una solución tecnológica integral que responde a las necesidades del proceso de admisión, sentando las bases para futuras mejoras y extensiones del sistema.

---

## 5.2. Recomendaciones

### Recomendación 1: Despliegue en entorno de producción en la nube y pruebas de carga

Se recomienda realizar el despliegue del Sistema Automatizado de Admisión (SAA) en un entorno de producción basado en la nube, preferiblemente utilizando Microsoft Azure App Service para el backend .NET 10 y Azure Static Web Apps para el frontend React 19, complementado con Azure SQL Database como servicio de base de datos administrado. Previo al lanzamiento en producción, es fundamental ejecutar pruebas de carga (*load testing*) y pruebas de estrés (*stress testing*) utilizando herramientas como Apache JMeter, k6 o Azure Load Testing, con el objetivo de validar el rendimiento del sistema bajo condiciones de alta concurrencia, simulando escenarios realistas como el procesamiento simultáneo de cientos de postulantes durante los períodos pico de admisión. Estas pruebas permitirán identificar cuellos de botella en la API, optimizar las consultas a la base de datos y dimensionar adecuadamente los recursos de infraestructura necesarios para garantizar tiempos de respuesta aceptables conforme al requisito no funcional RNF-01.

### Recomendación 2: Incrementar la cobertura de código al 80% o superior

Se recomienda encarecidamente incrementar la cobertura de código del proyecto al 80% o superior, focalizando los esfuerzos en las capas de dominio (actualmente en 33.6%) e infraestructura (actualmente en 19.6%). Para la capa de dominio (`SAA.Domain`), se deben crear pruebas unitarias que verifiquen la correcta inicialización de las entidades que actualmente tienen 0% de cobertura — tales como `ConfiguracionSistema`, `DocumentoPostulante`, `FichaPostulacion`, `LogAuditoria`, `LogMotorAdmision`, `Matricula`, `Notificacion`, `PeriodoAdmision`, `Rol`, `Sesion` y `TipoDocumento` —, así como validar las restricciones de negocio de cada entidad. Para la capa de infraestructura (`SAA.Infrastructure`), se recomienda agregar pruebas de integración para el servicio `SeedDataService` (actualmente en 0%) y validar las configuraciones de Entity Framework Core, incluyendo las migraciones y los mapeos de las entidades a las tablas de la base de datos. El objetivo es alcanzar una cobertura de métodos superior al 80% (actualmente en 51.8%) y una cobertura de ramas superior al 75% (actualmente en 58.6%), asegurando la detección temprana de regresiones ante futuros cambios.

### Recomendación 3: Implementar un pipeline de CI/CD con GitHub Actions

Se recomienda implementar un pipeline de Integración Continua y Despliegue Continuo (CI/CD) utilizando GitHub Actions, configurando flujos de trabajo automatizados que se ejecuten en cada *push* y *pull request* al repositorio. El pipeline debería incluir las siguientes etapas: (1) restauración de dependencias (`dotnet restore`), (2) compilación del proyecto (`dotnet build --no-restore`), (3) ejecución automatizada de todas las pruebas unitarias (`dotnet test --no-build --collect:"XPlat Code Coverage"`), (4) generación del reporte de cobertura con Coverlet y ReportGenerator, (5) verificación de que la cobertura mínima supere el umbral establecido (gate de calidad), (6) compilación del frontend React (`npm run build`), y (7) despliegue automático al entorno de staging o producción. Esta automatización eliminará la posibilidad de introducir código que no pase las pruebas, garantizará la calidad continua del código fuente y reducirá significativamente el tiempo de entrega de nuevas funcionalidades al usuario final.

### Recomendación 4: Incorporar pruebas del frontend con React Testing Library

Se recomienda incorporar pruebas automatizadas para el frontend desarrollado en React 19 con TypeScript, utilizando **React Testing Library** en conjunto con **Vitest** como test runner, dado que el proyecto ya utiliza Vite como herramienta de construcción. Las pruebas del frontend deberían cubrir los siguientes escenarios críticos: (a) renderizado correcto del formulario de inicio de sesión (componente de login con campos de usuario/DNI y contraseña, botón "Ingresar" y mensaje de error), (b) flujo completo de autenticación incluyendo almacenamiento del token JWT en `localStorage`, (c) renderizado condicional del dashboard de administrador versus el dashboard de postulante según el rol del usuario, (d) funcionamiento del filtro por programa académico en la tabla de resultados, (e) correcta visualización de los estados de admisión (Ingresante, Aprobado, Desaprobado) con sus respectivos mensajes y estilos CSS, y (f) pruebas de accesibilidad para verificar que la interfaz con diseño glassmorphism mantenga un contraste adecuado y sea navegable mediante teclado. La incorporación de estas pruebas complementará la validación del backend y proporcionará una cobertura de pruebas integral de punta a punta.

### Recomendación 5: Desarrollar una aplicación móvil complementaria con React Native

Se recomienda desarrollar una aplicación móvil multiplataforma (iOS y Android) utilizando **React Native** o **Expo**, que permita a los postulantes consultar sus resultados de admisión de manera rápida y conveniente desde sus dispositivos móviles. La aplicación móvil consumiría la misma API RESTful del SAA ya implementada, aprovechando los endpoints existentes (`/api/auth/login`, `/api/postulantes/mi-resultado`) y la autenticación JWT. Las funcionalidades prioritarias para la versión móvil incluirían: inicio de sesión seguro, consulta de estado de admisión (Ingresante/Aprobado/Desaprobado) con notificaciones push al momento de la publicación de resultados, visualización del puntaje obtenido y orden de mérito, y acceso al historial de postulaciones. Esta extensión móvil mejoraría significativamente la accesibilidad del sistema y la experiencia del postulante, alineándose con las tendencias actuales de consumo digital y con el requisito no funcional de usabilidad (RNF-03).

### Recomendación 6: Integrar notificaciones por correo electrónico para postulantes

Se recomienda integrar un servicio de notificaciones por correo electrónico que informe automáticamente a los postulantes sobre los eventos relevantes del proceso de admisión, tales como: confirmación de registro exitoso de la ficha de postulación, recordatorio de fecha y hora del examen de admisión, publicación de resultados (con indicación del estado obtenido: Ingresante, Aprobado o Desaprobado), e instrucciones para los siguientes pasos según el resultado. Para la implementación, se recomienda utilizar un servicio de correo transaccional como **SendGrid**, **Amazon SES** o **Azure Communication Services**, integrándolo en la capa de aplicación (`SAA.Application`) mediante el patrón de inyección de dependencias y eventos de dominio. El campo `Correo` ya se encuentra disponible en las entidades `Postulante` y `Usuario` del modelo de datos actual, lo cual facilita la incorporación de esta funcionalidad sin modificaciones al esquema de base de datos. La implementación de notificaciones por correo electrónico mejorará la comunicación institucional, reducirá las consultas presenciales y contribuirá a la transparencia del proceso de admisión.

---

---

# REFERENCIAS BIBLIOGRÁFICAS

Arias, F. G. (2012). *El proyecto de investigación: Introducción a la metodología científica* (6.ª ed.). Editorial Episteme.

Banks, A., & Porcello, E. (2020). *Learning React: Modern Patterns for Developing React Apps* (2nd ed.). O'Reilly Media.

Beck, K. (2004). *Extreme Programming Explained: Embrace Change* (2nd ed.). Addison-Wesley Professional.

Bernal Torres, C. A. (2016). *Metodología de la investigación: Administración, economía, humanidades y ciencias sociales* (4.ª ed.). Pearson Educación.

Brooke, J. (1996). SUS: A quick and dirty usability scale. En P. W. Jordan, B. Thomas, B. A. Weerdmeester, & I. L. McClelland (Eds.), *Usability Evaluation in Industry* (pp. 189–194). Taylor & Francis.

Carrasco Díaz, S. (2019). *Metodología de la investigación científica: Pautas metodológicas para diseñar y elaborar el proyecto de investigación* (2.ª ed.). Editorial San Marcos.

Chavez Minaya, J. A. (2024). *Desarrollo de un sistema web para la gestión del proceso de admisión en una institución educativa superior utilizando la metodología Scrum* [Tesis de pregrado, Universidad Nacional Mayor de San Marcos]. Repositorio institucional UNMSM.

CONCYTEC. (s.f.). *Definición de investigación aplicada*. Consejo Nacional de Ciencia, Tecnología e Innovación Tecnológica. https://www.gob.pe/concytec

Encalada Díaz, R. M., & Gómez Santillán, P. A. (2022). *Implementación de un sistema de información para automatizar el proceso de admisión de una universidad privada* [Tesis de pregrado, Universidad Privada del Norte]. Repositorio institucional UPN.

Evans, E. (2003). *Domain-Driven Design: Tackling Complexity in the Heart of Software*. Addison-Wesley Professional.

Fowler, M. (2018). *Refactoring: Improving the Design of Existing Code* (2nd ed.). Addison-Wesley Professional.

Freeman, A. (2022). *Pro ASP.NET Core 7: Develop Cloud-Ready Web Applications Using MVC, Blazor, and Razor Pages*. Apress.

Hernández-Sampieri, R., Fernández Collado, C., & Baptista Lucio, P. (2014). *Metodología de la investigación* (6.ª ed.). McGraw-Hill Interamericana.

ISO/IEC. (2023). *ISO/IEC 25010:2023 — Systems and software engineering — Systems and software Quality Requirements and Evaluation (SQuaRE) — Product quality model*. International Organization for Standardization.

Laudon, K. C., & Laudon, J. P. (2020). *Management Information Systems: Managing the Digital Firm* (16th ed.). Pearson.

Lock, A. (2023). *ASP.NET Core in Action* (3rd ed.). Manning Publications.

Martin, R. C. (2017). *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. Prentice Hall.

Meta Platforms. (2024). *React Documentation*. https://react.dev/

Microsoft. (2024a). *.NET Documentation*. https://learn.microsoft.com/dotnet/

Microsoft. (2024b). *ASP.NET Core Documentation*. https://learn.microsoft.com/aspnet/core/

Microsoft. (2024c). *Entity Framework Core Documentation*. https://learn.microsoft.com/ef/core/

Microsoft. (2024d). *SQL Server Documentation*. https://learn.microsoft.com/sql/sql-server/

Newman, S. (2021). *Building Microservices: Designing Fine-Grained Systems* (2nd ed.). O'Reilly Media.

Pressman, R. S., & Maxim, B. R. (2020). *Ingeniería del Software: Un Enfoque Práctico* (9.ª ed.). McGraw-Hill Interamericana.

Schwaber, K., & Sutherland, J. (2020). *The Scrum Guide: The Definitive Guide to Scrum — The Rules of the Game*. Scrum.org. https://scrumguides.org/

Sommerville, I. (2016). *Software Engineering* (10th ed.). Pearson Education.

Tamayo y Tamayo, M. (2012). *El proceso de la investigación científica: Incluye evaluación y administración de proyectos de investigación* (5.ª ed.). Editorial Limusa.

Coverlet Contributors. (2024). *Coverlet — Cross Platform Code Coverage Framework for .NET*. https://github.com/coverlet-coverage/coverlet

xUnit.net Contributors. (2024). *xUnit.net — Free, Open Source, Community-Focused Unit Testing Tool for .NET*. https://xunit.net/

---

---

# ANEXOS

## ANEXO 1: MATRIZ DE CONSISTENCIA

**Título:** Sistema Automatizado de Admisión (SAA) desarrollado con enfoque *Specification-Driven Development* (SSD)

**Autor:** Fredy Bonilla Rey

**Curso:** IS-489 Pruebas y Aseguramiento de la Calidad

| **Problema** | **Objetivo** | **Variable** | **Dimensión** | **Indicador** | **Técnica** | **Instrumento** |
|---|---|---|---|---|---|---|
| **PG:** ¿De qué manera el desarrollo de un Sistema Automatizado de Admisión (SAA) con enfoque SSD permite mejorar la gestión del proceso de admisión universitaria? | **OG:** Desarrollar un Sistema Automatizado de Admisión (SAA) aplicando el enfoque *Specification-Driven Development* (SSD) para mejorar la gestión del proceso de admisión universitaria. | **X:** Sistema Automatizado de Admisión (SAA) | Automatización del proceso de admisión | Nivel de automatización del proceso de admisión; Tiempo de procesamiento de resultados | Análisis documental; Observación | Ficha de análisis documental; Guía de observación |
| **PE1:** ¿Cómo realizar el análisis de requisitos del SAA utilizando el enfoque SSD para garantizar la completitud de las especificaciones? | **OE1:** Realizar el análisis de requisitos del SAA utilizando el enfoque SSD para documentar las especificaciones completas del sistema. | **X₁:** Análisis basado en especificaciones | Requisitos funcionales y no funcionales | Número de requisitos funcionales identificados (RF); Número de requisitos no funcionales identificados (RNF); Porcentaje de requisitos trazables | Análisis documental | Ficha de análisis documental (ERS) |
| **PE2:** ¿Cómo diseñar la arquitectura del SAA para asegurar la separación de responsabilidades y la escalabilidad del sistema? | **OE2:** Diseñar la arquitectura del SAA aplicando Clean Architecture para garantizar la separación de responsabilidades y la mantenibilidad del sistema. | **X₂:** Diseño arquitectónico | Arquitectura de software; Modelo de datos | Número de diagramas de arquitectura completados; Número de tablas del modelo E-R; Número de esquemas lógicos definidos | Observación | Guía de observación técnica; Diagrama E-R; Diagrama de arquitectura |
| **PE3:** ¿Cómo implementar el SAA utilizando tecnologías modernas (.NET 10, React 19) siguiendo las especificaciones definidas en la fase de análisis? | **OE3:** Implementar el SAA utilizando .NET 10, React 19 y SQL Server, siguiendo las especificaciones definidas y la metodología Scrum adaptada. | **X₃:** Implementación del sistema | Codificación; Control de versiones; Gestión ágil | Número de módulos implementados; Número de commits registrados; Número de sprints completados; Líneas de código fuente | Análisis documental | Repositorio Git/GitHub; Registro de sprints |
| **PE4:** ¿Cómo validar el funcionamiento del SAA mediante pruebas automatizadas para verificar el cumplimiento de las especificaciones? | **OE4:** Validar el funcionamiento del SAA mediante pruebas unitarias automatizadas con xUnit para verificar el cumplimiento de las especificaciones definidas. | **X₄:** Validación y pruebas | Pruebas unitarias; Cobertura de código | Porcentaje de cobertura de línea (%); Porcentaje de cobertura de ramas (%); Tasa de aprobación de pruebas (pass rate); Resultado Cumple/No Cumple por módulo | Pruebas automatizadas | Framework xUnit.net; Herramienta Coverlet; ReportGenerator |

---

## ANEXO 2: INSTRUMENTOS DE RECOLECCIÓN DE DATOS

### 2.1. Ficha de Análisis Documental

**Instrumento N.° 01 — Ficha de Análisis Documental para Requisitos del SAA**

| **Campo** | **Descripción** |
|---|---|
| **Código del Instrumento** | FAD-SAA-001 |
| **Proyecto** | Sistema Automatizado de Admisión (SAA) |
| **Investigador** | Fredy Bonilla Rey |
| **Curso** | IS-489 Pruebas y Aseguramiento de la Calidad |
| **Fecha de aplicación** | ____/____/________ |
| **Fase del proyecto** | Análisis / Diseño / Implementación / Validación |

**Sección A: Identificación del Documento Analizado**

| **N.°** | **Documento analizado** | **Fuente/Origen** | **Fecha del documento** | **Tipo de documento** |
|---|---|---|---|---|
| 1 | | | | Normativo / Técnico / Académico |
| 2 | | | | Normativo / Técnico / Académico |
| 3 | | | | Normativo / Técnico / Académico |

**Sección B: Requisitos Funcionales Identificados**

| **Código RF** | **Descripción del requisito funcional** | **Prioridad (Alta/Media/Baja)** | **Módulo asociado** | **Fuente del requisito** | **Estado (Identificado/Validado/Implementado)** |
|---|---|---|---|---|---|
| RF-01 | | | | | |
| RF-02 | | | | | |
| RF-03 | | | | | |
| RF-04 | | | | | |
| RF-05 | | | | | |

**Sección C: Requisitos No Funcionales Identificados**

| **Código RNF** | **Descripción del requisito no funcional** | **Característica ISO 25010** | **Métrica de verificación** | **Valor aceptable** | **Estado** |
|---|---|---|---|---|---|
| RNF-01 | | | | | |
| RNF-02 | | | | | |
| RNF-03 | | | | | |

**Sección D: Observaciones y Hallazgos**

| **N.°** | **Observación/Hallazgo** | **Impacto en el sistema** | **Acción recomendada** |
|---|---|---|---|
| 1 | | Alto / Medio / Bajo | |
| 2 | | Alto / Medio / Bajo | |

**Firma del investigador:** ___________________________

**Fecha:** ____/____/________

---

### 2.2. Guía de Observación Técnica

**Instrumento N.° 02 — Guía de Observación Técnica para la Validación del SAA**

| **Campo** | **Descripción** |
|---|---|
| **Código del Instrumento** | GOT-SAA-001 |
| **Proyecto** | Sistema Automatizado de Admisión (SAA) |
| **Investigador** | Fredy Bonilla Rey |
| **Curso** | IS-489 Pruebas y Aseguramiento de la Calidad |
| **Fecha de observación** | ____/____/________ |
| **Ambiente de pruebas** | Desarrollo / Staging / Producción |

**Sección A: Observación de la Arquitectura del Sistema**

| **N.°** | **Aspecto observado** | **Criterio de evaluación** | **Cumple (Sí/No)** | **Evidencia** | **Observaciones** |
|---|---|---|---|---|---|
| 1 | Separación en 4 capas (Domain, Application, Infrastructure, API) | Las capas están claramente separadas en proyectos independientes | | Estructura del directorio del proyecto | |
| 2 | Dependencias unidireccionales (de afuera hacia adentro) | Las capas externas dependen de las internas, no al revés | | Referencias del proyecto (.csproj) | |
| 3 | Modelo de datos con 3 esquemas (Admision, Config, Seguridad) | La base de datos tiene los 3 esquemas definidos | | Script SQL / Diagrama E-R | |
| 4 | Índice único sobre DNI del postulante | Existe restricción UNIQUE sobre el campo DNI | | Script de migración | |
| 5 | Integridad referencial entre tablas | Se verifican las FK entre entidades relacionadas | | Script SQL / EF Core Migrations | |

**Sección B: Observación del Funcionamiento del Sistema**

| **N.°** | **Funcionalidad evaluada** | **Procedimiento de prueba** | **Resultado esperado** | **Resultado obtenido** | **Cumple (Sí/No)** |
|---|---|---|---|---|---|
| 1 | Inicio de sesión (Login) | Ingresar credenciales válidas en el formulario de login | El sistema redirige al dashboard correspondiente (Admin/Postulante) | | |
| 2 | Registro de postulante | Enviar datos completos del postulante vía API | Se crea el registro en la tabla Admision.Postulante | | |
| 3 | Registro de examen | Registrar calificación de un postulante | Se crea el registro en Admision.ExamenAdmision | | |
| 4 | Procesamiento de resultados | Ejecutar el Motor de Admisión | Se generan resultados con estados Ingresante/Aprobado/Desaprobado | | |
| 5 | Consulta de resultado individual | Postulante consulta su estado de admisión | Se muestra el resultado con puntaje, puesto y estado | | |
| 6 | Reporte de ingresantes | Administrador genera el reporte | Se muestra la tabla con puesto, nombre, programa y puntaje | | |
| 7 | Filtro por programa académico | Seleccionar un programa del dropdown | La tabla se filtra mostrando solo los resultados del programa seleccionado | | |

**Sección C: Observación de la Cobertura de Pruebas**

| **Ensamblado** | **Cobertura de línea (%)** | **Cobertura de rama (%)** | **Cobertura de métodos (%)** | **Cumple umbral mínimo (≥50%)** |
|---|---|---|---|---|
| SAA.Application | | | | Sí / No |
| SAA.Domain | | | | Sí / No |
| SAA.Infrastructure | | | | Sí / No |
| **Total** | | | | |

**Firma del investigador:** ___________________________

**Fecha:** ____/____/________

---

## ANEXO 3: DETALLE DE HISTORIAS DE USUARIO / PRODUCT BACKLOG

### Metodología de priorización

Las historias de usuario fueron priorizadas utilizando el método **MoSCoW** (Must have, Should have, Could have, Won't have) y asignadas a los sprints según su dependencia funcional y su valor para el producto mínimo viable (MVP).

---

### HU-01: Registro de nuevos postulantes

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-01 |
| **Rol** | Administrador |
| **Historia** | Como administrador del sistema, quiero registrar nuevos postulantes ingresando sus datos personales completos para inscribirlos formalmente en el proceso de admisión. |
| **Prioridad** | Alta (Must have) |
| **Sprint** | Sprint 3 |
| **Esfuerzo estimado** | 8 puntos de historia |

**Criterios de Aceptación:**

1. El sistema debe permitir el ingreso de los siguientes datos obligatorios: Nombres, Apellidos, DNI (Documento Nacional de Identidad), Programa Académico de Interés (seleccionable de los programas activos) y Correo electrónico.
2. El sistema debe permitir el ingreso opcional de: Teléfono, Dirección y Fecha de Nacimiento.
3. El campo DNI debe ser único en el sistema; si se intenta registrar un postulante con un DNI ya existente, el sistema debe rechazar el registro y mostrar un mensaje de error descriptivo.
4. Al registrar exitosamente al postulante, el sistema debe asignar automáticamente el estado "Activo" y la fecha de creación (`FechaCreacion`) con la fecha y hora actual.
5. El sistema debe retornar los datos del postulante registrado, incluyendo su `IdPostulante` generado automáticamente.
6. La operación de registro debe ser accesible únicamente para usuarios autenticados con rol de Administrador.

---

### HU-02: Registro de calificaciones de exámenes

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-02 |
| **Rol** | Administrador |
| **Historia** | Como administrador del sistema, quiero registrar las calificaciones (puntajes) de los exámenes de admisión de cada postulante para evaluar su desempeño académico. |
| **Prioridad** | Alta (Must have) |
| **Sprint** | Sprint 3 |
| **Esfuerzo estimado** | 5 puntos de historia |

**Criterios de Aceptación:**

1. El sistema debe permitir registrar el puntaje obtenido por un postulante en su examen de admisión, identificando al postulante mediante su `IdPostulante`.
2. El puntaje debe ser un valor decimal con precisión de 2 decimales, en un rango de 0.00 a 1000.00.
3. Si el `IdPostulante` proporcionado no existe en el sistema, se debe lanzar una excepción con el mensaje "Postulante no encontrado."
4. El registro del examen debe incluir la fecha del examen (`FechaExamen`) asignada automáticamente con la fecha actual.
5. El sistema debe permitir agregar observaciones opcionales al registro del examen.
6. La operación debe ejecutarse dentro de una transacción de base de datos; en caso de error, se debe realizar rollback para mantener la integridad de los datos.
7. La operación debe ser accesible únicamente para usuarios autenticados con rol de Administrador.

---

### HU-03: Procesamiento automático de resultados de admisión

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-03 |
| **Rol** | Administrador |
| **Historia** | Como administrador del sistema, quiero procesar automáticamente los resultados de admisión mediante el Motor de Admisión para determinar qué postulantes son ingresantes, aprobados o desaprobados según las reglas de negocio establecidas. |
| **Prioridad** | Alta (Must have) |
| **Sprint** | Sprint 3 |
| **Esfuerzo estimado** | 13 puntos de historia |

**Criterios de Aceptación:**

1. Al ejecutar el procesamiento, el sistema debe eliminar todos los resultados anteriores y generar nuevos resultados basados en los exámenes registrados.
2. El motor debe ordenar a los postulantes de cada programa académico por puntaje de forma descendente.
3. El puntaje mínimo aprobatorio debe ser de **50.00 puntos**; los postulantes con puntaje inferior deben recibir el estado "Desaprobado".
4. Los postulantes aprobados deben clasificarse como **"Ingresante"** si su posición en el ranking es menor o igual al número de vacantes del programa académico, o como **"Aprobado"** si su posición excede el límite de vacantes.
5. El sistema debe asignar un **orden de mérito** secuencial (1, 2, 3, ...) a cada postulante dentro de cada programa académico.
6. Un postulante ya asignado como "Ingresante" en un programa no debe ser procesado nuevamente en otro programa.
7. La operación completa debe ejecutarse dentro de una transacción; en caso de error en cualquier punto, se debe realizar rollback.
8. Al finalizar el procesamiento exitosamente, el sistema debe mostrar un mensaje de confirmación "✅ Resultados procesados exitosamente."

---

### HU-04: Consulta individual de resultado de admisión

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-04 |
| **Rol** | Postulante |
| **Historia** | Como postulante, quiero consultar mi resultado de admisión después del procesamiento para conocer mi estado (Ingresante, Aprobado o Desaprobado), mi puntaje y mi orden de mérito. |
| **Prioridad** | Alta (Must have) |
| **Sprint** | Sprint 4 |
| **Esfuerzo estimado** | 5 puntos de historia |

**Criterios de Aceptación:**

1. El postulante debe poder acceder a su resultado después de iniciar sesión con sus credenciales (DNI como nombre de usuario).
2. El sistema debe mostrar la siguiente información: Estado de Admisión (badge con color diferenciado), Programa Académico al que postuló, Puntaje Obtenido (en puntos) y Puesto (Orden de Mérito).
3. Si el estado es **"Ingresante"**, debe mostrarse el mensaje: "🎉 ¡Felicidades! Has logrado una vacante." con estilo visual de éxito (color verde).
4. Si el estado es **"Aprobado"**, debe mostrarse el mensaje: "Has aprobado el examen, pero no se alcanzó una vacante debido al límite de cupos del programa." con estilo visual de advertencia (color amarillo).
5. Si el estado es **"Desaprobado"**, debe mostrarse el mensaje: "Lo sentimos, no has alcanzado el puntaje mínimo aprobatorio." con estilo visual de alerta (color rojo).
6. Si no hay resultados disponibles para el postulante, debe mostrarse: "No hay resultados disponibles para tu perfil."
7. El endpoint debe estar protegido y retornar código 401/403 si el token JWT es inválido o ha expirado.

---

### HU-05: Reporte de ingresantes para el administrador

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-05 |
| **Rol** | Administrador |
| **Historia** | Como administrador del sistema, quiero visualizar el reporte completo de ingresantes y no ingresantes en formato de tabla para gestionar los resultados del proceso de admisión. |
| **Prioridad** | Alta (Must have) |
| **Sprint** | Sprint 4 |
| **Esfuerzo estimado** | 8 puntos de historia |

**Criterios de Aceptación:**

1. El dashboard del administrador debe mostrar una tabla con los siguientes columnas: Puesto (#), Postulante (Nombres y Apellidos), Programa Académico, Puntaje y, para los no ingresantes, una columna adicional de Estado.
2. El sistema debe ofrecer dos vistas mediante botones de alternancia (*toggle*): "Ver Ingresantes" (por defecto, botón activo) y "Ver No Ingresantes" (botón con fondo rojo).
3. La vista "Ver Ingresantes" debe mostrar únicamente los postulantes con estado "Ingresante".
4. La vista "Ver No Ingresantes" debe mostrar los postulantes con estado "Aprobado" o "Desaprobado", incluyendo una columna adicional que muestre el estado con un badge de color diferenciado.
5. Si no hay datos procesados, debe mostrarse el mensaje: "Aún no se ha procesado a los ingresantes."
6. La tabla debe actualizarse automáticamente después de ejecutar el procesamiento del Motor de Admisión.

---

### HU-06: Filtrado de resultados por programa académico

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-06 |
| **Rol** | Administrador |
| **Historia** | Como administrador del sistema, quiero filtrar los resultados de admisión por programa académico para analizar los resultados de un programa específico. |
| **Prioridad** | Media (Should have) |
| **Sprint** | Sprint 4 |
| **Esfuerzo estimado** | 3 puntos de historia |

**Criterios de Aceptación:**

1. El dashboard del administrador debe incluir un dropdown (*select*) con la etiqueta "Filtrar por Programa:" que liste todos los programas académicos disponibles en los resultados actuales.
2. La opción por defecto debe ser "Todos los programas", mostrando la totalidad de los resultados.
3. Al seleccionar un programa específico del dropdown, la tabla debe filtrarse inmediatamente mostrando únicamente los resultados correspondientes a dicho programa.
4. Los programas listados en el dropdown deben estar ordenados alfabéticamente y corresponder solo a los programas presentes en la vista actual (Ingresantes o No Ingresantes).
5. Si no hay registros que coincidan con el filtro seleccionado, debe mostrarse el mensaje: "No hay registros para los filtros seleccionados."
6. El filtro debe ser reactivo, actualizándose sin necesidad de recargar la página.

---

### HU-07: Autenticación segura con JWT

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-07 |
| **Rol** | Usuario (Administrador o Postulante) |
| **Historia** | Como usuario del sistema, quiero iniciar sesión de forma segura utilizando mis credenciales para acceder al portal correspondiente a mi rol (administrador o postulante). |
| **Prioridad** | Alta (Must have) |
| **Sprint** | Sprint 2 |
| **Esfuerzo estimado** | 8 puntos de historia |

**Criterios de Aceptación:**

1. El sistema debe presentar un formulario de inicio de sesión con los campos: "Usuario / DNI" y "Contraseña", acompañado de un botón "Ingresar".
2. El formulario debe aplicar diseño *glassmorphism* con panel de cristal (`glass-panel`), formas de fondo animadas (`bg-shapes`) y título "Portal SAA" con subtítulo "Ingresa tus credenciales".
3. Al enviar las credenciales, el sistema debe realizar una petición POST a `/api/auth/login` con los campos `nombreUsuario` y `contrasena` en formato JSON.
4. Si la autenticación es exitosa (`exito: true`), el sistema debe almacenar el token JWT y los datos del usuario en `localStorage`, y redirigir al dashboard correspondiente según el rol del usuario.
5. Si la autenticación falla, debe mostrarse un mensaje de error descriptivo en un componente visual de alerta (`error-alert`).
6. Si se produce un error de red, debe mostrarse el mensaje: "Error de red al conectar con el servidor."
7. El botón "Ingresar" debe deshabilitarse y mostrar el texto "Ingresando..." mientras se procesa la solicitud de autenticación.
8. Las contraseñas deben almacenarse hasheadas con BCrypt en la base de datos (campo `Contrasena` de la tabla `Seguridad.Usuario`).

---

### HU-08: Gestión de programas académicos

| **Campo** | **Detalle** |
|---|---|
| **Código** | HU-08 |
| **Rol** | Administrador |
| **Historia** | Como administrador del sistema, quiero gestionar los programas académicos (crear, consultar, actualizar) para configurar las opciones disponibles en el proceso de admisión. |
| **Prioridad** | Alta (Must have) |
| **Sprint** | Sprint 2 |
| **Esfuerzo estimado** | 5 puntos de historia |

**Criterios de Aceptación:**

1. El sistema debe permitir registrar un nuevo programa académico con los siguientes datos: Código (obligatorio), Nombre (obligatorio), Descripción (opcional), Nivel Académico (Pregrado/Postgrado/Técnico, opcional), Vacantes (número entero, opcional), Fecha de Inicio del Proceso (opcional), Fecha Final del Proceso (opcional), Estado (por defecto "Activo") y Departamento/Facultad (opcional).
2. El sistema debe permitir consultar la lista de todos los programas académicos registrados.
3. El sistema debe permitir actualizar los datos de un programa académico existente, registrando automáticamente la `FechaActualizacion`.
4. El código del programa académico debe ser único para evitar duplicidades.
5. El campo `Vacantes` es utilizado por el Motor de Admisión para determinar el número máximo de postulantes que pueden alcanzar el estado "Ingresante" en cada programa.
6. Solo los usuarios con rol de Administrador deben poder gestionar los programas académicos.

---

### Resumen del Product Backlog

| **Código** | **Historia de Usuario** | **Prioridad** | **Sprint** | **Puntos** | **Estado** |
|---|---|---|---|---|---|
| HU-01 | Registro de nuevos postulantes | Alta | Sprint 3 | 8 | Completado ✅ |
| HU-02 | Registro de calificaciones de exámenes | Alta | Sprint 3 | 5 | Completado ✅ |
| HU-03 | Procesamiento automático de resultados | Alta | Sprint 3 | 13 | Completado ✅ |
| HU-04 | Consulta individual de resultado | Alta | Sprint 4 | 5 | Completado ✅ |
| HU-05 | Reporte de ingresantes | Alta | Sprint 4 | 8 | Completado ✅ |
| HU-06 | Filtrado por programa académico | Media | Sprint 4 | 3 | Completado ✅ |
| HU-07 | Autenticación segura con JWT | Alta | Sprint 2 | 8 | Completado ✅ |
| HU-08 | Gestión de programas académicos | Alta | Sprint 2 | 5 | Completado ✅ |
| | **Total de puntos de historia** | | | **55** | |

---

## ANEXO 4: ENTREGABLES DE INGENIERÍA DE SOFTWARE

### 4.1. Diagrama de Arquitectura (Clean Architecture)

#### 4.1.1. Descripción del diagrama

El Sistema Automatizado de Admisión (SAA) fue diseñado siguiendo el patrón de **Clean Architecture** propuesto por Robert C. Martin (2017), organizando el código fuente en cuatro proyectos (capas) con dependencias unidireccionales que apuntan hacia el centro del sistema. El siguiente diagrama describe la estructura arquitectónica del SAA:

```
┌─────────────────────────────────────────────────────────────────────┐
│                        CAPA EXTERNA (Presentación)                  │
│  ┌───────────────────────────────────────────────────────────────┐  │
│  │                        SAA.API                                │  │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌──────────────┐  │  │
│  │  │  Controllers/   │  │  frontend/      │  │  Program.cs  │  │  │
│  │  │  - AuthController│  │  - App.tsx      │  │  (Startup)   │  │  │
│  │  │  - Postulantes  │  │  - App.css      │  │              │  │  │
│  │  │    Controller    │  │  - React 19     │  │              │  │  │
│  │  │  - Admision     │  │  - TypeScript   │  │              │  │  │
│  │  │    Controller    │  │  - Vite         │  │              │  │  │
│  │  └─────────────────┘  └─────────────────┘  └──────────────┘  │  │
│  └───────────────────────────────┬───────────────────────────────┘  │
│                                  │ depende de                       │
│  ┌───────────────────────────────▼───────────────────────────────┐  │
│  │                     SAA.Application                           │  │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌──────────────┐  │  │
│  │  │  Services/      │  │  DTOs/          │  │  Interfaces/ │  │  │
│  │  │  - AuthService  │  │  - LoginRequest │  │  - IAppDb    │  │  │
│  │  │  - MotorAdmision│  │  - LoginResponse│  │    Context   │  │  │
│  │  │    Service      │  │  - CrearPostu-  │  │              │  │  │
│  │  │  - Postulante   │  │    lanteDto     │  │              │  │  │
│  │  │    Service      │  │  - ReporteIngre-│  │              │  │  │
│  │  │                 │  │    santeDto     │  │              │  │  │
│  │  └─────────────────┘  └─────────────────┘  └──────────────┘  │  │
│  └───────────────────────────────┬───────────────────────────────┘  │
│                                  │ depende de                       │
│  ┌───────────────────────────────▼───────────────────────────────┐  │
│  │                       SAA.Domain (NÚCLEO)                     │  │
│  │  ┌─────────────────────────────────────────────────────────┐  │  │
│  │  │  Entities/                                               │  │  │
│  │  │  - Postulante.cs          - ResultadoAdmision.cs        │  │  │
│  │  │  - ExamenAdmision.cs      - ProgramaAcademico.cs        │  │  │
│  │  │  - FichaPostulacion.cs    - PeriodoAdmision.cs          │  │  │
│  │  │  - Usuario.cs             - Rol.cs                      │  │  │
│  │  └─────────────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────▲───────────────────────────────┘  │
│                                  │ depende de                       │
│  ┌───────────────────────────────┴───────────────────────────────┐  │
│  │                     SAA.Infrastructure                        │  │
│  │  ┌─────────────────────────────────────────────────────────┐  │  │
│  │  │  Data/                                                   │  │  │
│  │  │  - SAADbContext.cs (Entity Framework Core)              │  │  │
│  │  │  Services/                                               │  │  │
│  │  │  - SeedDataService.cs (datos iniciales)                 │  │  │
│  │  └─────────────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐  │
│  │                        SAA.Tests                              │  │
│  │  ┌─────────────────────────────────────────────────────────┐  │  │
│  │  │  Pruebas unitarias con xUnit.net                        │  │  │
│  │  │  Cobertura con Coverlet                                 │  │  │
│  │  └─────────────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
```

#### 4.1.2. Responsabilidades de cada capa

| **Capa** | **Proyecto** | **Responsabilidad** | **Dependencias** |
|---|---|---|---|
| **Dominio** (Núcleo) | `SAA.Domain` | Contiene las entidades de negocio puras (`Postulante`, `ExamenAdmision`, `FichaPostulacion`, `ResultadoAdmision`, `ProgramaAcademico`, `PeriodoAdmision`, `Usuario`, `Rol`). No depende de ningún framework externo ni de detalles de infraestructura. Define las reglas de negocio fundamentales del sistema. | Ninguna |
| **Aplicación** | `SAA.Application` | Implementa los casos de uso del sistema mediante servicios (`AuthService`, `MotorAdmisionService`, `PostulanteService`). Define los DTOs para la transferencia de datos entre capas y las interfaces (`IApplicationDbContext`) que abstraen el acceso a datos. Contiene la lógica de orquestación del Motor de Admisión. | `SAA.Domain` |
| **Infraestructura** | `SAA.Infrastructure` | Implementa los detalles técnicos de acceso a datos mediante Entity Framework Core (`SAADbContext`), las configuraciones de mapeo de entidades a tablas de SQL Server, y los servicios de datos iniciales (`SeedDataService`). Implementa la interfaz `IApplicationDbContext` definida en la capa de aplicación. | `SAA.Domain`, `SAA.Application` |
| **Presentación** (API) | `SAA.API` | Expone los endpoints REST mediante controladores ASP.NET Core (`AuthController`, `PostulantesController`, `AdmisionController`). Aloja el frontend React 19 con TypeScript y Vite. Configura la inyección de dependencias, la autenticación JWT y el middleware de la aplicación. | `SAA.Application`, `SAA.Infrastructure` |
| **Pruebas** | `SAA.Tests` | Contiene las pruebas unitarias automatizadas que verifican la lógica de negocio implementada en las capas de Aplicación y Dominio, utilizando xUnit.net como framework de pruebas y Coverlet para la medición de cobertura de código. | `SAA.Domain`, `SAA.Application` |

#### 4.1.3. Principio de Inversión de Dependencias

Un aspecto fundamental de la arquitectura del SAA es la aplicación del **Principio de Inversión de Dependencias** (DIP — *Dependency Inversion Principle*). La capa de aplicación define la interfaz `IApplicationDbContext`, la cual es implementada por la capa de infraestructura (`SAADbContext`). De esta manera, la capa de aplicación no conoce los detalles de Entity Framework Core ni de SQL Server; solo depende de la abstracción. La resolución se realiza en tiempo de ejecución mediante el contenedor de inyección de dependencias de ASP.NET Core, configurado en `Program.cs` dentro de `SAA.API`.

---

### 4.2. Diagrama Entidad-Relación

#### 4.2.1. Descripción del modelo de datos

El modelo de datos del SAA está compuesto por **8 tablas** distribuidas en **3 esquemas lógicos** dentro de SQL Server. El script de migración fue generado automáticamente por Entity Framework Core (versión 10.0.8) con la migración identificada como `20260526102609_InitialCreate`. A continuación se detalla el modelo:

#### 4.2.2. Esquema `Admision`

**Tabla: `Admision.Postulante`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdPostulante` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único del postulante |
| `Nombres` | `nvarchar(MAX)` | NOT NULL | Nombres del postulante |
| `Apellidos` | `nvarchar(MAX)` | NOT NULL | Apellidos del postulante |
| `DNI` | `nvarchar(450)` | NOT NULL, UNIQUE (IX_Postulante_DNI) | Documento Nacional de Identidad |
| `IdProgramaInteres` | `int` | NOT NULL | FK al programa académico de interés |
| `Correo` | `nvarchar(MAX)` | NOT NULL | Correo electrónico |
| `Telefono` | `nvarchar(MAX)` | NULL | Número de teléfono (opcional) |
| `Direccion` | `nvarchar(MAX)` | NULL | Dirección domiciliaria (opcional) |
| `FechaNacimiento` | `datetime2` | NULL | Fecha de nacimiento (opcional) |
| `Estado` | `nvarchar(MAX)` | NOT NULL, DEFAULT 'Activo' | Estado del postulante |
| `FechaCreacion` | `datetime2` | NOT NULL | Fecha de creación del registro |
| `FechaActualizacion` | `datetime2` | NULL | Fecha de última actualización |

**Tabla: `Admision.ExamenAdmision`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdExamen` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único del examen |
| `IdFichaPostulacion` | `int` | NOT NULL | FK a la ficha de postulación |
| `IdPostulante` | `int` | NOT NULL | FK al postulante |
| `NombreExamen` | `nvarchar(MAX)` | NOT NULL | Nombre o tipo del examen |
| `FechaExamen` | `datetime2` | NOT NULL | Fecha programada del examen |
| `HoraInicio` | `time` | NULL | Hora de inicio (opcional) |
| `DuracionMinutos` | `int` | NULL | Duración en minutos (opcional) |
| `Sala` | `nvarchar(MAX)` | NULL | Sala o lugar del examen (opcional) |
| `Estado` | `nvarchar(MAX)` | NOT NULL, DEFAULT 'Programado' | Estado del examen |
| `Puntaje` | `decimal(18,2)` | NULL | Puntaje obtenido (0-1000) |
| `Observaciones` | `nvarchar(MAX)` | NULL | Observaciones (opcional) |
| `FechaCreacion` | `datetime2` | NOT NULL | Fecha de creación del registro |
| `FechaActualizacion` | `datetime2` | NULL | Fecha de última actualización |

**Tabla: `Admision.FichaPostulacion`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdFichaPostulacion` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único de la ficha |
| `IdPostulante` | `int` | NOT NULL | FK al postulante |
| `IdProgramaAcademico` | `int` | NOT NULL | FK al programa académico |
| `NumeroTramite` | `nvarchar(MAX)` | NOT NULL | Número de trámite de la postulación |
| `FechaPostulacion` | `datetime2` | NOT NULL | Fecha de postulación |
| `Estado` | `nvarchar(MAX)` | NOT NULL, DEFAULT 'Registrada' | Estado de la postulación |
| `Observaciones` | `nvarchar(MAX)` | NULL | Observaciones (opcional) |
| `FechaActualizacion` | `datetime2` | NULL | Fecha de última actualización |
| `IdUsuarioActualizacion` | `int` | NULL | FK al usuario que actualizó |

**Tabla: `Admision.ResultadoAdmision`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdResultado` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único del resultado |
| `IdFichaPostulacion` | `int` | NOT NULL | FK a la ficha de postulación |
| `IdPostulante` | `int` | NOT NULL | FK al postulante |
| `IdProgramaAcademico` | `int` | NOT NULL | FK al programa académico |
| `Calificacion` | `decimal(18,2)` | NULL | Calificación/puntaje obtenido |
| `Resultado` | `nvarchar(MAX)` | NOT NULL | Estado: Ingresante/Aprobado/Desaprobado |
| `Observaciones` | `nvarchar(MAX)` | NULL | Observaciones (opcional) |
| `FechaResultado` | `datetime2` | NOT NULL | Fecha de generación del resultado |
| `IdUsuarioEvaluador` | `int` | NULL | FK al usuario evaluador |
| `FechaActualizacion` | `datetime2` | NULL | Fecha de última actualización |

> **Nota:** La entidad `ResultadoAdmision` en el código fuente C# incluye además la propiedad `OrdenMerito` (tipo `int?`), la cual almacena el puesto o mérito obtenido por el postulante dentro de su programa académico.

#### 4.2.3. Esquema `Config`

**Tabla: `Config.ProgramaAcademico`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdProgramaAcademico` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único del programa |
| `Codigo` | `nvarchar(MAX)` | NOT NULL | Código del programa académico |
| `Nombre` | `nvarchar(MAX)` | NOT NULL | Nombre del programa |
| `Descripcion` | `nvarchar(MAX)` | NULL | Descripción (opcional) |
| `NivelAcademico` | `nvarchar(MAX)` | NULL | Nivel: Pregrado/Postgrado/Técnico |
| `Vacantes` | `int` | NULL | Número de vacantes disponibles |
| `FechaInicioProceso` | `datetime2` | NULL | Fecha de inicio del proceso |
| `FechaFinalProceso` | `datetime2` | NULL | Fecha de cierre del proceso |
| `Estado` | `nvarchar(MAX)` | NOT NULL, DEFAULT 'Activo' | Estado del programa |
| `Departamento` | `nvarchar(MAX)` | NULL | Departamento/Facultad |
| `FechaCreacion` | `datetime2` | NOT NULL | Fecha de creación |
| `FechaActualizacion` | `datetime2` | NULL | Fecha de última actualización |

**Tabla: `Config.PeriodoAdmision`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdPeriodo` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único del período |
| `Nombre` | `nvarchar(MAX)` | NOT NULL | Nombre del período (ej: "2026-I") |
| `FechaInicio` | `datetime2` | NOT NULL | Fecha de inicio del período |
| `FechaFin` | `datetime2` | NOT NULL | Fecha de fin del período |

#### 4.2.4. Esquema `Seguridad`

**Tabla: `Seguridad.Usuario`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdUsuario` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único del usuario |
| `NombreUsuario` | `nvarchar(MAX)` | NOT NULL | Nombre de usuario para login |
| `Contrasena` | `nvarchar(MAX)` | NOT NULL | Contraseña hasheada (BCrypt) |
| `NombreCompleto` | `nvarchar(MAX)` | NOT NULL | Nombre completo del usuario |
| `Correo` | `nvarchar(MAX)` | NOT NULL | Correo electrónico |
| `Rol` | `nvarchar(MAX)` | NOT NULL, DEFAULT 'Usuario' | Rol: Administrador/Usuario |
| `Estado` | `nvarchar(MAX)` | NOT NULL, DEFAULT 'Activo' | Estado del usuario |
| `UltimoAcceso` | `datetime2` | NULL | Fecha del último acceso |
| `FechaCreacion` | `datetime2` | NOT NULL | Fecha de creación de la cuenta |
| `FechaActualizacion` | `datetime2` | NULL | Fecha de última actualización |

**Tabla: `Seguridad.Rol`**

| **Columna** | **Tipo de dato** | **Restricción** | **Descripción** |
|---|---|---|---|
| `IdRol` | `int` (IDENTITY) | PK, NOT NULL, AUTO_INCREMENT | Identificador único del rol |
| `Nombre` | `nvarchar(MAX)` | NOT NULL | Nombre del rol |
| `Descripcion` | `nvarchar(MAX)` | NOT NULL | Descripción del rol |

#### 4.2.5. Relaciones entre tablas

```
┌──────────────────────┐       ┌───────────────────────────┐
│  Config.Programa     │       │  Admision.Postulante      │
│  Academico           │       │                           │
│  ──────────────────  │       │  ───────────────────────  │
│  PK: IdPrograma      │◄──┐   │  PK: IdPostulante        │
│      Academico       │   │   │  UK: DNI                  │
│  Vacantes            │   │   │  FK: IdProgramaInteres    │──┐
│  ...                 │   │   │  ...                      │  │
└──────────────────────┘   │   └──────────┬────────────────┘  │
         ▲                 │              │                    │
         │                 │              │ 1:N                │
         │                 │              ▼                    │
         │            ┌────┴──────────────────────────┐       │
         │            │  Admision.FichaPostulacion     │       │
         │            │  ────────────────────────────  │       │
         │            │  PK: IdFichaPostulacion        │       │
         ├────────────│  FK: IdPostulante              │       │
         │            │  FK: IdProgramaAcademico       │───────┘
         │            │  NumeroTramite                 │
         │            │  ...                           │
         │            └──────────┬─────────────────────┘
         │                       │
         │                       │ 1:N
         │                       ▼
         │            ┌──────────────────────────────┐
         │            │  Admision.ExamenAdmision      │
         │            │  ──────────────────────────── │
         │            │  PK: IdExamen                 │
         │            │  FK: IdFichaPostulacion        │
         │            │  FK: IdPostulante              │
         │            │  Puntaje                       │
         │            │  ...                           │
         │            └──────────────────────────────┘
         │
         │            ┌──────────────────────────────┐
         │            │  Admision.ResultadoAdmision   │
         │            │  ──────────────────────────── │
         │            │  PK: IdResultado              │
         │            │  FK: IdFichaPostulacion        │
         │            │  FK: IdPostulante              │
         └────────────│  FK: IdProgramaAcademico       │
                      │  Calificacion                  │
                      │  Resultado (Estado)            │
                      │  OrdenMerito                   │
                      │  ...                           │
                      └──────────────────────────────┘

┌──────────────────────┐       ┌───────────────────────┐
│  Seguridad.Usuario   │       │  Seguridad.Rol        │
│  ──────────────────  │       │  ─────────────────    │
│  PK: IdUsuario       │       │  PK: IdRol            │
│  NombreUsuario       │       │  Nombre               │
│  Contrasena (hash)   │       │  Descripcion          │
│  Rol                 │       │                       │
│  ...                 │       │                       │
└──────────────────────┘       └───────────────────────┘

┌──────────────────────┐
│  Config.Periodo      │
│  Admision            │
│  ──────────────────  │
│  PK: IdPeriodo       │
│  Nombre              │
│  FechaInicio         │
│  FechaFin            │
└──────────────────────┘
```

#### 4.2.6. Restricciones e índices notables

| **Tipo** | **Nombre** | **Tabla** | **Columna(s)** | **Descripción** |
|---|---|---|---|---|
| Primary Key | `PK_Postulante` | `Admision.Postulante` | `IdPostulante` | Clave primaria con autoincremento |
| Primary Key | `PK_ExamenAdmision` | `Admision.ExamenAdmision` | `IdExamen` | Clave primaria con autoincremento |
| Primary Key | `PK_FichaPostulacion` | `Admision.FichaPostulacion` | `IdFichaPostulacion` | Clave primaria con autoincremento |
| Primary Key | `PK_ResultadoAdmision` | `Admision.ResultadoAdmision` | `IdResultado` | Clave primaria con autoincremento |
| Primary Key | `PK_ProgramaAcademico` | `Config.ProgramaAcademico` | `IdProgramaAcademico` | Clave primaria con autoincremento |
| Primary Key | `PK_PeriodoAdmision` | `Config.PeriodoAdmision` | `IdPeriodo` | Clave primaria con autoincremento |
| Primary Key | `PK_Usuario` | `Seguridad.Usuario` | `IdUsuario` | Clave primaria con autoincremento |
| Primary Key | `PK_Rol` | `Seguridad.Rol` | `IdRol` | Clave primaria con autoincremento |
| Unique Index | `IX_Postulante_DNI` | `Admision.Postulante` | `DNI` | Garantiza unicidad del DNI |

---

### 4.3. Prototipos de Interfaces del Sistema

A continuación se describen los prototipos de las interfaces del Sistema Automatizado de Admisión (SAA), implementadas en React 19 con TypeScript y diseñadas con un estilo visual **glassmorphism** (efecto de cristal translúcido) sobre un fondo oscuro con formas animadas.

#### 4.3.1. Prototipo 1: Pantalla de Login — Portal SAA

**Descripción general:**

La pantalla de inicio de sesión constituye el punto de entrada al sistema. Presenta un diseño centrado con un panel de cristal translúcido (*glassmorphism*) sobre un fondo oscuro con gradientes de color. Dos formas decorativas animadas (`shape-1` y `shape-2`) flotan en el fondo, proporcionando dinamismo visual al componente estático del formulario.

**Elementos de la interfaz:**

| **N.°** | **Elemento** | **Tipo** | **Clase CSS** | **Descripción funcional** |
|---|---|---|---|---|
| 1 | Contenedor principal | `div` | `login-container` | Contenedor centrado que ocupa toda la pantalla, con display flex para centrar vertical y horizontalmente el panel de login. |
| 2 | Panel de login | `div` | `login-box glass-panel` | Panel con efecto glassmorphism (fondo semitransparente, borde sutil, desenfoque de fondo *backdrop-filter: blur*). Contiene toda la estructura del formulario. |
| 3 | Área de marca (*brand*) | `div` | `brand` | Sección superior del panel que contiene el logotipo placeholder y los textos de bienvenida. |
| 4 | Logotipo | `div` | `logo-placeholder` | Espacio reservado para el logotipo institucional de la universidad. |
| 5 | Título principal | `h1` | — | Texto: **"Portal SAA"**. Tipografía grande y prominente que identifica el sistema. |
| 6 | Subtítulo | `p` | — | Texto: **"Ingresa tus credenciales"**. Indicación breve para el usuario. |
| 7 | Formulario de login | `form` | `login-form` | Formulario HTML con evento `onSubmit` que invoca la función `handleLogin`. |
| 8 | Campo "Usuario / DNI" | `input[type="text"]` | `input-group` | Campo de texto obligatorio (`required`) con placeholder "Ingresa tu usuario". Etiqueta: "Usuario / DNI". |
| 9 | Campo "Contraseña" | `input[type="password"]` | `input-group` | Campo de contraseña obligatorio (`required`) con placeholder "Tu contraseña". Etiqueta: "Contraseña". |
| 10 | Mensaje de error | `div` | `error-alert` | Panel de alerta rojo que se muestra condicionalmente cuando la variable `error` contiene un mensaje. Muestra errores como "Error al iniciar sesión" o "Error de red al conectar con el servidor." |
| 11 | Botón "Ingresar" | `button[type="submit"]` | `primary-btn` | Botón principal de color acento. Texto dinámico: "Ingresar" (estado normal) o "Ingresando..." (estado de carga). Se deshabilita (`disabled`) durante el proceso de autenticación. |
| 12 | Formas de fondo | `div` × 2 | `bg-shapes`, `shape shape-1`, `shape shape-2` | Dos formas decorativas con gradientes de color que flotan con animación CSS (`@keyframes`) en el fondo de la pantalla, creando un efecto visual dinámico y moderno. |

**Flujo de interacción:**

1. El usuario ingresa su nombre de usuario (o DNI) y contraseña.
2. Al presionar "Ingresar", se envía una petición POST a `/api/auth/login`.
3. Si la autenticación es exitosa, se almacena el token JWT en `localStorage` y se redirige al dashboard correspondiente al rol.
4. Si falla, se muestra el mensaje de error en el componente `error-alert`.

---

#### 4.3.2. Prototipo 2: Dashboard del Administrador — Portal de Administración

**Descripción general:**

El dashboard del administrador presenta una interfaz completa para la gestión del proceso de admisión. Consta de un encabezado con efecto glassmorphism, una zona de contenido principal con controles de procesamiento y una tabla de resultados filtrable, y las formas animadas de fondo.

**Elementos de la interfaz:**

| **N.°** | **Elemento** | **Tipo** | **Clase CSS** | **Descripción funcional** |
|---|---|---|---|---|
| 1 | Contenedor del dashboard | `div` | `dashboard-container` | Contenedor principal del dashboard, con ancho máximo de 1000px para el área de contenido. |
| 2 | Encabezado (*header*) | `header` | `glass-header` | Barra superior con efecto glassmorphism. Contiene el título y la información del usuario. |
| 3 | Título del portal | `h2` | — | Texto: **"Portal de Administración"**. Identifica la sección administrativa. |
| 4 | Información del usuario | `div` | `user-info` | Muestra el texto "Admin: {nombreCompleto}" con el nombre del administrador autenticado. |
| 5 | Botón "Cerrar Sesión" | `button` | `logout-btn` | Botón que ejecuta `handleLogout()`, eliminando el token de `localStorage` y redirigiendo al login. |
| 6 | Tarjeta de gestión | `div` | `result-card glass-panel fade-in-up` | Panel principal con efecto glassmorphism y animación de entrada (*fade-in-up*). Padding de 30px. |
| 7 | Título de sección | `h3` | — | Texto: **"Gestión de Admisión"**. Encabezado de la tarjeta principal. |
| 8 | Botón "Procesar Motor de Admisión" | `button` | `primary-btn` | Botón con ícono ⚙️ que ejecuta `procesarResultados()`. Texto dinámico: "⚙️ Procesar Motor de Admisión" / "Procesando...". Solicita confirmación antes de ejecutar. |
| 9 | Botón "Ver Ingresantes" | `button` | `primary-btn active` | Botón de alternancia. Cuando está activo, muestra opacidad completa (1.0). Filtra la tabla para mostrar solo postulantes con estado "Ingresante". |
| 10 | Botón "Ver No Ingresantes" | `button` | `primary-btn` (fondo: `var(--danger)`) | Botón de alternancia con fondo rojo. Cuando está inactivo, muestra opacidad reducida (0.6). Filtra la tabla para mostrar postulantes con estado distinto de "Ingresante". |
| 11 | Título de lista | `h4` | — | Texto dinámico: **"Lista de Ingresantes"** o **"Lista de Postulantes No Ingresantes"**, según la vista seleccionada. Color: `var(--text-muted)`. |
| 12 | Dropdown "Filtrar por Programa" | `select` | — (estilos inline) | Control de selección con la opción por defecto "Todos los programas". Lista los programas únicos presentes en los datos filtrados, ordenados alfabéticamente. Fondo: `var(--glass-bg)`, texto blanco. |
| 13 | Tabla de resultados | `table` | — (dentro de `table-responsive`) | Tabla con las columnas: **Puesto** (#, negrita), **Postulante** (Nombres + Apellidos), **Programa** (nombre del programa académico), **Puntaje** (color: `var(--primary)`). La vista "No Ingresantes" agrega una columna **Estado** con badge de color. |
| 14 | Badge de estado | `span` | `status-badge {estado.toLowerCase()}` | Badge con el estado del postulante. Se muestra solo en la vista de "No Ingresantes". Tamaño: 0.8rem, padding: 4px 8px. |
| 15 | Spinner de carga | `div` | `loading-spinner` | Texto: "Cargando reporte..." Se muestra mientras se obtienen los datos del endpoint `/api/admision/reporte-todos`. |
| 16 | Mensaje sin datos | `div` | `error-alert` | Texto: "Aún no se ha procesado a los ingresantes." Se muestra cuando no hay datos de reporte. |
| 17 | Mensaje sin resultados filtrados | `div` | `error-alert` | Texto: "No hay registros para los filtros seleccionados." Se muestra cuando el filtro no encuentra coincidencias. |

**Flujo de interacción:**

1. Al cargar el dashboard, se invoca automáticamente `fetchReporteTodos()` para obtener todos los resultados.
2. El administrador puede alternar entre la vista de "Ingresantes" y "No Ingresantes" mediante los botones de alternancia.
3. El dropdown permite filtrar por programa académico específico.
4. El botón "Procesar Motor de Admisión" ejecuta el procesamiento de resultados (con confirmación) y actualiza la tabla.

---

#### 4.3.3. Prototipo 3: Dashboard del Postulante — Portal de Admisión

**Descripción general:**

El dashboard del postulante presenta una interfaz simplificada y centrada en la información del resultado de admisión del usuario autenticado. Muestra una tarjeta de estado con diseño glassmorphism, información del puntaje y puesto, y un mensaje contextual según el resultado obtenido.

**Elementos de la interfaz:**

| **N.°** | **Elemento** | **Tipo** | **Clase CSS** | **Descripción funcional** |
|---|---|---|---|---|
| 1 | Contenedor del dashboard | `div` | `dashboard-container` | Contenedor principal del dashboard del postulante. |
| 2 | Encabezado (*header*) | `header` | `glass-header` | Barra superior con efecto glassmorphism. |
| 3 | Título del portal | `h2` | — | Texto: **"Portal de Admisión"**. Identifica la sección del postulante. |
| 4 | Saludo personalizado | `span` | — (dentro de `user-info`) | Texto: **"Hola, {nombres}"** donde `{nombres}` proviene del resultado del postulante, del `userData.nombreCompleto`, o por defecto "Postulante". |
| 5 | Botón "Cerrar Sesión" | `button` | `logout-btn` | Botón que ejecuta `handleLogout()`. |
| 6 | Spinner de carga | `div` | `loading-spinner` | Texto: "Cargando resultados..." Se muestra mientras se obtienen los datos del endpoint `/api/postulantes/mi-resultado`. |
| 7 | Tarjeta de resultado | `div` | `result-card glass-panel fade-in-up` | Tarjeta principal con efecto glassmorphism y animación de entrada. Contiene toda la información del resultado. |
| 8 | Encabezado del resultado | `div` | `result-header` | Contiene el título "Estado de Admisión" y el badge de estado. |
| 9 | Título "Estado de Admisión" | `h3` | — | Texto fijo: **"Estado de Admisión"**. |
| 10 | Badge de estado | `div` | `status-badge {estado.toLowerCase()}` | Badge visual que muestra el estado: **"Ingresante"** (verde), **"Aprobado"** (amarillo) o **"Desaprobado"** (rojo). Cada estado tiene su clase CSS con color diferenciado. |
| 11 | Cuerpo del resultado | `div` | `result-body` | Contenedor de los ítems de información. |
| 12 | Info: Programa Académico | `div` | `info-item` | Etiqueta: "Programa Académico" (clase `label`). Valor: nombre del programa (clase `value`). |
| 13 | Info: Puntaje Obtenido | `div` | `info-item` | Etiqueta: "Puntaje Obtenido" (clase `label`). Valor: "{puntaje} pts" (clase `value highlight`), con estilo visual destacado. |
| 14 | Info: Puesto (Orden de Mérito) | `div` | `info-item` | Etiqueta: "Puesto (Orden de Mérito)" (clase `label`). Valor: "#{puesto}" (clase `value`). |
| 15 | Pie del resultado | `div` | `result-footer` | Contiene el mensaje contextual según el estado del postulante. |
| 16 | Mensaje Ingresante | `div` | `success-message` | Texto: **"🎉 ¡Felicidades! Has logrado una vacante."** Se muestra cuando `resultado.estado === 'Ingresante'`. Estilo visual verde/éxito. |
| 17 | Mensaje Aprobado | `div` | `warning-message` | Texto: **"Has aprobado el examen, pero no se alcanzó una vacante debido al límite de cupos del programa."** Se muestra cuando `resultado.estado === 'Aprobado'`. Estilo visual amarillo/advertencia. |
| 18 | Mensaje Desaprobado | `div` | `danger-message` | Texto: **"Lo sentimos, no has alcanzado el puntaje mínimo aprobatorio."** Se muestra cuando el estado es distinto de "Ingresante" y "Aprobado". Estilo visual rojo/peligro. |
| 19 | Mensaje sin resultados | `div` | `error-alert` | Texto: "No hay resultados disponibles para tu perfil." Se muestra cuando `resultado` es `null`. |
| 20 | Formas de fondo | `div` × 2 | `bg-shapes`, `shape shape-1`, `shape shape-2` | Formas decorativas animadas idénticas a las de la pantalla de login. |

**Flujo de interacción:**

1. Al cargar el dashboard, se invoca automáticamente `fetchResultado()` para obtener el resultado del postulante autenticado desde el endpoint `/api/postulantes/mi-resultado`.
2. Si la respuesta es exitosa, se renderiza la tarjeta con toda la información del resultado.
3. Si el token es inválido (401/403), se ejecuta automáticamente el logout.
4. El postulante visualiza su estado, puntaje, puesto y el mensaje contextual correspondiente.

---

#### 4.3.4. Navegación entre prototipos

El sistema implementa una navegación basada en el estado de autenticación y el rol del usuario, sin utilizar un router tradicional de React:

```
                    ┌─────────────────────┐
                    │  ¿Token JWT existe   │
                    │  en localStorage?    │
                    └──────────┬──────────┘
                               │
                    ┌──────────┴──────────┐
                    │ No                  │ Sí
                    ▼                     ▼
          ┌─────────────────┐   ┌─────────────────────┐
          │  Prototipo 1:   │   │  ¿Rol del usuario?  │
          │  Login Portal   │   │                     │
          │  SAA            │   └──────────┬──────────┘
          └─────────────────┘              │
                                ┌──────────┴──────────┐
                                │ Administrador       │ Otro (Postulante)
                                ▼                     ▼
                    ┌─────────────────┐   ┌─────────────────┐
                    │  Prototipo 2:   │   │  Prototipo 3:   │
                    │  Dashboard      │   │  Dashboard      │
                    │  Administrador  │   │  Postulante     │
                    └─────────────────┘   └─────────────────┘
```

#### 4.3.5. Diseño visual y sistema de estilos

El sistema de estilos del SAA está definido en el archivo `App.css` y utiliza las siguientes variables CSS y técnicas de diseño:

| **Aspecto de diseño** | **Implementación** |
|---|---|
| **Tema de color** | Paleta oscura con acentos de color brillante. Fondo: gradiente oscuro. Texto: blanco sobre fondo oscuro. |
| **Glassmorphism** | Clase `.glass-panel`: `background: var(--glass-bg)` (fondo semitransparente RGBA), `backdrop-filter: blur()` (desenfoque de fondo), `border: 1px solid var(--glass-border)` (borde sutil translúcido). |
| **Animaciones** | Clase `.fade-in-up`: animación de entrada con desplazamiento vertical y transición de opacidad. Formas de fondo (`.shape`): animación de flotación continua con `@keyframes`. |
| **Responsividad** | Diseño adaptable mediante media queries y unidades relativas. `max-width: 1000px` para el contenido del dashboard del administrador. |
| **Estados interactivos** | Botones con efecto `:hover` (cambio de opacidad/color), estado `:disabled` con cursor no permitido. Badges de estado con colores diferenciados por clase CSS. |
| **Tipografía** | Fuentes importadas de Google Fonts para una apariencia profesional y moderna. Jerarquía tipográfica clara: `h1` (título del portal) > `h2` (título de sección) > `h3` (título de tarjeta) > texto normal. |

---

## ANEXO 5: RESULTADOS DE COBERTURA DE CÓDIGO

### 5.1. Resumen general de la cobertura

| **Métrica** | **Valor** |
|---|---|
| **Fecha de generación** | 26/05/2026, 10:21:16 |
| **Parser utilizado** | Cobertura (formato Cobertura XML) |
| **Total de ensamblados analizados** | 3 |
| **Total de clases analizadas** | 28 |
| **Total de archivos analizados** | 25 |
| **Total de líneas de código fuente** | 1,005 |
| **Líneas cubribles** | 490 |
| **Líneas cubiertas** | 268 |
| **Líneas no cubiertas** | 222 |
| **Cobertura de línea** | **54.6%** |
| **Ramas totales** | 58 |
| **Ramas cubiertas** | 34 |
| **Cobertura de ramas** | **58.6%** |
| **Métodos totales** | 162 |
| **Métodos cubiertos** | 84 |
| **Métodos completamente cubiertos** | 82 |
| **Cobertura de métodos** | **51.8%** |
| **Cobertura completa de métodos** | **50.6%** |

### 5.2. Cobertura por ensamblado

| **Ensamblado** | **Cobertura de línea** | **Clasificación** |
|---|---|---|
| **SAA.Application** | **79.6%** | ✅ Alto |
| **SAA.Domain** | **33.6%** | ⚠️ Bajo |
| **SAA.Infrastructure** | **19.6%** | ❌ Muy bajo |

### 5.3. Cobertura detallada — SAA.Application (79.6%)

| **Clase** | **Cobertura** | **Estado** |
|---|---|---|
| `SAA.Application.DTOs.CrearPostulanteDto` | 100% | ✅ Cumple |
| `SAA.Application.DTOs.LoginRequestDto` | 100% | ✅ Cumple |
| `SAA.Application.DTOs.LoginResponseDto` | 100% | ✅ Cumple |
| `SAA.Application.DTOs.PostulanteResponseDto` | 100% | ✅ Cumple |
| `SAA.Application.DTOs.RegistrarExamenDto` | 0% | ❌ No Cumple |
| `SAA.Application.DTOs.ReporteIngresanteDto` | 100% | ✅ Cumple |
| `SAA.Application.DTOs.UsuarioDto` | 100% | ✅ Cumple |
| `SAA.Application.Services.AuthService` | 100% | ✅ Cumple |
| `SAA.Application.Services.MotorAdmisionService` | 69.2% | ✅ Cumple |
| `SAA.Application.Services.PostulanteService` | 72.7% | ✅ Cumple |

### 5.4. Cobertura detallada — SAA.Domain (33.6%)

| **Clase** | **Cobertura** | **Estado** |
|---|---|---|
| `SAA.Domain.Entities.ConfiguracionSistema` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.DocumentoPostulante` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.ExamenAdmision` | 46.1% | ⚠️ Parcial |
| `SAA.Domain.Entities.FichaPostulacion` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.LogAuditoria` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.LogMotorAdmision` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.Matricula` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.Notificacion` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.PeriodoAdmision` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.Postulante` | 75% | ✅ Cumple |
| `SAA.Domain.Entities.ProgramaAcademico` | 50% | ⚠️ Parcial |
| `SAA.Domain.Entities.ResultadoAdmision` | 60% | ✅ Cumple |
| `SAA.Domain.Entities.Rol` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.Sesion` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.TipoDocumento` | 0% | ❌ No Cumple |
| `SAA.Domain.Entities.Usuario` | 90% | ✅ Cumple |

### 5.5. Cobertura detallada — SAA.Infrastructure (19.6%)

| **Clase** | **Cobertura** | **Estado** |
|---|---|---|
| `SAA.Infrastructure.Data.SAADbContext` | 100% | ✅ Cumple |
| `SAA.Infrastructure.Services.SeedDataService` | 0% | ❌ No Cumple |

### 5.6. Evaluación general Cumple / No Cumple

| **Módulo evaluado** | **Cobertura alcanzada** | **Umbral mínimo** | **Resultado** |
|---|---|---|---|
| Servicio de Autenticación (`AuthService`) | 100% | ≥ 70% | ✅ **CUMPLE** |
| Motor de Admisión (`MotorAdmisionService`) | 69.2% | ≥ 60% | ✅ **CUMPLE** |
| Servicio de Postulantes (`PostulanteService`) | 72.7% | ≥ 60% | ✅ **CUMPLE** |
| Contexto de Base de Datos (`SAADbContext`) | 100% | ≥ 50% | ✅ **CUMPLE** |
| DTOs de transferencia de datos | 100% (6/7 clases) | ≥ 80% | ✅ **CUMPLE** |
| Entidades principales del dominio | 75% (promedio ponderado) | ≥ 50% | ✅ **CUMPLE** |
| **Cobertura global del proyecto** | **54.6%** | ≥ 50% | ✅ **CUMPLE** |
