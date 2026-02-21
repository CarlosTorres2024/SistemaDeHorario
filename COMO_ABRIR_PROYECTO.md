# Guía de Uso - Sistema de Gestión de Horarios

## 1. Cómo abrir en Visual Studio
1. Navegue a la carpeta del proyecto.
2. Busque el archivo **`SistemaGestionHorarios.sln`** (Archivo de Solución).
3. Haga doble clic para abrirlo. Visual Studio cargará el proyecto completo.
4. Presione `F5` o el botón "Iniciar" (play verde) para compilar y ejecutar el sistema.
   - **Nota Importante:** La primera vez que inicie, el sistema creará automáticamente la base de datos `SistemaHorarios`.

## 2. Cómo ver la Base de Datos en SQL Server
La aplicación usa **LocalDB** por defecto. Para ver las tablas y datos:

### Opción A: Desde Visual Studio (Más fácil)
1. En Visual Studio, vaya al menú **Ver** > **Explorador de objetos de SQL Server**.
2. Despliegue el nodo **SQL Server** > **(localdb)\MSSQLLocalDB**.
3. Busque la base de datos llamada **`SistemaHorarios`**.
4. Ahí podrá ver las Tablas (`Docentes`, `Horarios`, etc.) y hacer consultas (clic derecho > Ver datos).

### Opción B: Desde SSMS (SQL Server Management Studio)
1. Abra SSMS.
2. En "Server name" (Nombre del servidor), escriba: `(localdb)\mssqllocaldb`
3. Autenticación: `Windows Authentication`.
4. Conecte y busque la base de datos `SistemaHorarios`.

## 3. Configuración (Cambiar Base de Datos)
Si necesita usar una instancia propia de SQL Server (como `SQLEXPRESS` o un servidor remoto) en lugar de LocalDB, siga estos pasos:

1. Abra el archivo **`appsettings.json`** en la raíz del proyecto.
2. Busque la sección `"DefaultConnection"`.
3. Reemplace el valor por su cadena de conexión.

> [!IMPORTANT]
> **Ejemplo para SQL Express (Instancia Local):**
> ```json
> "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SistemaHorarios;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
> ```
> 
> **Ejemplo para Servidor Remoto (Con Usuario/Contraseña):**
> ```json
> "DefaultConnection": "Server=MI_IP;Database=SistemaHorarios;User Id=mi_usuario;Password=mi_password;MultipleActiveResultSets=true;TrustServerCertificate=True"
> ```

4. Guarde los cambios y ejecute el proyecto (`F5`). El sistema creará las tablas automáticamente.

## 4. Solución de Problemas Comunes
- **Base de datos bloqueada**: Si recibe errores de base de datos en uso, asegúrese de no tener la tabla abierta en modo edición en VS mientras ejecuta el programa.
