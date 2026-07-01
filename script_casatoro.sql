-- =============================================
-- SCRIPT SQL - Módulo de Gestión de Solicitudes
-- Casa Toro
-- =============================================

-- Crear la base de datos
CREATE DATABASE CasaToro_Solicitudes;
GO

-- Usar la base de datos recién creada
USE CasaToro_Solicitudes;
GO

-- =============================================
-- TABLA: Usuarios
-- Almacena los usuarios del sistema
-- Solo usuarios con Activo = 1 pueden crear solicitudes (RN-02)
-- =============================================
CREATE TABLE Usuarios (
    Id      INT             IDENTITY(1,1)   PRIMARY KEY,
    Nombre  NVARCHAR(100)   NOT NULL,
    Email   NVARCHAR(100)   NOT NULL        UNIQUE,
    Activo  BIT             NOT NULL        DEFAULT 1
);
GO

-- =============================================
-- TABLA: TiposSolicitud
-- Catálogo de tipos de solicitud disponibles
-- =============================================
CREATE TABLE TiposSolicitud (
    Id      INT             IDENTITY(1,1)   PRIMARY KEY,
    Nombre  NVARCHAR(100)   NOT NULL
);
GO

-- =============================================
-- TABLA: Solicitudes
-- Almacena cada solicitud registrada
-- RN-03: Toda solicitud inicia en estado Pendiente (EstadoId = 1)
-- =============================================
CREATE TABLE Solicitudes (
    Id              INT             IDENTITY(1,1)   PRIMARY KEY,
    Numero          NVARCHAR(20)    NOT NULL        UNIQUE,
    Fecha           DATETIME        NOT NULL        DEFAULT GETDATE(),
    UsuarioId       INT             NOT NULL,
    TipoId          INT             NOT NULL,
    EstadoId        INT             NOT NULL        DEFAULT 1,
    Observaciones   NVARCHAR(500)   NULL,

    -- Relación con Usuarios
    CONSTRAINT FK_Solicitudes_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),

    -- Relación con TiposSolicitud
    CONSTRAINT FK_Solicitudes_Tipos
        FOREIGN KEY (TipoId) REFERENCES TiposSolicitud(Id)
);
GO

-- =============================================
-- TABLA: Seguimientos
-- Registra cada cambio de estado de una solicitud (RN-04)
-- =============================================
CREATE TABLE Seguimientos (
    Id              INT             IDENTITY(1,1)   PRIMARY KEY,
    SolicitudId     INT             NOT NULL,
    Fecha           DATETIME        NOT NULL        DEFAULT GETDATE(),
    UsuarioId       INT             NOT NULL,
    Comentario      NVARCHAR(500)   NOT NULL,
    EstadoAnterior  INT             NULL,
    EstadoNuevo     INT             NOT NULL,

    -- Relación con Solicitudes
    CONSTRAINT FK_Seguimientos_Solicitudes
        FOREIGN KEY (SolicitudId) REFERENCES Solicitudes(Id),

    -- Relación con Usuarios
    CONSTRAINT FK_Seguimientos_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);
GO

-- =============================================
-- ÍNDICES para optimizar consultas frecuentes
-- =============================================

-- Optimiza la búsqueda de solicitudes por usuario y fecha (RN-01)
CREATE INDEX IX_Solicitudes_UsuarioId_TipoId_Fecha
    ON Solicitudes(UsuarioId, TipoId, Fecha);

-- Optimiza la búsqueda de seguimientos por solicitud
CREATE INDEX IX_Seguimientos_SolicitudId
    ON Seguimientos(SolicitudId);
GO

-- =============================================
-- DATOS DE PRUEBA
-- =============================================
INSERT INTO TiposSolicitud (Nombre) VALUES
    ('Vacaciones'),
    ('Permiso'),
    ('Soporte Técnico'),
    ('Certificado Laboral');

INSERT INTO Usuarios (Nombre, Email, Activo) VALUES
    ('Juan Pérez',    'juan@casatoro.com',   1),
    ('María López',   'maria@casatoro.com',  1),
    ('Carlos Ruiz',   'carlos@casatoro.com', 0); -- Usuario inactivo
GO

-- =============================================
-- CONSULTA PRINCIPAL - Actividad 4
-- Retorna por cada usuario activo:
--   - Cantidad de solicitudes
--   - Cantidad de seguimientos
--   - Fecha de la última solicitud
-- =============================================
SELECT
    u.Id                                AS UsuarioId,
    u.Nombre                            AS NombreUsuario,
    u.Email                             AS Email,
    COUNT(DISTINCT s.Id)                AS CantidadSolicitudes,
    COUNT(seg.Id)                       AS CantidadSeguimientos,
    MAX(s.Fecha)                        AS FechaUltimaSolicitud
FROM
    Usuarios u
    LEFT JOIN Solicitudes s
        ON s.UsuarioId = u.Id
    LEFT JOIN Seguimientos seg
        ON seg.SolicitudId = s.Id
WHERE
    u.Activo = 1  -- Solo usuarios activos
GROUP BY
    u.Id,
    u.Nombre,
    u.Email
ORDER BY
    FechaUltimaSolicitud DESC;
GO