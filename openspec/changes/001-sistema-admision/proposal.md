# Propuesta SSD — Sistema Automatizado de Admisión (SAA)

> **Identificador del Cambio:** `001-sistema-admision`  
> **Fecha de Elaboración:** 2026-07-08  
> **Versión del Documento:** 1.0  
> **Metodología:** Desarrollo Dirigido por Especificaciones (SSD) — OpenSpec  
> **Estado:** COMPLETADO

---

## 1. Contexto y Problemática

Las instituciones de educación superior en el Perú enfrentan desafíos significativos en la gestión de sus procesos de admisión. Históricamente, dichos procesos se han ejecutado de forma manual o semiautomatizada, lo cual genera las siguientes problemáticas:

- **Propensión a errores humanos:** La calificación manual de exámenes de admisión y la clasificación de postulantes introduce inconsistencias en los resultados finales, afectando la equidad del proceso.
- **Falta de transparencia:** Los postulantes carecen de acceso inmediato y verificable a sus resultados, lo que genera desconfianza en la institución evaluadora.
- **Ineficiencia operativa:** La consolidación de datos provenientes de fichas de postulación, exámenes y programas académicos requiere esfuerzo manual considerable, incrementando los tiempos de procesamiento.
- **Ausencia de trazabilidad:** No se dispone de un registro auditable que documente las decisiones de admisión, los puntajes asignados y los criterios aplicados en cada período.
- **Escalabilidad limitada:** Los procesos manuales no soportan adecuadamente el crecimiento en el número de postulantes ni la incorporación de nuevos programas académicos.

En este contexto, se identifica la necesidad de desarrollar un **Sistema Automatizado de Admisión (SAA)** que aborde estas deficiencias mediante la automatización integral del ciclo de admisión, desde el registro del postulante hasta la generación de resultados finales con orden de mérito.

---

## 2. Objetivo

### 2.1 Objetivo General

Diseñar, implementar y documentar un Sistema Automatizado de Admisión (SAA) aplicando la metodología de **Desarrollo Dirigido por Especificaciones (SSD)** y los principios de **Clean Architecture**, garantizando que cada componente del sistema sea trazable a una especificación formal definida previamente a la codificación.

### 2.2 Objetivos Específicos

1. Automatizar el registro, evaluación y clasificación de postulantes mediante un motor de admisión basado en reglas de negocio formalizadas.
2. Implementar una arquitectura de software desacoplada en cuatro capas (Dominio, Aplicación, Infraestructura y Presentación) siguiendo los principios SOLID.
3. Proveer una API RESTful segura mediante autenticación JWT con control de acceso basado en roles (`Administrador`, `Postulante`).
4. Desarrollar una interfaz de usuario SPA con React 19 que permita la interacción diferenciada por rol.
5. Alcanzar una cobertura de pruebas superior al 90%, validando la conformidad del código con las especificaciones SSD.
6. Documentar exhaustivamente el proceso de desarrollo mediante artefactos OpenSpec (propuesta, diseño, tareas, especificación central).

---

## 3. Alcance

### 3.1 Funcionalidades Incluidas

El sistema SAA comprende las siguientes áreas funcionales:

| Área Funcional | Descripción |
|---|---|
| **Registro de Postulantes** | Alta de postulantes con validación de unicidad de DNI, creación simultánea de usuario vinculado y ficha de postulación. |
| **Gestión de Exámenes** | Registro de puntajes de exámenes de admisión asociados a postulantes, con soporte para observaciones y fechas. |
| **Motor de Admisión** | Procesamiento automático de resultados: cálculo de puntajes, aplicación de umbral aprobatorio (50.0), ordenamiento por mérito, asignación de vacantes por programa académico y generación de resultados (`Ingresante`, `Aprobado`, `Desaprobado`). |
| **Clasificación y Mérito** | Asignación de orden de mérito por programa académico con respeto al límite de vacantes configurado. |
| **Reportes** | Generación de reportes de ingresantes y reportes generales con datos completos del postulante y programa. |
| **Autenticación y Autorización** | Sistema JWT con roles diferenciados: administradores acceden a gestión completa; postulantes consultan sus propios resultados. |
| **Trazabilidad** | Registro de fechas de creación, actualización y último acceso en todas las entidades del sistema. |
| **Gestión de Programas Académicos** | Configuración de programas con código, nombre, nivel académico, vacantes, fechas de proceso y estado. |
| **Gestión de Períodos de Admisión** | Definición de períodos con fechas de inicio y fin para organizar ciclos de admisión. |

