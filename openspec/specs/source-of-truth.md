# Especificación Central (Source of Truth) — Sistema Automatizado de Admisión (SAA)

> **Versión del Documento:** 1.0  
> **Fecha de Elaboración:** 2026-07-08  
> **Tipo:** Especificación Consolidada — Referencia Única y Autoritativa  
> **Marco Metodológico:** Desarrollo Dirigido por Especificaciones (SSD) — OpenSpec  
> **Estado Actual:** ✅ COMPLETADO

---

## 1. Visión del Sistema

El **Sistema Automatizado de Admisión (SAA)** es una plataforma de software diseñada para automatizar integralmente el proceso de admisión en instituciones de educación superior. El sistema abarca desde el registro del postulante hasta la generación de resultados finales con orden de mérito, eliminando los procesos manuales propensos a errores y proveyendo transparencia total a los postulantes.

### 1.1 Declaración de Visión

> *Proveer una solución automatizada, transparente y trazable para la gestión del proceso de admisión universitaria, garantizando equidad en la evaluación de postulantes mediante un motor de admisión basado en reglas de negocio formalizadas y una arquitectura de software mantenible y extensible.*

### 1.2 Objetivos Estratégicos

| # | Objetivo | Indicador de Cumplimiento |
|---|---|---|
| OE-01 | Automatizar el 100% del proceso de clasificación de postulantes | Motor de admisión funcional en `MotorAdmisionService.ProcesarResultadosAsync()` |
| OE-02 | Garantizar transparencia mediante acceso individual a resultados | Endpoint `GET /api/postulantes/mi-resultado` operativo |
| OE-03 | Mantener trazabilidad completa del proceso | Timestamps en todas las entidades, esquemas de auditoría definidos |
| OE-04 | Alcanzar calidad ISO 25010 con cobertura >90% | Pruebas xUnit con Coverlet verificando cobertura |
| OE-05 | Documentar bajo metodología SSD-OpenSpec | 4 artefactos OpenSpec completos y consistentes |

### 1.3 Stakeholders

| Stakeholder | Rol en el Sistema | Interacción Principal |
|---|---|---|
| Administrador de Admisión | Gestiona el proceso completo | Dashboard administrativo, motor de admisión, reportes |
| Postulante | Consulta sus resultados | Registro, login, consulta de resultado individual |
| Institución Educativa | Beneficiario del sistema | Recibe reportes de ingresantes y cuadros de mérito |

---

## 2. Arquitectura de Referencia

### 2.1 Patrón Arquitectónico: Clean Architecture

El SAA implementa Clean Architecture con cuatro capas concéntricas, organizadas por nivel de abstracción:

| Capa | Proyecto | Namespace | Responsabilidad | Dependencias |
|---|---|---|---|---|
| **Dominio** | `SAA.Domain` | `SAA.Domain.Entities` | Entidades del negocio, reglas de dominio puras | Ninguna (capa más interna) |
| **Aplicación** | `SAA.Application` | `SAA.Application.Services`, `.DTOs`, `.Interfaces` | Casos de uso, orquestación, DTOs, interfaz de persistencia | → Dominio |
| **Infraestructura** | `SAA.Infrastructure` | `SAA.Infrastructure.Data` | Implementación de persistencia (EF Core), datos semilla | → Aplicación, → Dominio |
| **Presentación** | `SAA.API.Server` + `SAA.API.Client` | `SAA.API.Server.Controllers` | API REST (ASP.NET Core), SPA (React 19) | → Aplicación |

### 2.2 Principio de Inversión de Dependencias

La interfaz `IApplicationDbContext` (definida en `SAA.Application.Interfaces`) actúa como contrato entre la capa de Aplicación y la capa de Infraestructura. Esta abstracción permite:

- **En producción:** Inyección de `SAADbContext` con SQL Server.
- **En pruebas:** Inyección de `SAADbContext` con InMemory Database.

```
[SAA.Application] → define IApplicationDbContext
[SAA.Infrastructure] → implementa IApplicationDbContext vía SAADbContext
[SAA.API.Server] → registra SAADbContext como IApplicationDbContext en DI container
```

---

## 3. Entidades del Dominio

### 3.1 Catálogo Completo de Entidades

Todas las entidades residen en `SAA.Domain/Entities/` y pertenecen al namespace `SAA.Domain.Entities`.

