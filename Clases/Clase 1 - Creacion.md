# Clase 1 - Comandos Básicos

## Instalación de ambiente

* [.NET Core SDK](https://dotnet.microsoft.com/download)
* [Visual Studio Code](https://code.visualstudio.com/Download)
* [Postman](https://www.getpostman.com/apps)

## Commandos
Commando | Resultado
------------ | -------------
dotnet new sln| Creamos solucion (Es necesario para abrirlo en Visual Studio y para poder visualizar los errores en VS Code)
dotnet new webapi --no-https -n "Nombre del Proyecto"| Crear un nuevo Proyecto del template WebApi
dotnet new mstest -n "Nombre del Proyecto"| Crear un nuevo Proyecto del template Prueba Unitaria
dotnet new console -n "Nombre del Proyecto"| Crear un nuevo Proyecto del template Console
dotnet sln add | Asociamos el proyecto creado al .sln
dotnet new classlib -n "Nombre del Proyecto"| Crear un nueva libreria (standard)
dotnet add "Nombre del Proyecto 1".csproj reference "Nombre del Proyecto 2".csproj| Agrega una referencia al Proyecto 1 del Proyecto 2
dotnet add package "Nombre del Package" | Instala la Package al proyecto actual

## Commandos para creación de proyeto HomeworkWebApi

### Creamos el sln

```
dotnet new sln
```

### Creamos la libreria businesslogic.interface y la agregamos al sln

```PowerShell
dotnet new classlib -n Homeworks.BusinessLogic.Interface
dotnet sln add Homeworks.BusinessLogic.Interface
```

### Creamos la libreria businesslogic y la agregamos al sln

```PowerShell
dotnet new classlib -n Homeworks.BusinessLogic
dotnet sln add Homeworks.BusinessLogic
```

### Creamos la libreria dataaccess.interface y la agregamos al sln

```PowerShell
dotnet new classlib -n Homeworks.DataAccess.Interface
dotnet sln add Homeworks.DataAccess.Interface
```

### Creamos la libreria dataaccess y la agregamos al sln

```PowerShell
dotnet new classlib -n Homeworks.DataAccess
dotnet sln add Homeworks.DataAccess
```

### Creamos la libreria domain y la agregamos al sln

```PowerShell
dotnet new classlib -n Homeworks.Domain
dotnet sln add Homeworks.Domain
```

### Agregamos la referencia del domain al dataaccess

```PowerShell
dotnet add Homeworks.DataAccess reference Homeworks.Domain
dotnet add Homeworks.DataAccess reference Homeworks.DataAccess.Interface
```

### Agregamos las referencias de domain y dataaccess a businesslogic

```PowerShell
dotnet add Homeworks.BusinessLogic reference Homeworks.Domain
dotnet add Homeworks.BusinessLogic reference Homeworks.DataAccess.Interface
dotnet add Homeworks.BusinessLogic reference Homeworks.BusinessLogic.Interface
```

### Descargamos Entity Framework Core

Nos movemos a la carpeta dataaccess (cd Homeworks.DataAccess)

```PowerShell
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

# WebApi

## Creamos el proyecto webapi y lo agregamos al sln

```PowerShell
dotnet new webapi --no-https -n Homeworks.WebApi
dotnet sln add Homeworks.WebApi
```

### Agregamos referencias de los proyectos a la webapi

```PowerShell
dotnet add Homeworks.WebApi reference Homeworks.DataAccess
dotnet add Homeworks.WebApi reference Homeworks.DataAccess.Interface
dotnet add Homeworks.WebApi reference Homeworks.Domain
dotnet add Homeworks.WebApi reference Homeworks.BusinessLogic
dotnet add Homeworks.WebApi reference Homeworks.BusinessLogic.Interface
```

## EF WebApi

Nos movemos a la carpeta web api (cd Homeworks.WebApi)

```PowerShell
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## Mas Info

* [Comandos](https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet?tabs=netcore21)
