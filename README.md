# Test para FullStack Ensitech
  -- BibliotecaApi

## tecnologías:

- **Backend**
Net Core SDK 8.0

- **Bases de Datos**
EF - InMemoryDatabase

## Entorno

Comando para verficar versiones de sdk dotnet instaladas, para desarrollo de este proyecto fue usado
la versión netcore 8.0, pero esté puede ser corrido con versiones superiores, o en caso de usar
Visual Studio, este solicitara la versión requerida en caso de no poder funcionar con la encontrada.

```sh
dotnet --list-sdks
```
## Visual Studio

Para ejecución del proyecto estamos haciendo uso de Visual Studio Code 1.88.1

### Configuraciones de entorno Backend (BibliotecaApi)

1. Editar archivo launchSettings.json, clave applicationUrl, para agregar o modificar url de la aplicacion -- Por Defecto: http://localhost:5024
2. Base de datos InMemoria Configurada en Program.cs en la linea 84 clave UseInMemoryDatabase con el nombre AuthorDb

## Data insertada 

El Proyecto BibliotecaApi cuenta con una inserción de datos semillas configurada en el archivo DbModel/DBSeeder.cs
la cual se inserta al iniciar el proyecto luego de crear en memoria la base de datos, este proceso es invocado
en la línea 108 del archivo Program.cs 

## Correr proyecto

      comando: 
        dotnet build
        dotnet run

Una vez se encuentre ejecutando el proyecto en local deberá poder acceder a este 
haciendo uso de las siguiente URL local

> http://localhost:5024/api/swagger  -API

## Credenciales para autenticación

Administrador
- Admin
- Superadmin@123

Usuario1
- Usuario1
- Seguro@123

Usuario2
- Usuario2
- Seguro@123