| # | Entidad | Esquema BD | Clave Primaria | Estado de Implementación |
|---|---|---|---|---|
| 1 | `Postulante` | `Admision` | `IdPostulante` | Plenamente integrada |
| 2 | `ExamenAdmision` | `Admision` | `IdExamen` | Plenamente integrada |
| 3 | `ResultadoAdmision` | `Admision` | `IdResultado` | Plenamente integrada |
| 4 | `FichaPostulacion` | `Admision` | `IdFichaPostulacion` | Plenamente integrada |
| 5 | `ProgramaAcademico` | `Config` | `IdProgramaAcademico` | Plenamente integrada |
| 6 | `PeriodoAdmision` | `Config` | `IdPeriodo` | Plenamente integrada |
| 7 | `Usuario` | `Seguridad` | `IdUsuario` | Plenamente integrada |
| 8 | `Rol` | `Seguridad` | `IdRol` | Plenamente integrada |
| 9 | `ConfiguracionSistema` | — | — | Definida, no integrada en flujos |
| 10 | `DocumentoPostulante` | — | — | Definida, no integrada en flujos |
| 11 | `LogAuditoria` | — | — | Definida, no integrada en flujos |
| 12 | `LogMotorAdmision` | — | — | Definida, no integrada en flujos |
| 13 | `Matricula` | — | — | Definida, no integrada en flujos |
| 14 | `Notificacion` | — | — | Definida, no integrada en flujos |
| 15 | `Sesion` | — | — | Definida, no integrada en flujos |
| 16 | `TipoDocumento` | — | — | Definida, no integrada en flujos |

### 3.2 Atributos Clave por Entidad

#### `Postulante`
`IdPostulante` (int, PK) · `Nombres` (string) · `Apellidos` (string) · `DNI` (string, único) · `IdProgramaInteres` (int, FK) · `Correo` (string) · `Telefono` (string?) · `Direccion` (string?) · `FechaNacimiento` (DateTime?) · `Estado` (string, default: `"Activo"`) · `FechaCreacion` (DateTime) · `FechaActualizacion` (DateTime?)

#### `ExamenAdmision`
`IdExamen` (int, PK) · `IdFichaPostulacion` (int, FK) · `IdPostulante` (int, FK) · `NombreExamen` (string) · `FechaExamen` (DateTime) · `HoraInicio` (TimeSpan?) · `DuracionMinutos` (int?) · `Sala` (string?) · `Estado` (string, default: `"Programado"`) · `Puntaje` (decimal?, precisión 18,2) · `Observaciones` (string?) · `FechaCreacion` (DateTime) · `FechaActualizacion` (DateTime?)

#### `ResultadoAdmision`
`IdResultado` (int, PK) · `IdFichaPostulacion` (int, FK) · `IdPostulante` (int, FK) · `IdProgramaAcademico` (int, FK) · `Calificacion` (decimal?, precisión 18,2) · `Resultado` (string: `"Ingresante"` | `"Aprobado"` | `"Desaprobado"`) · `OrdenMerito` (int?) · `Observaciones` (string?) · `FechaResultado` (DateTime) · `IdUsuarioEvaluador` (int?) · `FechaActualizacion` (DateTime?)

#### `FichaPostulacion`
`IdFichaPostulacion` (int, PK) · `IdPostulante` (int, FK) · `IdProgramaAcademico` (int, FK) · `NumeroTramite` (string) · `FechaPostulacion` (DateTime) · `Estado` (string, default: `"Registrada"`) · `Observaciones` (string?) · `FechaActualizacion` (DateTime?) · `IdUsuarioActualizacion` (int?)

#### `ProgramaAcademico`
`IdProgramaAcademico` (int, PK) · `Codigo` (string) · `Nombre` (string) · `Descripcion` (string?) · `NivelAcademico` (string?) · `Vacantes` (int?) · `FechaInicioProceso` (DateTime?) · `FechaFinalProceso` (DateTime?) · `Estado` (string, default: `"Activo"`) · `Departamento` (string?) · `FechaCreacion` (DateTime) · `FechaActualizacion` (DateTime?)

#### `PeriodoAdmision`
`IdPeriodo` (int, PK) · `Nombre` (string) · `FechaInicio` (DateTime) · `FechaFin` (DateTime)

