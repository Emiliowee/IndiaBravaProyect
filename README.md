# India Brava - Aplicación de Gestión de Inventario

## Configuración de la Base de Datos

Para crear la base de datos con los datos iniciales, sigue estos pasos:

1. Asegúrate de tener instalado .NET 6.0 SDK o superior.

2. Abre una terminal en la carpeta raíz del proyecto.

3. Ejecuta el siguiente comando para instalar la herramienta de Entity Framework Core:
   ```
   dotnet tool install --global dotnet-ef
   ```

4. Actualiza la cadena de conexión en el archivo `appsettings.json` con los datos de tu servidor SQL Server local.

5. Ejecuta el siguiente comando para aplicar las migraciones y crear la base de datos:
   ```
   dotnet ef database update
   ```

6. La base de datos se creará con los datos iniciales incluidos.

7. Ejecuta la aplicación con el comando:
   ```
   dotnet run
   ```

¡Listo! Ahora tienes la aplicación en funcionamiento con la base de datos configurada.
