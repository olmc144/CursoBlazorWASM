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
mkdir MiBlazorMudApp
cd MiBlazorMudApp

# 2. Crear la estructura del proyecto Blazor WASM (ejecutado dentro de un contenedor .NET 9)
# Nota: La estructura será MiBlazorMudApp/MiBlazorMudApp.csproj
docker run --rm -v "$(pwd):/app" -w /app [mcr.microsoft.com/dotnet/sdk:9.0](https://mcr.microsoft.com/dotnet/sdk:9.0) dotnet new blazorwasm -n MiBlazorMudApp

# 3. Entrar a la carpeta del proyecto recién creada y añadir MudBlazor
cd MiBlazorMudApp
docker run --rm -v "$(pwd):/app" -w /app [mcr.microsoft.com/dotnet/sdk:9.0](https://mcr.microsoft.com/dotnet/sdk:9.0) dotnet add package MudBlazor
```

### 1.2. Solución de Permisos (Candado 🔒)
La carpeta del proyecto fue creada por el usuario root del contenedor. Debes cambiar el propietario recursivamente para tu usuario actual de Ubuntu.
```bash
# Ejecutar desde el directorio PAI (la carpeta raíz que creaste en el paso 1)
sudo chown -R $(whoami):$(id -g -n) MiBlazorMudApp
```

## 🛠️ 2. Entorno de Desarrollo (VS Code Dev Containers)

Utilizamos Dev Containers para ejecutar el SDK de .NET 9 en un contenedor aislado, mientras editamos el código en Ubuntu.

### 2.1. Configuración del Contenedor (`.devcontainer/devcontainer.json`)
Crea la carpeta `.devcontainer` en la raíz de tu proyecto (la carpeta que contiene la subcarpeta `MiBlazorMudApp/`). Dentro, crea el archivo `devcontainer.json`:
```bash
{
    "name": ".NET 9 Blazor WASM",
    "image": "[mcr.microsoft.com/dotnet/sdk:9.0](https://mcr.microsoft.com/dotnet/sdk:9.0)",
    
    // Ruta corregida: Debe apuntar a la subcarpeta que contiene el .csproj
    "workspaceFolder": "/workspace/MiBlazorMudApp",
    
    // Comando para descargar todas las dependencias al iniciar
    "postCreateCommand": "dotnet restore",
    
    "customizations": {
        "vscode": {
            "extensions": [
                "ms-dotnettools.csharp",
                "ms-dotnettools.csdevkit",
                "ms-azuretools.vscode-docker"
            ]
        }
    },
    
    // Mapear los puertos para acceder desde el navegador en el host (Ubuntu)
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