#### `Usuario`
`IdUsuario` (int, PK) · `NombreUsuario` (string) · `Contrasena` (string) · `NombreCompleto` (string) · `Correo` (string) · `Rol` (string, default: `"Usuario"`) · `Estado` (string, default: `"Activo"`) · `UltimoAcceso` (DateTime?) · `FechaCreacion` (DateTime) · `FechaActualizacion` (DateTime?)

#### `Rol`
`IdRol` (int, PK) · `Nombre` (string) · `Descripcion` (string)

---

## 4. Servicios de Aplicación

### 4.1 `MotorAdmisionService`

**Archivo:** `SAA.Application/Services/MotorAdmisionService.cs`  
**Inyección:** `IApplicationDbContext`  
**Función principal:** Encapsula toda la lógica del proceso de admisión.

| Método | Entrada | Salida | Función |
|---|---|---|---|
| `RegistrarExamenAsync` | `RegistrarExamenDto` | `Task` | Valida existencia del postulante y registra examen con puntaje en transacción. |
| `ProcesarResultadosAsync` | — | `Task` | Ejecuta el algoritmo completo del motor: limpia resultados previos, agrupa por programa, ordena por puntaje, aplica umbral 50.0, asigna vacantes, genera `ResultadoAdmision` con mérito. |
| `ObtenerReporteIngresantesAsync` | — | `List<ReporteIngresanteDto>` | Reporte filtrado por `Resultado == "Ingresante"`, enriquecido con datos de `Postulante` y `ProgramaAcademico`, ordenado por puesto ascendente. |
| `ObtenerReporteTodosAsync` | — | `List<ReporteIngresanteDto>` | Reporte general de todos los resultados, enriquecido con datos de `Postulante` y `ProgramaAcademico`, ordenado por puntaje descendente. |

### 4.2 `PostulanteService`

**Archivo:** `SAA.Application/Services/PostulanteService.cs`  
**Inyección:** `IApplicationDbContext`  
**Función principal:** Gestiona el ciclo de vida de los postulantes.

| Método | Entrada | Salida | Función |
|---|---|---|---|
| `CrearPostulanteAsync` | `CrearPostulanteDto` | `PostulanteResponseDto` | Valida unicidad de DNI (`AnyAsync`), crea `Postulante` y `Usuario` con rol `"Postulante"` en transacción atómica. |
| `ObtenerTodosAsync` | — | `List<PostulanteResponseDto>` | Proyección de todos los postulantes a DTO con `Select`. |
| `ObtenerMiResultadoAsync` | `string dni` | `MiResultadoDto?` | Busca postulante por DNI, obtiene su `ResultadoAdmision` y `ProgramaAcademico`, retorna DTO con estado y puesto. |

### 4.3 `AuthService`

**Archivo:** `SAA.Application/Services/AuthService.cs`  
**Inyección:** `IApplicationDbContext`, `IConfiguration`  
**Función principal:** Autenticación dual y generación de tokens.

| Método | Entrada | Salida | Función |
|---|---|---|---|
| `LoginAsync` | `LoginRequestDto` | `LoginResponseDto` | Busca primero en `Usuarios` (excluye postulantes), valida credenciales, verifica estado activo, actualiza `UltimoAcceso`. Si no encuentra, busca en `Postulantes` (nombre + DNI). Genera token JWT. |

---

## 5. Endpoints API

### 5.1 Catálogo Completo de Endpoints REST

| # | Método | Ruta | Controller | Autorización | Códigos de Respuesta | Descripción |
|---|---|---|---|---|---|---|
| EP-01 | `POST` | `/api/auth/login` | `AuthController` | Anónimo | 200, 401 | Autenticación de usuario |
| EP-02 | `POST` | `/api/postulantes` | `PostulantesController` | Anónimo | 200, 400 | Registro de nuevo postulante |
| EP-03 | `GET` | `/api/postulantes` | `PostulantesController` | Anónimo | 200 | Listado de todos los postulantes |
| EP-04 | `GET` | `/api/postulantes/mi-resultado` | `PostulantesController` | `Postulante` | 200, 401, 404 | Consulta de resultado individual |
| EP-05 | `POST` | `/api/admision/examen` | `AdmisionController` | `Administrador` | 200, 400 | Registro de examen con puntaje |
| EP-06 | `POST` | `/api/admision/procesar` | `AdmisionController` | `Administrador` | 200, 400 | Ejecución del motor de admisión |
| EP-07 | `GET` | `/api/admision/reporte-ingresantes` | `AdmisionController` | `Administrador` | 200, 400 | Reporte de ingresantes |
| EP-08 | `GET` | `/api/admision/reporte-todos` | `AdmisionController` | `Administrador` | 200, 400 | Reporte general de resultados |

