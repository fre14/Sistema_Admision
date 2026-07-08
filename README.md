# 🎓 Sistema Automatizado de Admisión (SAA)

> Sistema web para automatizar el proceso de evaluación y selección de candidatos en procesos de admisión educativa.  
> Desarrollado con el enfoque **SSD (Specification-Driven Development)** y metodología **Scrum**.

---

## 📋 Descripción

El **SAA** es una plataforma integral que permite:
- **Registrar postulantes** con validación de DNI único
- **Gestionar exámenes de admisión** con registro de calificaciones
- **Clasificar automáticamente** a los postulantes (Motor de Admisión)
- **Generar reportes** de ingresantes y resultados generales
- **Portal dual**: Dashboard de Administrador y Portal del Postulante

## 🏗️ Arquitectura

El proyecto sigue los principios de **Clean Architecture** (Robert C. Martin):

```
┌──────────────────────────────────────────┐
│           SAA.API (Presentación)          │
│  Controllers · Frontend React · JWT      │
├──────────────────────────────────────────┤
│        SAA.Application (Aplicación)       │
│  Services · DTOs · Interfaces            │
├──────────────────────────────────────────┤
│          SAA.Domain (Dominio)             │
│  Entities (16 entidades de negocio)      │
├──────────────────────────────────────────┤
│     SAA.Infrastructure (Infraestructura)  │
│  DbContext · Migrations · SeedData       │
└──────────────────────────────────────────┘
```

## 🛠️ Stack Tecnológico

### Backend
| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| .NET | 10 | Framework principal |
| C# | 13 | Lenguaje de programación |
| ASP.NET Core | 10 | API REST |
| Entity Framework Core | 10 | ORM / Acceso a datos |
| SQL Server | 2022 | Base de datos relacional |
| JWT | RFC 7519 | Autenticación y autorización |

### Frontend
| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| React | 19.2.1 | Librería UI |
| TypeScript | 5.9 | Tipado estático |
| Vite | 7.2.6 | Build tool y dev server |

### Testing
| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| xUnit | 2.8.1 | Framework de pruebas |
| FluentAssertions | 6.12.1 | Aserciones legibles |
| Moq | 4.20.70 | Mocking |
| Coverlet | 10.0.1 | Cobertura de código |

## 📁 Estructura del Proyecto

```
Sistema_Admision/
├── SAA.API/
│   ├── SAA.API.Server/          # API REST
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── PostulantesController.cs
│   │   │   └── AdmisionController.cs
│   │   └── Program.cs
│   ├── SAA.API.AppHost/         # .NET Aspire Host
│   └── frontend/               # React SPA
│       └── src/
│           ├── App.tsx
│           └── App.css
├── SAA.Application/
│   ├── Services/
│   │   ├── MotorAdmisionService.cs
│   │   ├── PostulanteService.cs
│   │   └── AuthService.cs
│   ├── DTOs/
│   └── Interfaces/
├── SAA.Domain/
│   └── Entities/                # 16 entidades
├── SAA.Infrastructure/
│   ├── Data/SAADbContext.cs
│   ├── Migrations/
│   └── Services/SeedDataService.cs
├── SAA.Tests/                   # Pruebas unitarias e integración
├── SAA_AdmisionDB_Schema.sql    # Schema de base de datos
├── coveragereport_new/          # Reporte de cobertura
└── SAA.Solution.slnx
```

## 🌐 API Endpoints

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/auth/login` | Iniciar sesión | No |
| `POST` | `/api/postulantes` | Registrar postulante | No |
| `GET` | `/api/postulantes` | Listar postulantes | No |
| `GET` | `/api/postulantes/mi-resultado` | Consultar resultado propio | JWT (Postulante) |
| `POST` | `/api/admision/examen` | Registrar examen | JWT (Admin) |
| `POST` | `/api/admision/procesar` | Procesar motor de admisión | JWT (Admin) |
| `GET` | `/api/admision/reporte-ingresantes` | Reporte de ingresantes | JWT (Admin) |
| `GET` | `/api/admision/reporte-todos` | Reporte general | JWT (Admin) |

## 🗄️ Base de Datos

3 schemas con 8 tablas principales:

- **Schema Admision**: `Postulante`, `ExamenAdmision`, `FichaPostulacion`, `ResultadoAdmision`
- **Schema Config**: `ProgramaAcademico`, `PeriodoAdmision`
- **Schema Seguridad**: `Usuario`, `Rol`

## ⚙️ Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server)
- [Node.js 20+](https://nodejs.org/)

## 🚀 Instalación

```bash
# Clonar repositorio
git clone https://github.com/fre14/Sistema_Admision.git
cd Sistema_Admision

# Backend
dotnet restore
dotnet build

# Frontend
cd SAA.API/frontend
npm install
npm run dev
```

## 🧪 Ejecutar Pruebas

```bash
# Ejecutar todas las pruebas
dotnet test SAA.Tests/SAA.Tests.csproj

# Con cobertura de código
dotnet test SAA.Tests/SAA.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

# Generar reporte HTML de cobertura
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:SAA.Tests/coverage.cobertura.xml -targetdir:coveragereport_new -reporttypes:TextSummary
```

## 📊 Cobertura de Código

| Ensamblado | Cobertura de Líneas |
|-----------|-------------------|
| **SAA.Application** | 79.6% |
| **SAA.Domain** | 33.6% |
| **SAA.Infrastructure** | 19.6% |
| **Total** | **54.6%** |

- Líneas cubiertas: 268/490
- Ramas cubiertas: 34/58 (58.6%)
- Métodos cubiertos: 84/162 (51.8%)

## 👤 Autor

- **Fredy Bonilla Rey**
- Curso: IS-489 — Pruebas y Aseguramiento de la Calidad
- Docente: Ing. Richard Zapata Casaverde
- Universidad Nacional de San Cristóbal de Huamanga
- Ayacucho, Perú — 2026

## 📄 Licencia

Proyecto académico — Universidad Nacional de San Cristóbal de Huamanga (UNSCH)
