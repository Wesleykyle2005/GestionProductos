# Sistema de Gestión de Productos

Sistema de escritorio desarrollado en C# (.NET Framework) con WPF para la interfaz gráfica y SQL Server como base de datos. Permite la gestión de productos, usuarios y opciones mediante login, registro con validaciones, filtros de búsqueda y administración de estados.

## Tabla de contenidos

- [Características principales](#características-principales)
- [Requisitos previos](#requisitos-previos)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Modelos de datos](#modelos-de-datos)
- [Tecnologías utilizadas](#tecnologías-utilizadas)

## Características principales

- **Interfaz gráfica**: Desarrollada con WPF para una experiencia de usuario intuitiva y moderna.
- **Base de datos**: Utiliza SQL Server para almacenar y gestionar información de productos y usuarios.
- **Gestión de productos**: Permite agregar, editar y buscar productos con filtros por estado.
- **Autenticación y registro**: Incluye login y registro de usuarios con validaciones y hashing de contraseñas.
- **Patrón MVVM**: Arquitectura Model-View-ViewModel para una mejor separación de responsabilidades.
- **Entity Framework**: ORM para interactuar con la base de datos.
- **Seguridad**: Implementa hashing de contraseñas con BCrypt para proteger datos de usuarios.

## Requisitos previos

- .NET Framework 4.8 o superior
- SQL Server 2019 o superior (Express o estándar)
- Visual Studio 2022 o superior

## Estructura del proyecto

```plaintext
+---Models
|   Opcion.cs              # Modelo para las opciones de productos
|   Producto.cs            # Modelo para los productos
|   Usuario.cs             # Modelo para los usuarios
|
+---Services
|   DbContextFactory.cs     # Fábrica para instancias de DbContext
|   GestionProductosContext.cs # DbContext para la base de datos
|   IUserService.cs         # Interfaz para el servicio de usuarios
|   UserService.cs          # Lógica para registro y autenticación
|
+---ViewModels
|   AuthViewModel.cs       # Lógica de presentación para autenticación
|
+---Views
    AuthWindow.xaml        # Interfaz gráfica para autenticación
    AuthWindow.xaml.cs     # Código de la vista
```

## Modelos de datos

[![](https://mermaid.ink/img/pako:eNqNU01v2zAM_SsCr7OL2I3jWocBW9phO3TbIb202kGxGEeoTQayXDQL8t8n22mWLi2wk8THj_dIUTso2SBIiONYUcm0spVUJEStt9x5KbB-7E2_xgalIOTYaDdANfPjgCgak2vdttdWV043iox1WHrLJBaf-2ghBr_46dh0pWexU6R8j3-w5MWcja34CLXeWarEd26WDl8F3jzb1iOVVh_hJXMtblqvzTsFAucTomE3Ctmf6vmxKXuRr9V8MyP8H3pG4S9dva3pjPSu7bSzfMZ6wN-mfcf5aYN1bU9aX249PvwKwsg73SLpr7pd_5s1Z-fwrNYCa1wx_cWvtceFbVB8wXKt5w71OJaTjo4PqiBRIOL4Y7g1mrbBOAxXirBY3iKF0UEElbMGpHcdRtCga3Rvwq4vp2BYNAUyXIdFA0X7kLPRdM_cvKQ57qo1yJWu22B1GxN0HlbvGIJk0M25Iw-yGCqA3MEzyLhILy4nyTRJszxLizSfRrANcJpcZHmSzmbpVZZl-Sy53Efwe2ANnkkxKaZZkYUjSa7yCNBYz-52_D7DL9r_AWESEjc?type=png)](https://mermaid.live/edit#pako:eNqNU01v2zAM_SsCr7OL2I3jWocBW9phO3TbIb202kGxGEeoTQayXDQL8t8n22mWLi2wk8THj_dIUTso2SBIiONYUcm0spVUJEStt9x5KbB-7E2_xgalIOTYaDdANfPjgCgak2vdttdWV043iox1WHrLJBaf-2ghBr_46dh0pWexU6R8j3-w5MWcja34CLXeWarEd26WDl8F3jzb1iOVVh_hJXMtblqvzTsFAucTomE3Ctmf6vmxKXuRr9V8MyP8H3pG4S9dva3pjPSu7bSzfMZ6wN-mfcf5aYN1bU9aX249PvwKwsg73SLpr7pd_5s1Z-fwrNYCa1wx_cWvtceFbVB8wXKt5w71OJaTjo4PqiBRIOL4Y7g1mrbBOAxXirBY3iKF0UEElbMGpHcdRtCga3Rvwq4vp2BYNAUyXIdFA0X7kLPRdM_cvKQ57qo1yJWu22B1GxN0HlbvGIJk0M25Iw-yGCqA3MEzyLhILy4nyTRJszxLizSfRrANcJpcZHmSzmbpVZZl-Sy53Efwe2ANnkkxKaZZkYUjSa7yCNBYz-52_D7DL9r_AWESEjc)


**Descripción**:
- **Usuario**: Almacena información de usuarios con contraseñas hasheadas.
- **Producto**: Contiene datos de productos, como nombre, descripción y estado.
- **Opcion**: Representa opciones configurables asociadas a los productos.

## Tecnologías utilizadas

- **C# (.NET Framework 4.8)**: Lenguaje principal
- **WPF**: Interfaz gráfica
- **SQL Server**: Base de datos
- **Entity Framework 6.5.1**: ORM para la base de datos
- **BCrypt.Net-Next 4.0.3**: Hashing de contraseñas
- **CommunityToolkit.Mvvm 8.4.0**: Soporte para MVVM
- **Microsoft.DependencyInjection 9.0.9**: Inyección de dependencias
- **Microsoft.Extensions.Logging 9.0.0**: Logging
- **Microsoft.Extensions.Logging.Debug 9.0.0**: Depuración