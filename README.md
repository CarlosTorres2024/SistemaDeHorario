# Sistema de Gestión de Horarios Escolares

Este sistema permite la gestión integral de horarios para instituciones educativas, incluyendo docentes, materias, aulas, grupos y la generación de reportes en PDF con validación de conflictos.

## Requisitos Previos

Antes de ejecutar el proyecto, asegúrese de tener instalado:
- **.NET 8.0 SDK** o superior.
- **Visual Studio 2022** (con la carga de trabajo "Desarrollo de ASP.NET y web").
- **LocalDB** (instancia por defecto de SQL Server que viene con Visual Studio).

## Instrucciones para Ejecutar el Proyecto

Siga estos pasos para poner en marcha la aplicación:

1.  **Descarga**: Clone el repositorio o descomprima el código fuente en una carpeta local.
2.  **Abrir Proyecto**: Haga doble clic en el archivo **`SistemaGestionHorarios.sln`** para abrirlo en Visual Studio.
3.  **Configuración de Base de Datos**: 
    - Por defecto, el sistema utiliza **LocalDB**. 
    - No es necesario ejecutar scripts SQL manualmente; el sistema utiliza `context.Database.EnsureCreated()` en el primer inicio para crear las tablas automáticamente.
4.  **Ejecución**: Presione **`F5`** o el botón "Iniciar" en Visual Studio.
 
 
 ---

## Instrucciones 

Si desea utilizar su propia instancia de SQL Server en lugar de LocalDB, siga estos pasos:

1. Abra el archivo **`appsettings.json`** en la raíz del proyecto.
2. Localice la sección `ConnectionStrings` -> `DefaultConnection`.
3. Cambie el valor por su cadena de conexión. 

**Ejemplo para SQL Express:**
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=SistemaHorarios;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

> [!TIP]
> Para más detalles sobre la configuración, consulte la guía detallada: [COMO_ABRIR_PROYECTO.md](./COMO_ABRIR_PROYECTO.md)

---

## Arquitectura del Proyecto (Requisitos)

Siguiendo las especificaciones técnicas solicitadas, el proyecto se divide en las siguientes capas:

1.  **Domain / Entities**: Ubicada en la carpeta `Models`, define la estructura de datos (incluyendo la entidad `ScheduleItem`).
2.  **Persistence / Infrastructure**: Implementada con **Entity Framework Core 8** y SQL Server. El contexto se encuentra en `Data/AppDbContext.cs`.
3.  **API / Application**: Controladores de lógica de negocio y API REST (`ScheduleController`).
4.  **Frontend**: Interfaz de usuario dinámica desarrollada con **ASP.NET Core MVC**.

## Documentación de la API

El sistema incluye un endpoint de API para la integración con otros sistemas:

-   **GET `/api/schedule`**: Retorna la lista completa de horarios en formato JSON.
-   **POST `/api/schedule`**: Permite registrar un nuevo bloque de horario. 
    *Ejemplo de cuerpo (JSON):*
    ```json
    {
      "SubjectName": "Matemáticas",
      "TeacherName": "Juan Pérez",
      "DayOfWeek": "Lunes",
      "StartTime": "08:00",
      "EndTime": "08:50",
      "RoomNumber": "101",
      "GradeLevel": "4to de Media"
    }
    ```


---

## Tecnologías Utilizadas
- ASP.NET Core MVC 8.0
- Entity Framework Core (Code First)
- SQL Server (LocalDB / Express)
- API REST
- Bootstrap 5 + BoxIcons
- Rotativa.AspNetCore (Reportes PDF)