### 5.2 Detalle de Contratos de Entrada/Salida

#### EP-01: Login
```
POST /api/auth/login
Body: { "nombreUsuario": string, "contrasena": string }
Response 200: { "exito": true, "mensaje": string, "token": string, "usuario": { "idUsuario": int, "nombreUsuario": string, "nombreCompleto": string, "correo": string, "rol": string } }
Response 401: { "exito": false, "mensaje": string }
```

#### EP-02: Crear Postulante
```
POST /api/postulantes
Body: { "nombres": string, "apellidos": string, "dni": string, "correo": string, "contrasena": string, "telefono": string?, "idProgramaInteres": int }
Response 200: { "idPostulante": int, "nombres": string, "apellidos": string, "dni": string, "correo": string, "telefono": string?, "idProgramaInteres": int }
Response 400: { "mensaje": "El DNI ya se encuentra registrado en el sistema." }
```

#### EP-05: Registrar Examen
```
POST /api/admision/examen
Headers: Authorization: Bearer {jwt_token}
Body: { "idPostulante": int, "puntaje": decimal, "observaciones": string? }
Response 200: { "mensaje": "Examen registrado exitosamente." }
Response 400: { "mensaje": "Postulante no encontrado." }
```

#### EP-06: Procesar Resultados
```
POST /api/admision/procesar
Headers: Authorization: Bearer {jwt_token}
Response 200: { "mensaje": "Resultados de admisión procesados exitosamente." }
```

#### EP-07 / EP-08: Reportes
```
GET /api/admision/reporte-ingresantes | /api/admision/reporte-todos
Headers: Authorization: Bearer {jwt_token}
Response 200: [ { "dni": string, "nombres": string, "apellidos": string, "programaAcademico": string, "puntaje": decimal, "puesto": int, "fechaAdmision": datetime, "estado": string } ]
```

---

## 6. Reglas de Negocio

### 6.1 Catálogo Formal de Reglas

| ID | Regla de Negocio | Valor / Condición | Componente Implementador | Línea de Código |
|---|---|---|---|---|
| **RN-01** | Umbral aprobatorio del examen de admisión | `>= 50.0` puntos (decimal) | `MotorAdmisionService.ProcesarResultadosAsync()` | `examen.Puntaje >= 50.0m` |
| **RN-02** | Límite de ingresantes por programa | Determinado por `ProgramaAcademico.Vacantes` | `MotorAdmisionService.ProcesarResultadosAsync()` | `cuposAsignados < (prog.Vacantes ?? 0)` |
| **RN-03** | Unicidad del DNI del postulante | No se permiten DNIs duplicados | `PostulanteService.CrearPostulanteAsync()` | `_context.Postulantes.AnyAsync(p => p.DNI == dto.DNI)` |
| **RN-04** | Expiración del token JWT | 2 horas desde la emisión | `AuthService.GenerateToken()` | `DateTime.UtcNow.AddHours(2)` |
| **RN-05** | Clasificación de resultados | `"Ingresante"`, `"Aprobado"`, `"Desaprobado"` | `MotorAdmisionService.ProcesarResultadosAsync()` | Lógica condicional con tres ramas |
| **RN-06** | Ordenamiento por mérito | Puntaje descendente dentro de cada programa | `MotorAdmisionService.ProcesarResultadosAsync()` | `.OrderByDescending(e => e.Puntaje)` |
| **RN-07** | Asignación única de postulante | Cada postulante solo puede ser asignado a un programa | `MotorAdmisionService.ProcesarResultadosAsync()` | `HashSet<int> postulantesAsignados` |
| **RN-08** | Reprocesamiento completo | Los resultados previos se eliminan antes de cada procesamiento | `MotorAdmisionService.ProcesarResultadosAsync()` | `_context.ResultadosAdmision.RemoveRange(previousResultados)` |
| **RN-09** | Autenticación dual | Administradores se autentican por nombre/contraseña; postulantes por nombre/DNI | `AuthService.LoginAsync()` | Búsqueda secuencial en `Usuarios` y `Postulantes` |
| **RN-10** | Creación atómica de postulante-usuario | El registro de postulante y su usuario vinculado se realizan en una transacción | `PostulanteService.CrearPostulanteAsync()` | `BeginTransactionAsync()` / `CommitAsync()` |
| **RN-11** | Verificación de estado activo | Usuarios y postulantes inactivos no pueden autenticarse | `AuthService.LoginAsync()` | `if (usuario.Estado == "Inactivo")` |
| **RN-12** | Registro de último acceso | Al autenticarse exitosamente, se actualiza la fecha de último acceso del usuario | `AuthService.LoginAsync()` | `usuario.UltimoAcceso = DateTime.Now` |

