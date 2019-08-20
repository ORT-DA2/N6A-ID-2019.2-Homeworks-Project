# Clase 2 - Entity Framework Core - Code First

## Paquetes Necesarios

Paquete | Descripción
------------ | -------------
Microsoft.EntityFrameworkCore| EF Core
Microsoft.EntityFrameworkCore.Design| Contiene toda la lógica de design-time para EF Core. Contiene clases que nos servirán para indicarle a EF Tools por ejemplo como crear un contexto.
Microsoft.EntityFrameworkCore.SqlServer| Es el provider para la bd Microsoft SQL Server
Microsoft.EntityFrameworkCore.Tools| Este paquete permite la ejecución de comandos de entity framework (dotnet ef). Este permite hacer más fácil realizar varias tareas de EF Core, como: migraciones, scaffolding, etc
Microsoft.EntityFrameworkCore.InMemory| (Opcional) Es un provider para bd en Memoria, es sobretodo útil para testing.

## DB Context de Referencia

```c#
namespace Homeworks.DataAccess
{
    public class HomeworksContext : DbContext
    {
        public DbSet<Homework> Homeworks {get; set;}
        public DbSet<Exercise> Exercises {get; set;}

        public HomeworksContext(DbContextOptions options) : base(options) { }
    }
}
```

## Microsoft SQL Server

Primero que crearemos la clase ContextFactory. Esta tiene la responsabilidad de crear instancias del db context, tanto en memoria como en MSQLS.

```c#
using Microsoft.EntityFrameworkCore;

namespace Homeworks.DataAccess
{
    public class ContextFactory
    {
        public static HomeworksContext GetMemoryContext(string nameBd) { //BD EN MEMORIA
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            return new HomeworksContext(GetMemoryConfig(builder, nameBd));
        }

        private static DbContextOptions GetMemoryConfig(DbContextOptionsBuilder builder, string nameBd) {
            builder.UseInMemoryDatabase(nameBd);
            return builder.Options;
        }

        public static HomeworksContext GetSqlContext() { //BD EN SQL
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            return new HomeworksContext(GetSqlConfig(builder));
        }

        private static DbContextOptions GetSqlConfig(DbContextOptionsBuilder builder) {
            builder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=HomeworksDB;
                Trusted_Connection=True;MultipleActiveResultSets=True;");
            return builder.Options;
        }
    }
}
```

## Creación de la BD

EF Core no cuenta con migraciones automáticas. Si usamos una base de datos relacional SQL (!InMemory) entonces debemos crear las migraciones. Hay varias formas de hacer esto, pararnos sobre el proyecto del Contexto y referencia al de arranque con los .json de configuración. O lo inverso.

Antes que nada tenemos que estar seguro de tenes instalado el paquete "Microsoft.EntityFrameworkCore.Tools" en nuestro DataAccess para esto nos dirigiremos a este y

```PowerShell
 cd Homeworks.DataAcess
 dotnet add package Microsoft.EntityFrameworkCore.Tools
```

Desde la raíz del proyecto:

```PowerShell
 cd Homeworks.DataAcess
 dotnet ef migrations add MyMigration --startup-project="..\Homeworks.WebApi\"
```

Output: Si vamos al proyecto DataAccess nos debió haber generado una carpeta llamada Migrations con la migración.

![Imagen CreateHomeworksDB](../imgs/migracionCreateDB.PNG)

Después de crear la migración es necesario ejecutarla para eso utilizaremos el siguiente comando:

```PowerShell
 dotnet ef database update --startup-project="..\Homeworks.WebApi\"
```

Output:
![Imagen UpdateHomeworksDB](../imgs/migracionUpdateDB.PNG)

## Migraciones

Las migraciones son la manera de mantener el schema de la BD sincronizado con el Dominio, por esto cada vez que se modifica el dominio se deberá crear una migración.

Commando | Descripción
------------ | -------------
dotnet ef migrations add NOMBRE_DE_LA_MIGRATION| Este comando creará la migración. Crea 3 archivos .cs 1) <timestamp>_<migration name>: Contiene las operaciones Up() y Down() que se aplicaran a la BD para remover o añadir objetos. 2) <timestamp>_<migration name>.Designer: Contiene la metadata que va a ser usada por EF Core. 3) <contextname>ModelSnapshot: Contiene un snapshot del modelo actual. Que será usada para determinar qué cambio cuando se realice la siguiente migración.
dotnet ef database update| Este comando crea la BD en base al context, las clases del dominio y el snapshot de la migración.
dotnet ef migrations remove| Este comando remueve la ultima migración y revierte el snapshot a la migración anterior. Esto solo puede ocurrir si la migración no fue aplicada todavía.
dotnet ef database update NOMBRE_DE_LA_MIGRATION| Este commando lleva la BD al migración del nombre NOMBRE_DE_LA_MIGRATION.
  
## Memoria

Es conveniente para testing usar el provider InMemory, este nos permite tener una base de datos en memoria. Permitiéndonos no impactar en la BD real.
Para este en particular simplemente se requiere en el builder del context
usar builder.UseInMemoryDatabase y simplemente pasarle como parámetro un string con el nombre de la bd. Ver FactoryContext más arriba.

## Mas Info

* [EF Core Doc](http://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)
