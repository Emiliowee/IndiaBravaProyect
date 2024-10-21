# India Brava 
## Configuración de la Base de Datos

Para crear la base de datos con los datos iniciales, sigue estos pasos:

1. Tener instalado .NET SDK.

2. Ejecutar el siguiente comando para instalar la herramienta de Entity Framework Core:
   ```
   dotnet tool install --global dotnet-ef
   ```

3. Actualiza la cadena de conexión en el archivo `appsettings.json` .

5. Ejecuta el siguiente comando para aplicar las migraciones y crear la base de datos:
   ```
   dotnet ef database update
   ```
6. Ejecuta la aplicación con el comando:
   ```
   dotnet run
   ```