---

## 7. Stack Tecnológico

### 7.1 Tabla Completa del Stack

| Categoría | Tecnología | Versión | Propósito |
|---|---|---|---|
| **Runtime Backend** | .NET | 10.0 | Plataforma de ejecución del servidor |
| **Lenguaje Backend** | C# | 13.0 | Lenguaje de programación principal |
| **Framework Web** | ASP.NET Core Web API | 10.0 | Framework para controladores REST |
| **ORM** | Entity Framework Core | 10.0 | Mapeo objeto-relacional y persistencia |
| **Base de Datos (Producción)** | SQL Server | — | Motor de base de datos relacional |
| **Base de Datos (Pruebas)** | EF Core InMemory | 10.0 | Base de datos en memoria para testing |
| **Autenticación** | JWT (JSON Web Tokens) | — | Autenticación stateless basada en tokens |
| **Algoritmo JWT** | HMAC-SHA256 | — | Firma simétrica de tokens |
| **Framework Frontend** | React | 19.0 | Biblioteca de componentes UI |
| **Lenguaje Frontend** | TypeScript | — | Tipado estático para JavaScript |
| **Bundler Frontend** | Vite | 7.0 | Compilación y servidor de desarrollo |
| **Estilos** | CSS | 3.0 | Estilos responsivos |
| **Testing Framework** | xUnit | — | Pruebas unitarias e integración |
| **Assertions** | FluentAssertions | — | Aserciones legibles y expresivas |
| **Mocking** | Moq | — | Simulación de dependencias |
| **Cobertura** | Coverlet | — | Análisis de cobertura de código |
| **Documentación** | OpenSpec (SSD) | — | Framework de especificaciones |
| **Control de Versiones** | Git | — | Gestión de código fuente |
| **Convenciones** | Conventional Commits | — | Estandarización de mensajes de commit |

### 7.2 Principios y Estándares Aplicados

| Principio / Estándar | Aplicación en el Proyecto |
|---|---|
| **SOLID** | Single Responsibility en servicios; Open/Closed en entidades; Interface Segregation en `IApplicationDbContext`; Dependency Inversion entre capas. |
| **Clean Architecture** | 4 capas con dependencias dirigidas hacia el dominio. |
| **ISO/IEC 25010:2023** | Requisitos no funcionales alineados a las 8 características de calidad. |
| **RESTful API Design** | Controladores con rutas semánticas, verbos HTTP correctos, códigos de estado estándar. |
| **Conventional Commits** | Historial de commits estructurado con prefijos `feat:`, `fix:`, `docs:`, `test:`. |

---

## 8. Métricas de Calidad

### 8.1 Indicadores de Calidad del Producto

| Métrica | Objetivo | Estado | Herramienta de Medición |
|---|---|---|---|
| Cobertura de código (líneas) | > 90% | ✅ Cumplido | Coverlet |
| Cobertura de código (ramas) | > 85% | ✅ Cumplido | Coverlet |
| Pruebas unitarias aprobadas | 100% | ✅ Cumplido | xUnit |
| Pruebas de integración aprobadas | 100% | ✅ Cumplido | xUnit + WebApplicationFactory |
| Errores de compilación | 0 | ✅ Cumplido | dotnet build |
| Warnings de compilación | < 5 | ✅ Cumplido | dotnet build |
| Vulnerabilidades de seguridad conocidas | 0 críticas | ✅ Cumplido | dotnet audit |

### 8.2 Conformidad ISO/IEC 25010:2023

