IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    IF SCHEMA_ID(N'Admision') IS NULL EXEC(N'CREATE SCHEMA [Admision];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    IF SCHEMA_ID(N'Config') IS NULL EXEC(N'CREATE SCHEMA [Config];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    IF SCHEMA_ID(N'Seguridad') IS NULL EXEC(N'CREATE SCHEMA [Seguridad];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Admision].[ExamenAdmision] (
        [IdExamen] int NOT NULL IDENTITY,
        [IdFichaPostulacion] int NOT NULL,
        [IdPostulante] int NOT NULL,
        [NombreExamen] nvarchar(max) NOT NULL,
        [FechaExamen] datetime2 NOT NULL,
        [HoraInicio] time NULL,
        [DuracionMinutos] int NULL,
        [Sala] nvarchar(max) NULL,
        [Estado] nvarchar(max) NOT NULL,
        [Puntaje] decimal(18,2) NULL,
        [Observaciones] nvarchar(max) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaActualizacion] datetime2 NULL,
        CONSTRAINT [PK_ExamenAdmision] PRIMARY KEY ([IdExamen])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Admision].[FichaPostulacion] (
        [IdFichaPostulacion] int NOT NULL IDENTITY,
        [IdPostulante] int NOT NULL,
        [IdProgramaAcademico] int NOT NULL,
        [NumeroTramite] nvarchar(max) NOT NULL,
        [FechaPostulacion] datetime2 NOT NULL,
        [Estado] nvarchar(max) NOT NULL,
        [Observaciones] nvarchar(max) NULL,
        [FechaActualizacion] datetime2 NULL,
        [IdUsuarioActualizacion] int NULL,
        CONSTRAINT [PK_FichaPostulacion] PRIMARY KEY ([IdFichaPostulacion])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Config].[PeriodoAdmision] (
        [IdPeriodo] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [FechaInicio] datetime2 NOT NULL,
        [FechaFin] datetime2 NOT NULL,
        CONSTRAINT [PK_PeriodoAdmision] PRIMARY KEY ([IdPeriodo])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Admision].[Postulante] (
        [IdPostulante] int NOT NULL IDENTITY,
        [Nombres] nvarchar(max) NOT NULL,
        [Apellidos] nvarchar(max) NOT NULL,
        [DNI] nvarchar(450) NOT NULL,
        [IdProgramaInteres] int NOT NULL,
        [Correo] nvarchar(max) NOT NULL,
        [Telefono] nvarchar(max) NULL,
        [Direccion] nvarchar(max) NULL,
        [FechaNacimiento] datetime2 NULL,
        [Estado] nvarchar(max) NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaActualizacion] datetime2 NULL,
        CONSTRAINT [PK_Postulante] PRIMARY KEY ([IdPostulante])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Config].[ProgramaAcademico] (
        [IdProgramaAcademico] int NOT NULL IDENTITY,
        [Codigo] nvarchar(max) NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [Descripcion] nvarchar(max) NULL,
        [NivelAcademico] nvarchar(max) NULL,
        [Vacantes] int NULL,
        [FechaInicioProceso] datetime2 NULL,
        [FechaFinalProceso] datetime2 NULL,
        [Estado] nvarchar(max) NOT NULL,
        [Departamento] nvarchar(max) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaActualizacion] datetime2 NULL,
        CONSTRAINT [PK_ProgramaAcademico] PRIMARY KEY ([IdProgramaAcademico])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Admision].[ResultadoAdmision] (
        [IdResultado] int NOT NULL IDENTITY,
        [IdFichaPostulacion] int NOT NULL,
        [IdPostulante] int NOT NULL,
        [IdProgramaAcademico] int NOT NULL,
        [Calificacion] decimal(18,2) NULL,
        [Resultado] nvarchar(max) NOT NULL,
        [Observaciones] nvarchar(max) NULL,
        [FechaResultado] datetime2 NOT NULL,
        [IdUsuarioEvaluador] int NULL,
        [FechaActualizacion] datetime2 NULL,
        CONSTRAINT [PK_ResultadoAdmision] PRIMARY KEY ([IdResultado])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Seguridad].[Rol] (
        [IdRol] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Rol] PRIMARY KEY ([IdRol])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE TABLE [Seguridad].[Usuario] (
        [IdUsuario] int NOT NULL IDENTITY,
        [NombreUsuario] nvarchar(max) NOT NULL,
        [Contrasena] nvarchar(max) NOT NULL,
        [NombreCompleto] nvarchar(max) NOT NULL,
        [Correo] nvarchar(max) NOT NULL,
        [Rol] nvarchar(max) NOT NULL,
        [Estado] nvarchar(max) NOT NULL,
        [UltimoAcceso] datetime2 NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaActualizacion] datetime2 NULL,
        CONSTRAINT [PK_Usuario] PRIMARY KEY ([IdUsuario])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Postulante_DNI] ON [Admision].[Postulante] ([DNI]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526102609_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260526102609_InitialCreate', N'10.0.8');
END;

COMMIT;
GO

