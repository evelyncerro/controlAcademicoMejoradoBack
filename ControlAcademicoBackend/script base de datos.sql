

CREATE TABLE usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(150) NOT NULL,
    rol VARCHAR(50) NOT NULL,
    user_insert VARCHAR(100) NOT NULL,
    user_update VARCHAR(100) DEFAULT NULL,
    fecha_sistema DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
    eliminado TINYINT(1) NOT NULL DEFAULT 0
);

CREATE TABLE asignatura (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    codigo VARCHAR(50) NOT NULL,
    max_clases_semana INT NOT NULL,
    user_insert VARCHAR(100) NOT NULL,
    user_update VARCHAR(100) DEFAULT NULL,
    fecha_sistema DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
    eliminado TINYINT(1) NOT NULL DEFAULT 0
);

CREATE TABLE schedules (
    id INT AUTO_INCREMENT PRIMARY KEY,
    dia VARCHAR(20) NOT NULL,
    hora_inicio VARCHAR(5) NOT NULL,
    hora_fin VARCHAR(5) NOT NULL,
    id_usuario INT NOT NULL,
    id_asignatura INT NOT NULL,
    user_insert VARCHAR(100) NOT NULL,
    user_update VARCHAR(100) DEFAULT NULL,
    fecha_sistema DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
    eliminado TINYINT(1) NOT NULL DEFAULT 0,
    CONSTRAINT fk_schedule_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuario(id)
        ON DELETE CASCADE,
    CONSTRAINT fk_schedule_asignatura
        FOREIGN KEY (id_asignatura) REFERENCES asignatura(id)
        ON DELETE CASCADE
);



CREATE INDEX idx_schedule_usuario ON schedules(id_usuario);
CREATE INDEX idx_schedule_asignatura ON schedules(id_asignatura);
CREATE INDEX idx_usuario_correo ON usuario(correo);


ALTER TABLE schedules
MODIFY hora_inicio TIME NOT NULL,
MODIFY hora_fin TIME NOT NULL;


-- INSERTS TABLA usuario
INSERT INTO usuario (nombre, correo, rol, user_insert)
VALUES 
('Juan Pérez', 'juan.perez@correo.com', 'docente', 'admin'),
('María Gómez', 'maria.gomez@correo.com', 'docente', 'admin'),
('Carlos Ruiz', 'carlos.ruiz@correo.com', 'estudiante', 'admin'),
('Ana Torres', 'ana.torres@correo.com', 'estudiante', 'admin'),
('Administrador', 'admin@sistema.com', 'admin', 'system');

-- INSERTS TABLA asignatura
INSERT INTO asignatura (nombre, codigo, max_clases_semana, user_insert)
VALUES
('Matemáticas', 'MAT101', 4, 'admin'),
('Programación', 'PROG201', 5, 'admin'),
('Base de Datos', 'BD301', 4, 'admin'),
('Redes', 'RED401', 3, 'admin'),
('Ingeniería de Software', 'ING501', 3, 'admin');


-- INSERTS TABLA schedules (horarios)

INSERT INTO schedules 
(dia, hora_inicio, hora_fin, id_usuario, id_asignatura, user_insert)
VALUES
('Lunes',    '07:00:00', '09:00:00', 1, 1, 'admin'), 
('Martes',   '09:00:00', '11:00:00', 1, 2, 'admin'),
('Miércoles','08:00:00', '10:00:00', 2, 3, 'admin'), 
('Jueves',   '10:00:00', '12:00:00', 2, 2, 'admin'), 
('Viernes',  '07:00:00', '09:00:00', 1, 5, 'admin'),
('Lunes',    '14:00:00', '16:00:00', 2, 4, 'admin'), 
('Martes',   '18:00:00', '20:00:00', 1, 3, 'admin'); 



