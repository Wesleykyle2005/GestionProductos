# Sistema de Gesti�n de Productos

Sistema de escritorio desarrollado en C# (.NET Framework) con WPF para la interfaz gr�fica y SQL Server como base de datos. Permite la gesti�n de productos, usuarios y opciones mediante login, registro con validaciones, filtros de b�squeda y administraci�n de estados.

## Tabla de contenidos

- [Caracter�sticas principales](#caracter�sticas-principales)
- [Requisitos previos](#requisitos-previos)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Modelos de datos](#modelos-de-datos)
- [Tecnolog�as utilizadas](#tecnolog�as-utilizadas)

## Caracter�sticas principales

- **Interfaz gr�fica**: Desarrollada con WPF para una experiencia de usuario intuitiva y moderna.
- **Base de datos**: Utiliza SQL Server para almacenar y gestionar informaci�n de productos y usuarios.
- **Gesti�n de productos**: Permite agregar, editar y buscar productos con filtros por estado.
- **Autenticaci�n y registro**: Incluye login y registro de usuarios con validaciones y hashing de contrase�as.
- **Patr�n MVVM**: Arquitectura Model-View-ViewModel para una mejor separaci�n de responsabilidades.
- **Entity Framework**: ORM para interactuar con la base de datos.
- **Seguridad**: Implementa hashing de contrase�as con BCrypt para proteger datos de usuarios.

## Requisitos previos

- .NET Framework 4.8 o superior
- SQL Server 2019 o superior (Express o est�ndar)
- Visual Studio 2022 o superior

## Estructura del proyecto

```plaintext
+---Models
|   Opcion.cs              # Modelo para las opciones de productos
|   Producto.cs            # Modelo para los productos
|   Usuario.cs             # Modelo para los usuarios
|
+---Services
|   DbContextFactory.cs     # F�brica para instancias de DbContext
|   GestionProductosContext.cs # DbContext para la base de datos
|   IUserService.cs         # Interfaz para el servicio de usuarios
|   UserService.cs          # L�gica para registro y autenticaci�n
|
+---ViewModels
|   AuthViewModel.cs       # L�gica de presentaci�n para autenticaci�n
|
+---Views
    AuthWindow.xaml        # Interfaz gr�fica para autenticaci�n
    AuthWindow.xaml.cs     # C�digo de la vista
```

## Modelos de datos

[![](https://mermaid.ink/img/pako:eNqNU01v2zAM_SsCr7OL2I3jWocBW9phO3TbIb202kGxGEeoTQayXDQL8t8n22mWLi2wk8THj_dIUTso2SBIiONYUcm0spVUJEStt9x5KbB-7E2_xgalIOTYaDdANfPjgCgak2vdttdWV043iox1WHrLJBaf-2ghBr_46dh0pWexU6R8j3-w5MWcja34CLXeWarEd26WDl8F3jzb1iOVVh_hJXMtblqvzTsFAucTomE3Ctmf6vmxKXuRr9V8MyP8H3pG4S9dva3pjPSu7bSzfMZ6wN-mfcf5aYN1bU9aX249PvwKwsg73SLpr7pd_5s1Z-fwrNYCa1wx_cWvtceFbVB8wXKt5w71OJaTjo4PqiBRIOL4Y7g1mrbBOAxXirBY3iKF0UEElbMGpHcdRtCga3Rvwq4vp2BYNAUyXIdFA0X7kLPRdM_cvKQ57qo1yJWu22B1GxN0HlbvGIJk0M25Iw-yGCqA3MEzyLhILy4nyTRJszxLizSfRrANcJpcZHmSzmbpVZZl-Sy53Efwe2ANnkkxKaZZkYUjSa7yCNBYz-52_D7DL9r_AWESEjc?type=png)](https://mermaid.live/edit#pako:eNqNU01v2zAM_SsCr7OL2I3jWocBW9phO3TbIb202kGxGEeoTQayXDQL8t8n22mWLi2wk8THj_dIUTso2SBIiONYUcm0spVUJEStt9x5KbB-7E2_xgalIOTYaDdANfPjgCgak2vdttdWV043iox1WHrLJBaf-2ghBr_46dh0pWexU6R8j3-w5MWcja34CLXeWarEd26WDl8F3jzb1iOVVh_hJXMtblqvzTsFAucTomE3Ctmf6vmxKXuRr9V8MyP8H3pG4S9dva3pjPSu7bSzfMZ6wN-mfcf5aYN1bU9aX249PvwKwsg73SLpr7pd_5s1Z-fwrNYCa1wx_cWvtceFbVB8wXKt5w71OJaTjo4PqiBRIOL4Y7g1mrbBOAxXirBY3iKF0UEElbMGpHcdRtCga3Rvwq4vp2BYNAUyXIdFA0X7kLPRdM_cvKQ57qo1yJWu22B1GxN0HlbvGIJk0M25Iw-yGCqA3MEzyLhILy4nyTRJszxLizSfRrANcJpcZHmSzmbpVZZl-Sy53Efwe2ANnkkxKaZZkYUjSa7yCNBYz-52_D7DL9r_AWESEjc)


**Descripci�n**:
- **Usuario**: Almacena informaci�n de usuarios con contrase�as hasheadas.
- **Producto**: Contiene datos de productos, como nombre, descripci�n y estado.
- **Opcion**: Representa opciones configurables asociadas a los productos.

## Tecnolog�as utilizadas

- **C# (.NET Framework 4.8)**: Lenguaje principal
- **WPF**: Interfaz gr�fica
- **SQL Server**: Base de datos
- **Entity Framework 6.5.1**: ORM para la base de datos
- **BCrypt.Net-Next 4.0.3**: Hashing de contrase�as
- **CommunityToolkit.Mvvm 8.4.0**: Soporte para MVVM
- **Microsoft.DependencyInjection 9.0.9**: Inyecci�n de dependencias
- **Microsoft.Extensions.Logging 9.0.0**: Logging
- **Microsoft.Extensions.Logging.Debug 9.0.0**: Depuraci�n