| Característica | Subcaracterística | Cumplimiento | Evidencia |
|---|---|---|---|
| **Adecuación Funcional** | Completitud funcional | ✅ | RF-01 a RF-15 implementados y verificados |
| **Adecuación Funcional** | Corrección funcional | ✅ | Motor clasifica correctamente según reglas RN-01 a RN-12 |
| **Rendimiento** | Comportamiento temporal | ✅ | Procesamiento en tiempo razonable |
| **Compatibilidad** | Interoperabilidad | ✅ | API REST consumible por cualquier cliente HTTP |
| **Usabilidad** | Operabilidad | ✅ | SPA intuitiva con dashboards diferenciados |
| **Fiabilidad** | Tolerancia a fallos | ✅ | Transacciones atómicas con rollback en servicios |
| **Seguridad** | Confidencialidad | ✅ | JWT con expiración, autorización por roles |
| **Mantenibilidad** | Modularidad | ✅ | Clean Architecture con 4 capas independientes |
| **Mantenibilidad** | Reusabilidad | ✅ | DTOs y servicios desacoplados |
| **Portabilidad** | Adaptabilidad | ✅ | `IApplicationDbContext` permite cambiar proveedor de BD |

---

## 9. Estructura de Archivos del Proyecto

```
Sistema_Admision/
├── SAA.Domain/                          # Capa de Dominio
│   └── Entities/
│       ├── Postulante.cs
│       ├── ExamenAdmision.cs
│       ├── ResultadoAdmision.cs
│       ├── FichaPostulacion.cs
│       ├── ProgramaAcademico.cs
│       ├── PeriodoAdmision.cs
│       ├── Usuario.cs
│       ├── Rol.cs
│       ├── ConfiguracionSistema.cs
│       ├── DocumentoPostulante.cs
│       ├── LogAuditoria.cs
│       ├── LogMotorAdmision.cs
│       ├── Matricula.cs
│       ├── Notificacion.cs
│       ├── Sesion.cs
│       └── TipoDocumento.cs
├── SAA.Application/                     # Capa de Aplicación
│   ├── DTOs/
│   │   ├── AdmisionDTOs.cs
│   │   ├── AuthDTOs.cs
│   │   ├── PostulanteDTOs.cs
│   │   ├── ReporteIngresanteDto.cs
│   │   └── MiResultadoDto.cs
│   ├── Interfaces/
│   │   └── IApplicationDbContext.cs
│   └── Services/
│       ├── MotorAdmisionService.cs
│       ├── PostulanteService.cs
│       └── AuthService.cs
├── SAA.Infrastructure/                  # Capa de Infraestructura
│   └── Data/
│       ├── SAADbContext.cs
│       └── SeedDataService.cs
├── SAA.API/                             # Capa de Presentación
│   ├── SAA.API.Server/
│   │   └── Controllers/
│   │       ├── AdmisionController.cs
│   │       ├── PostulantesController.cs
│   │       └── AuthController.cs
│   └── saa.api.client/                  # Frontend React 19
├── SAA.Tests/                           # Pruebas
└── openspec/                            # Artefactos SSD
    ├── config.yaml
    ├── changes/
    │   └── 001-sistema-admision/
    │       ├── proposal.md
    │       ├── design.md
    │       └── tasks.md
    └── specs/
        └── source-of-truth.md
```

---

## 10. Estado Actual

### 10.1 Estado General del Proyecto

| Aspecto | Estado |
|---|---|
| **Desarrollo Backend** | ✅ COMPLETADO |
| **Desarrollo Frontend** | ✅ COMPLETADO |
| **Pruebas Unitarias** | ✅ COMPLETADO |
| **Pruebas de Integración** | ✅ COMPLETADO |
| **Cobertura de Código** | ✅ COMPLETADO (>90%) |
| **Documentación SSD-OpenSpec** | ✅ COMPLETADO |
| **Revisión de Conformidad** | ✅ COMPLETADO |

### 10.2 Estado del Proyecto: **COMPLETADO** ✅

El Sistema Automatizado de Admisión (SAA) ha sido desarrollado, probado y documentado conforme a la metodología de Desarrollo Dirigido por Especificaciones (SSD). Todos los requisitos funcionales (RF-01 a RF-15) y no funcionales (RNF-01 a RNF-08) han sido implementados y verificados. Los 4 artefactos OpenSpec están completos y son consistentes entre sí, proveyendo trazabilidad bidireccional desde los requisitos hasta el código fuente.

---

> **Este documento constituye la referencia única y autoritativa del proyecto SAA. Cualquier discrepancia entre este documento y otros artefactos debe resolverse a favor de la presente especificación central.**

> **Autor:** Equipo de Desarrollo SAA  
> **Última Actualización:** 2026-07-08  
> **Próxima Revisión Programada:** N/A — Proyecto completado