### 3.2 Entidades del Dominio Cubiertas

El modelo de dominio implementado en `SAA.Domain.Entities` comprende las siguientes entidades: `Postulante`, `ExamenAdmision`, `FichaPostulacion`, `ResultadoAdmision`, `ProgramaAcademico`, `PeriodoAdmision`, `Usuario`, `Rol`, `ConfiguracionSistema`, `DocumentoPostulante`, `LogAuditoria`, `LogMotorAdmision`, `Matricula`, `Notificacion`, `Sesion` y `TipoDocumento`.

---

## 4. No-Alcance

Las siguientes funcionalidades se encuentran **explícitamente excluidas** del alcance del presente proyecto:

| Exclusión | Justificación |
|---|---|
| **Procesamiento de pagos** | La gestión financiera de derechos de admisión requiere integración con pasarelas de pago y cumplimiento normativo financiero que excede el ámbito académico del proyecto. |
| **Proceso de matrícula post-admisión** | Aunque la entidad `Matricula` existe en el modelo de dominio como referencia futura, su flujo completo no se encuentra implementado en esta versión. |
| **Soporte multi-institución** | El sistema está diseñado para operar en una única institución educativa. La arquitectura multi-tenant requiere consideraciones adicionales de aislamiento de datos. |
| **Notificaciones en tiempo real** | Si bien la entidad `Notificacion` existe en el dominio, el subsistema de envío de correos electrónicos o notificaciones push no se encuentra implementado. |
| **Gestión documental avanzada** | La entidad `DocumentoPostulante` contempla la referencia a documentos, pero el almacenamiento y procesamiento de archivos digitales no forma parte del alcance actual. |
| **Auditoría avanzada** | Las entidades `LogAuditoria` y `LogMotorAdmision` proveen la estructura para trazabilidad, pero la implementación completa de un sistema de auditoría queda para versiones futuras. |

---

## 5. Requisitos Funcionales

Los requisitos funcionales fueron definidos conforme a la metodología SSD, **previamente** a la codificación, y se encuentran implementados y verificados en el código fuente.

