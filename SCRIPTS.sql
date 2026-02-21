
-- CREACIÓN DE TABLAS - SISTEMA DE HORARIOS


-- 1. TABLA DOCENTES
CREATE TABLE Docentes (
    IdDocente INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Especialidad NVARCHAR(100) NULL,
    HorasMaximas INT NOT NULL
);

-- 2. TABLA ASIGNATURAS
CREATE TABLE Asignaturas (
    IdAsignatura INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    HorasSemanales INT NOT NULL
);

-- 3. TABLA AULAS
CREATE TABLE Aulas (
    IdAula INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Capacidad INT NOT NULL
);

-- 4. TABLA GRUPOS
CREATE TABLE Grupos (
    IdGrupo INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    NivelEducativo NVARCHAR(50) NOT NULL
);

-- 5. TABLA HORARIOS (TABLA CENTRAL)
CREATE TABLE Horarios (
    IdHorario INT PRIMARY KEY IDENTITY(1,1),

    IdDocente INT NOT NULL,
    IdAsignatura INT NOT NULL,
    IdAula INT NOT NULL,
    IdGrupo INT NOT NULL,

    DiaSemana NVARCHAR(20) NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,

    CONSTRAINT FK_Horarios_Docentes
        FOREIGN KEY (IdDocente) REFERENCES Docentes(IdDocente),

    CONSTRAINT FK_Horarios_Asignaturas
        FOREIGN KEY (IdAsignatura) REFERENCES Asignaturas(IdAsignatura),

    CONSTRAINT FK_Horarios_Aulas
        FOREIGN KEY (IdAula) REFERENCES Aulas(IdAula),

    CONSTRAINT FK_Horarios_Grupos
        FOREIGN KEY (IdGrupo) REFERENCES Grupos(IdGrupo)
);