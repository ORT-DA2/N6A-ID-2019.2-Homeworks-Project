# Clase 1 - .NET Core

## Repaso .NET Framework

Es una plataforma de desarrollo de software. Que permite creara aplicaciones que se ejecutan en Windows.

![image](http://www.developerin.net/include/ArticleImages/Components.jpg)

**.NET Framework** provee herramientas y tecnologías para desarrollar en Windows tanto aplicaciones webs o enterprise.
Los principales componentes son:

1. CLR (Common Language Runtime): Este es el entorno de ejecución en el que corren todos los programas de .NET Framework. El código que corre bajo el CLR es llamado Managed Code (Ya que los programadores no tienen que procurarse por el manejo de memoria o de threads).
El compilador del lenguaje compilara el código del programa en un .dll o .exe que este contiene Metadata y CIL (Common Intermediate Language).
Luego el CLR a la hora de correr el programa vuelve a complicar el código una vez mas para generar código nativo esta conversion es realizada al momento de ejecutar las intrusiones y es llamada JIT (Just In Time)
2. BCL (Base Class Library): Esta librería básica contiene tipos que representan los tipos de datos incorporados en el CLI, EJ: Data Types, Basic Dile Access, Collections, etc.
3. FCL (Framework Class Library): Esta librería contiene ASP.NET, WinForms, XML, ADO.NET y mas.
4. CLS (Common language specification): Esta contiene un juego de reglas que todos los lenguajes deben satisfacer para poder ser compilado y ejecutados por el CLR.

## .NET Core

.NET Core otra implementación de .NET de microsoft las principales diferencias entre estas son:

1. Modelos de aplicación: .NET Core no es compatible con todos los modelos de aplicación de .NET Framework. En concreto, no es compatible con los formularios Web Forms ASP.NET ni ASP.NET MVC, pero sí con ASP.NET Core MVC. (Aunque: Se anunció que .NET Core 3 será compatible con WPF y Windows Forms.)
2. API: .NET Core contiene un amplio subconjunto de bibliotecas de clases base de .NET Framework con una factorización distinta (los nombres de ensamblado son distintos y los miembros expuestos en los tipos difieren en los casos clave). Estas diferencias requieren cambios en el origen del puerto de .NET Core en algunos casos (vea microsoft/dotnet-apiport). .NET Core implementa la especificación de la API .NET Standard.
3. Subsistemas: .NET Core implementa un subconjunto de los subsistemas de .NET Framework, de cara a una implementación y un modelo de programación más sencillos. Por ejemplo, no se admite seguridad de acceso del código (CAS), aunque se admite la reflexión.
4. Plataformas: .NET Framework admite Windows y Windows Server, mientras que .NET Core también es compatible con macOS y Linux.
5. Código abierto: .NET Core es código abierto, mientras que un subconjunto de .NET Framework de solo lectura es código abierto.

### Arquitectura

![iamge](https://www.dotnetcurry.com/images/dotnetcore/core-future/dotnet-core-architecture.png)

Componentes:

1. CoreCLR es un runtime que esta optimizado para multiplataforma y para desarrollos orientados a la nube. Es usado cuando se construyen aplicaciones en ASP.NET Core. Este CLR utiliza JIT
2. .NET Runtime: es el CLR para las Universal Apps de Windows y esta optimizado para correr negativamente esto quiere decir que no produce una compilación en JIT si no que sigue compilación AOT (Ahead of time) esto quiere decir que se compila directamente a código de maquina, asegurando una mejor performance.
3. Unified BCL (Base Class Library) consta de las clases básicas y fundamentales que forman parte de .NET Core. Esto también se llama como CoreFX. .NET Core sigue el modelo de NuGet para la entrega de paquetes para BCL.

## Xamarin

No entraremos en detalles sobre este Framework, solo diremos esta basado tambien en .NET y permite generar aplicaciones nativas para iOS, OS X y Android con la misma base de codigo (actualmente C#).

## .NET Standard

Al tener 3 frameworks distintos orientados a propositos distintos nos surge un problema, imaginen que necesito desarrollar 3 aplicaciones que tienen la misma logica de negocio.

* Windows Form (.NET Framework)
* UWP (.NET Core)
* Una aplicación en Android (Xamarin)

Estas 3 van a tener la misma lógica de negocio y van a estar desarrolladas en el mismo lenguaje (C#) pero como las librerías de estas no son compatibles entre si necesito que cada una de estas tenga un duplicado de la logica de negocio.
Por este problema surge .NET Standard que es un conjunto de APIs que todas las plataformas de .NET tienen que implementar. Esto unifica las plataformas de .NET

Ya que ahora si hago mi lógica de negocios en .NET Standard, solo necesito generar capas de presentación en cada uno de los frameworks establecidos y todos usaran la misma logica de negocios sin duplicarla.
*Nota importante: una proyecto en .NET Standard solo puede compilar a .dll, jamas un proyecto en Standard va a dar como resultado un ejecutable a la hora de ser compilado, tampoco es posible compilar una proyecto en Standard por si solo si no que este debe de compilarse a 1 de los otros frameworks*

![image](https://cdn-images-1.medium.com/max/2400/1*-bQofDO6WBkiru3Tu5VpMg.png)

## Cuando usar .NET Core

* Si vas a desarrollar una aplicacion multiplataforma.
* Si estas apuntando a microservicios
* Si necesitas alta eficiencia y escalabilidad.

## Cuando usar .NET Framework

* Si estas desarrollando una aplicación que usa .NET Framework
* Necesitas librerías que no se encuentran en .NET Core.
* La plataforma en la que vas a desarrollar no soporta .NET Core

## Mas Info

* [Xamarin](https://visualstudio.microsoft.com/es/xamarin)
* [Modern Web App wit ASP.NET Core](https://docs.microsoft.com/en-us/dotnet/standard/modern-web-apps-azure-architecture/)
* [.NET Core VS .NET Framework](https://docs.microsoft.com/en-us/dotnet/standard/choosing-core-framework-server)
* [Dotnet](https://docs.microsoft.com/es-es/dotnet/core/about)