| ID | Requisito Funcional | Clase/Componente Implementador |
|---|---|---|
| **RF-01** | El sistema debe permitir el registro de nuevos postulantes con datos personales (nombres, apellidos, DNI, correo, teléfono, programa de interés). | `PostulanteService.CrearPostulanteAsync()` |
| **RF-02** | El sistema debe validar la unicidad del DNI al registrar un postulante, rechazando duplicados con mensaje descriptivo. | `PostulanteService.CrearPostulanteAsync()` — validación `AnyAsync(p => p.DNI == dto.DNI)` |
| **RF-03** | El sistema debe crear automáticamente un usuario vinculado al postulante al momento del registro, con rol `Postulante`. | `PostulanteService.CrearPostulanteAsync()` — creación de entidad `Usuario` con rol `"Postulante"` |
| **RF-04** | El sistema debe listar todos los postulantes registrados con sus datos principales. | `PostulanteService.ObtenerTodosAsync()` |
| **RF-05** | El sistema debe permitir el registro de puntajes de exámenes de admisión asociados a un postulante específico. | `MotorAdmisionService.RegistrarExamenAsync()` |
| **RF-06** | El sistema debe validar la existencia del postulante antes de registrar un examen. | `MotorAdmisionService.RegistrarExamenAsync()` — validación `FindAsync(dto.IdPostulante)` |
| **RF-07** | El motor de admisión debe procesar automáticamente todos los resultados, eliminando resultados previos y recalculando desde cero. | `MotorAdmisionService.ProcesarResultadosAsync()` — `RemoveRange(previousResultados)` |
| **RF-08** | El motor debe agrupar exámenes por programa académico a través de las fichas de postulación. | `MotorAdmisionService.ProcesarResultadosAsync()` — agrupación por `IdProgramaAcademico` |
| **RF-09** | El motor debe ordenar postulantes por puntaje descendente dentro de cada programa académico. | `MotorAdmisionService.ProcesarResultadosAsync()` — `OrderByDescending(e => e.Puntaje)` |
| **RF-10** | El motor debe aplicar un umbral aprobatorio de **50.0 puntos**: puntajes inferiores se clasifican como `Desaprobado`. | `MotorAdmisionService.ProcesarResultadosAsync()` — condición `examen.Puntaje >= 50.0m` |
| **RF-11** | El motor debe asignar el estado `Ingresante` a postulantes aprobados mientras existan vacantes disponibles en el programa; los aprobados restantes se clasifican como `Aprobado`. | `MotorAdmisionService.ProcesarResultadosAsync()` — lógica `cuposAsignados < (prog.Vacantes ?? 0)` |
| **RF-12** | El motor debe asignar un orden de mérito secuencial a cada postulante evaluado por programa. | `MotorAdmisionService.ProcesarResultadosAsync()` — `ordenMeritoActual++` |
| **RF-13** | El sistema debe generar reportes de ingresantes filtrados por estado `Ingresante`, ordenados por puesto de mérito. | `MotorAdmisionService.ObtenerReporteIngresantesAsync()` |
| **RF-14** | El sistema debe generar reportes generales de todos los resultados, ordenados por puntaje descendente. | `MotorAdmisionService.ObtenerReporteTodosAsync()` |
| **RF-15** | El sistema debe autenticar usuarios mediante JWT, diferenciando entre administradores (login por nombre de usuario y contraseña) y postulantes (login por nombre y DNI). | `AuthService.LoginAsync()` — flujo dual de autenticación |

---

## 6. Requisitos No Funcionales

Los requisitos no funcionales se definen conforme al estándar **ISO/IEC 25010:2023** para la calidad de productos de software.

| ID | Categoría ISO 25010 | Requisito | Métrica/Criterio |
|---|---|---|---|
| **RNF-01** | Rendimiento | El motor de admisión debe procesar los resultados de todos los postulantes en tiempo razonable. | Procesamiento completo en menos de 5 segundos para hasta 1000 postulantes. |
| **RNF-02** | Seguridad | La autenticación debe implementarse mediante tokens JWT con expiración controlada. | Tokens con vigencia de 2 horas, firmados con HMAC-SHA256 (`SecurityAlgorithms.HmacSha256Signature`). |
| **RNF-03** | Seguridad | Los endpoints administrativos deben estar protegidos por autorización basada en roles. | Atributo `[Authorize(Roles = "Administrador")]` en `AdmisionController`. |
| **RNF-04** | Usabilidad | La interfaz de usuario debe ser intuitiva y responsiva, accesible desde navegadores modernos. | SPA desarrollada con React 19 y CSS responsivo. |
| **RNF-05** | Mantenibilidad | La arquitectura debe facilitar la modificación y extensión independiente de cada capa. | Clean Architecture con 4 capas desacopladas e inversión de dependencias mediante `IApplicationDbContext`. |
| **RNF-06** | Fiabilidad | Las operaciones de escritura críticas deben ejecutarse dentro de transacciones atómicas. | Uso de `BeginTransactionAsync()`, `CommitAsync()` y `RollbackAsync()` en servicios. |
| **RNF-07** | Portabilidad | La capa de datos debe abstraer el proveedor de base de datos mediante un contexto configurable. | `SAADbContext` configurable para SQL Server (producción) e InMemory (pruebas). |
| **RNF-08** | Compatibilidad | La API debe exponerse conforme a los estándares RESTful con documentación OpenAPI. | Controladores decorados con `[ApiController]` y rutas `[Route("api/[controller]")]`. |

---

## 7. Criterios de Éxito

