# Sistema de Gestion de Horarios

Este proyecto es una aplicacion web para la gestion de horarios escolares, desarrollada con ASP.NET Core MVC y Entity Framework Core.

## Requisitos Previos

Para ejecutar el proyecto en cualquier PC, asegurese de tener instalado lo siguiente:

1. .NET SDK (Version 8.0 o superior recomendada).
2. SQL Server o SQL Server Express (El sistema usa LocalDB por defecto).
3. Navegador web moderno.

## Instrucciones para Ejecutar el Proyecto

### Opcion A: Usando Visual Studio (Recomendado)

1. Abra la carpeta del proyecto y busque el archivo "SistemaGestionHorarios.sln".
2. Haga doble clic en el archivo para abrirlo con Visual Studio.
3. El sistema restaurara automaticamente los paquetes NuGet necesarios.
4. Presione la tecla F5 o haga clic en el boton "Iniciar" para ejecutar la aplicacion.

### Opcion B: Usando la Terminal (Consola)

1. Abra una terminal en la raiz del proyecto ("SistemaDeHorario").
2. Ejecute el siguiente comando para restaurar dependencias:
   dotnet restore
3. Ejecute el comando para iniciar la aplicacion:
   dotnet run
4. Abra su navegador y acceda a la direccion que se indique en la terminal (usualmente https://localhost:5001 o http://localhost:5000).

## Configuracion de la Base de Datos

La aplicacion esta configurada para usar "LocalDB" de forma predeterminada. La base de datos se creara automaticamente la primera vez que se ejecute el sistema.

Si desea usar una instancia diferente de SQL Server, siga estos pasos:

1. Abra el archivo "appsettings.json".
2. Modifique la linea "DefaultConnection" dentro de "ConnectionStrings" con sus propios datos de conexion.
   Ejemplo para SQL Server: 
   "Server=NOMBRE_DE_TU_SERVIDOR;Database=SistemaHorarios;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
3. Guarde el archivo y vuelva a ejecutar el proyecto.


## Notas Adicionales

- El sistema incluye validaciones automaticas para evitar conflictos en los horarios de docentes, aulas y grupos.
- Los datos del centro educativo pueden ser configurados directamente desde el menu principal en la seccion "Centro Educativo".
