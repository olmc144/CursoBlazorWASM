# 🚀 MiBlazorMudApp: Blazor WebAssembly, .NET 9 y MudBlazor en Docker

Este proyecto está configurado para desarrollarse de manera totalmente multiplataforma utilizando **Docker** para aislar el SDK de .NET 9 del sistema operativo anfitrión (Ubuntu).

## 💡 Stack Tecnológico

* **Frontend:** Blazor WebAssembly (WASM) con .NET 9
* **UI Components:** MudBlazor
* **Base de Datos:** SQL Server (Gestionada vía `docker-compose`)
* **Entorno de Desarrollo:** Docker (Dev Containers) en Ubuntu

***

## ⚙️ 1. Configuración Inicial del Proyecto (Usando Docker)

Para evitar la instalación del SDK de .NET 9 en Ubuntu, utilizamos un contenedor temporal para generar la estructura del proyecto.

### 1.1. Creación de la Estructura Base

Ejecuta estos comandos desde la carpeta raíz donde deseas que se aloje **todo** el proyecto. **Importante**: Se utiliza el comando en una sola línea para evitar errores de referencia en Docker.

```bash
# 1. Crear la carpeta del proyecto
mkdir MiBlazorMudAppSolution
cd MiBlazorMudAppSolution

# 2. Crea el Archivo de Solución (.sln) en la raíz (ejecutado dentro de un contenedor Docker):
docker run --rm -v "$(pwd):/app" -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet new sln -n MiBlazorMudAppSolution
# Esto genera MiBlazorMudAppSolution.sln

# 3. Crear el proyecto Web API y forzar la salida (-o) para evitar anidaciones
docker run --rm -v "$(pwd):/app" -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet new webapi -n MiBlazorMudApp.Backend -o MiBlazorMudApp.Backend

# 4. Agregar el proyecto de API a la solución
dotnet sln MiBlazorMudAppSolution.sln add MiBlazorMudApp.Backend/MiBlazorMudApp.Backend.csproj

# 5. Crear el proyecto Blazor WASM y forzar la salida (-o)
docker run --rm -v "$(pwd):/app" -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet new blazorwasm -n MiBlazorMudApp -o MiBlazorMudApp

# 6. Agregar el proyecto de Frontend a la solución
dotnet sln add MiBlazorMudApp/MiBlazorMudApp.csproj

# 7. Agregar el paquete MudBlazor al proyecto Frontend
dotnet add MiBlazorMudApp/MiBlazorMudApp.csproj package MudBlazor

#8. Crear el Proyecto de Librería .NET Standard
docker run --rm -v "$(pwd):/app" -w /app mcr.microsoft.com/dotnet/sdk:9.0 \
dotnet new classlib -n MiBlazorMudApp.DataAccess -o MiBlazorMudApp.DataAccess

# 9. Agregar la Librería a la Solución
docker run --rm -v "$(pwd):/app" -w /app mcr.microsoft.com/dotnet/sdk:9.0 \
dotnet sln MiBlazorMudAppSolution.sln add MiBlazorMudApp.Helpers/MiBlazorMudApp.Helpers.csproj

# 10. Añadir una referencia de proyecto del Backend al proyecto de la capa de acceso a datos
dotnet add MiBlazorMudApp.Backend/MiBlazorMudApp.Backend.csproj reference MiBlazorMudApp.AccessData/MiBlazorMudApp.AccessData.csproj

# 11. Instalar dotnet-ef para migraciones
dotnet tool install --global dotnet-ef --version 9.0.9-*
export PATH="$PATH:/root/.dotnet/tools"

# 11. Agregar migraciones
dotnet ef migrations add InitialMigration

# 12. Actualizar migración
dotnet ef database update
```

### 1.2. Solución de Permisos (Candado 🔒)
La carpeta del proyecto fue creada por el usuario root del contenedor. Debes cambiar el propietario recursivamente para tu usuario actual de Ubuntu.
```bash
# Ejecutar desde el directorio raíz de la SOLUCIÓN (MiBlazorMudAppSolution/)
sudo chown -R $(whoami):$(id -g -n) .
```

## 🛠️ 2. Entorno de Desarrollo (VS Code Dev Containers)

Utilizamos Dev Containers para ejecutar el SDK de .NET 9 en un contenedor aislado, mientras editamos el código en Ubuntu.

### 2.1. Configuración del Contenedor (`.devcontainer/devcontainer.json`) (Opcional)
Crea la carpeta `.devcontainer` en la raíz de tu proyecto (la carpeta que contiene la subcarpeta `MiBlazorMudAppSolution/`). Dentro, crea el archivo `devcontainer.json`:
```bash
{
    "name": ".NET 9 MiBlazorMud",

    // Referencia el archivo docker-compose
    "dockerComposeFile": ["../docker-compose.yml"],

    // Servicio principal (dev container) que debe gestionar tu código (DEBES crear un servicio 'dev' en el .yml)
    "service": "dev", 

    // Le dice a VS Code qué servicios adicionales debe arrancar (mssql)
    "runServices": ["dev", "mssql"], 
    
    // Abrir la carpeta del proyecto dentro del contenedor
    "workspaceFolder": "/workspaces/New",
    
    // Montar la carpeta raíz de tu proyecto dentro del contenedor
    "postCreateCommand": "dotnet restore",
    
    // Instalar extensiones de VS Code automáticamente dentro del contenedor
    "customizations": {
        "vscode": {
            "extensions": [
                "ms-dotnettools.csharp",
                "ms-dotnettools.csdevkit",
                "ms-azuretools.vscode-docker"
            ]
        }
    },
    
    // Mapear el puerto para la ejecución
    "forwardPorts": [5000, 5001]
}
```

### 2.2. Inicio y Ejecución con Hot Reload
Abre Visual Studio Code en la **carpeta raíz** (la que contiene `.devcontainer/`).

Cuando se solicite, selecciona **"Reopen in Container"**.

Para ejecutar y **habilitar la recarga en caliente (Hot Reload)**, usa el siguiente comando en el terminal de VS Code. Esto es crucial, ya que soluciona los problemas de detección de archivos en volúmenes Docker.

```bash
# COMANDO DE EJECUCIÓN FINAL CON HOT RELOAD FUNCIONAL
DOTNET_USE_POLLING_FILE_WATCHER=true dotnet watch run --non-interactive
```
