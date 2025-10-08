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

### 1.1. Creación del Proyecto
Ejecuta los siguientes comandos desde el directorio donde quieres alojar tu proyecto. **Importante:** Se utiliza el comando en una sola línea para evitar errores de referencia en Docker.

```bash
# 1. Crear la carpeta del proyecto
mkdir MiBlazorMudApp
cd MiBlazorMudApp

# 2. Crear la estructura del proyecto Blazor WASM (ejecutado dentro de un contenedor .NET 9)
docker run --rm -v "$(pwd):/app" -w /app [mcr.microsoft.com/dotnet/sdk:9.0](https://mcr.microsoft.com/dotnet/sdk:9.0) dotnet new blazorwasm -n MiBlazorMudApp

# 3. Entrar a la carpeta del proyecto y añadir MudBlazor
cd MiBlazorMudApp
docker run --rm -v "$(pwd):/app" -w /app [mcr.microsoft.com/dotnet/sdk:9.0](https://mcr.microsoft.com/dotnet/sdk:9.0) dotnet add package MudBlazor