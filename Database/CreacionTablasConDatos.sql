/*==============================================================
  Nombre del Script : CreacionTablasConDatos.sql
  Autor             : Wesley Kyle Ruiz Centeno
  Fecha de creación : 2025-09-18 18:06:00
  Descripción       : Script de creación de tablas base para el 
                      sistema de administración de productos.
                      Incluye:
                        - Productos
                        - Usuarios
                        - Opciones
                      Además inserta 10 productos con opciones 
                      asociadas como ejemplo.
==============================================================*/
CREATE DATABASE GestionProductos
GO
USE GestionProductos
GO
CREATE TABLE Productos(
	Codigo INT IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(100) UNIQUE NOT NULL,
	Existencia INT DEFAULT(0) NOT NULL,
		CONSTRAINT CK_Productos_Existencia CHECK (Existencia >=0),
	Estado BIT DEFAULT(1) NOT NULL,
	NombreProveedor VARCHAR(100)
)
GO

CREATE TABLE Usuarios(
	IdUsuario INT IDENTITY(1,1) PRIMARY KEY ,
	NombreUsuario VARCHAR(100) UNIQUE NOT NULL,
	Apellido VARCHAR(100), 
	ContrasenaHash VARCHAR(256) NOT NULL,
	Correo VARCHAR(100) UNIQUE NOT NULL,
	Telefono CHAR(8) UNIQUE,
	FechaCreacion DATETIME2 DEFAULT(SYSDATETIME())
)
GO

CREATE TABLE Opciones(
	IdOpcion INT IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(50) NOT NULL,
	CodigoProducto INT NOT NULL,
	Estado BIT DEFAULT(1) NOT NULL,
	CONSTRAINT FK_Opcion_Producto FOREIGN KEY (CodigoProducto) 
	REFERENCES Productos(Codigo)
	ON DELETE CASCADE
    ON UPDATE CASCADE
)
GO

/*==============================================================
  Inserción de datos
==============================================================*/

INSERT INTO Productos (Nombre, Existencia, Estado, NombreProveedor)
VALUES 
('Hamburguesa Clásica', 50, 1, 'Proveedor A'),
('Hamburguesa Doble', 40, 1, 'Proveedor A'),
('Vaso', 100, 1, 'Proveedor B'),
('Refresco', 120, 1, 'Proveedor B'),
('Juguete Carro', 30, 1, 'Proveedor C'),
('Juguete Muñeca', 25, 1, 'Proveedor C'),
('Pizza', 20, 1, 'Proveedor D'),
('Café', 70, 1, 'Proveedor E'),
('Taco', 60, 1, 'Proveedor F'),
('Helado', 80, 1, 'Proveedor G');
GO

INSERT INTO Opciones (Nombre, CodigoProducto, Estado)
VALUES
('Mediano', 1, 1),
('Grande', 1, 1),
('Extra Queso', 2, 1),
('Doble Carne', 2, 1),
('Pequeño', 3, 1),
('Grande', 3, 1),
('Cola', 4, 1),
('Naranja', 4, 1),
('Rojo', 5, 1),
('Azul', 5, 1),
('Rubia', 6, 1),
('Morena', 6, 1),
('Grande', 7, 1),
('Familiar', 7, 1),
('Americano', 8, 1),
('Capuchino', 8, 1),
('Pastor', 9, 1),
('Asada', 9, 1),
('Chocolate', 10, 1),
('Vainilla', 10, 1);
GO