El proyecto se considera exitoso cuando se cumplen **todos** los siguientes criterios:

| Criterio | Descripción | Estado |
|---|---|---|
| **CE-01** | Cobertura de pruebas unitarias e integración superior al 90%, verificada mediante Coverlet. | ✅ Cumplido |
| **CE-02** | Todas las operaciones CRUD de postulantes funcionan correctamente (crear, listar, consultar resultado). | ✅ Cumplido |
| **CE-03** | El motor de admisión clasifica correctamente a los postulantes en `Ingresante`, `Aprobado` y `Desaprobado` según el umbral de 50.0 puntos y las vacantes disponibles. | ✅ Cumplido |
| **CE-04** | La autenticación JWT funciona para ambos roles (`Administrador` y `Postulante`) con flujos diferenciados. | ✅ Cumplido |
| **CE-05** | Los reportes de ingresantes y general se generan con datos consistentes y ordenamiento correcto. | ✅ Cumplido |
| **CE-06** | La arquitectura cumple con los principios de Clean Architecture y SOLID, verificable mediante inspección del código. | ✅ Cumplido |
| **CE-07** | Todos los artefactos OpenSpec (propuesta, diseño, tareas, especificación central) están completos y son consistentes entre sí. | ✅ Cumplido |

---

## 8. Enfoque SSD (Desarrollo Dirigido por Especificaciones)

### 8.1 Definición del Enfoque

El Desarrollo Dirigido por Especificaciones (SSD) constituye una metodología en la cual las **especificaciones formales** del sistema se definen de manera exhaustiva **antes** de iniciar la codificación. A diferencia del desarrollo ad-hoc, el SSD establece un contrato formal entre los requisitos y la implementación, garantizando trazabilidad bidireccional.

### 8.2 Aplicación en el Proyecto SAA

En el contexto del proyecto SAA, el enfoque SSD se aplicó de la siguiente manera:

1. **Especificación previa al código:** Los 15 requisitos funcionales (RF-01 a RF-15) y los 8 requisitos no funcionales (RNF-01 a RNF-08) fueron formalizados antes de escribir cualquier línea de código. Cada requisito incluye una referencia directa al componente implementador.

2. **Modelo de dominio especificado:** Las entidades del dominio (`Postulante`, `ExamenAdmision`, `ResultadoAdmision`, `FichaPostulacion`, `ProgramaAcademico`, `PeriodoAdmision`, `Usuario`, `Rol`) fueron diseñadas a partir de las especificaciones del proceso de admisión, no del código existente.

3. **Reglas de negocio formalizadas:** Las reglas del motor de admisión (umbral 50.0, asignación por vacantes, orden de mérito) fueron documentadas como especificaciones verificables antes de su implementación en `MotorAdmisionService`.

4. **Arquitectura especificada:** La decisión de utilizar Clean Architecture con cuatro capas fue una especificación arquitectónica que precedió a la creación de la estructura de proyectos.

5. **Pruebas derivadas de especificaciones:** Los casos de prueba se derivan directamente de los requisitos funcionales, no del código, garantizando que las pruebas validan el cumplimiento de las especificaciones y no meramente la cobertura de ramas.

### 8.3 Documentación OpenSpec

El framework OpenSpec se utiliza para estructurar y versionar los artefactos SSD del proyecto. La configuración se define en `openspec/config.yaml` y los artefactos se organizan bajo `openspec/changes/001-sistema-admision/`:

- **`proposal.md`** (este documento): Formaliza el problema, objetivos, alcance y requisitos.
- **`design.md`**: Documenta las decisiones arquitectónicas y el diseño detallado.
- **`tasks.md`**: Descompone el trabajo en tareas trazables organizadas por sprints.
- **`specs/source-of-truth.md`**: Consolida la especificación central como referencia única y autoritativa.

---

> **Aprobación:** Este documento ha sido revisado y aprobado conforme a los lineamientos del marco SSD-OpenSpec.  
> **Autor:** Equipo de Desarrollo SAA  
> **Fecha de Aprobación:** 2026-07-08
