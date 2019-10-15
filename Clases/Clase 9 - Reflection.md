# Reflection

## Introducción a Reflection

Reflection es la habilidad de un programa de autoexaminarse con el objetivo de encontrar ensamblados (.dll), módulos, o información de tipos en tiempo de ejecución. En otras palabras, a nivel de código vamos a tener clases y objetos, que nos van a permitir referenciar a ensamblados, y a los tipos que se encuentran contenidos.

Se dice que un programa se refleja en sí mismo (de ahí el termino "reflección"), a partir de extraer metadata de sus assemblies y de usar esa metadata para ciertos fines. Ya sea para informarle al usuario o para modificar su comportamiento.

Al usar Reflection en C#, estamos pudiendo obtener la información detallada de un objeto, sus métodos, e incluso crear objetos e invocar sus métodos en tiempo de ejecución, sin haber tenido que realizar una referencia al ensamblado que contiene la clase y a su namespace.

Específicamente lo que nos permite usar Reflection es el namespace ```System.Reflecion```, que contiene clases e interfaces que nos permiten manejar todo lo mencionado anteriormente: ensamblados, tipos, métodos, campos, crear objetos, invocar métodos, etc.

## Estructura de un assembly/ensamblado

Los assemblies contienen módulos, los módulos contienen tipos y los tipos contienen miembros. Reflection provee clases para encapsular estos elementos. Entonces como dijimos posible utilizar reflection para crear dinámicamente instancias de un tipo, obtener el tipo de un objeto existente e invocarle métodos y acceder a sus atributos de manera dinámica.

![alt text](http://www.codeproject.com/KB/cs/DLR/structure.JPG)

## ¿Para qué podría servir?

Supongamos por ejemplo, que necesitamos que nuestra aplicación soporte diferentes tipos de loggers (mecanismos para registrar datos/eventos que van ocurriendo en el flujo del programa). Además, supongamos que hay desarrolladores terceros que nos brindan una .dll externa que escribe información de logger y la envía a un servidor. En ese caso, tenemos dos opciones:

1) Podemos referenciar al ensamblado directamente y llamar a sus métodos (como hemos hecho siempre)
2) Podemos usar Reflection para cargar el ensamblado y llamar a sus métodos a partir de sus interfaces.

En este caso, si quisiéramos que nuestra aplicación sea lo más desacoplada posible, de manera que otros loggers puedan ser agregados (o 'plugged in' -de ahí el nombre plugin-) de forma sencilla y SIN RECOMPILAR la aplicación, es necesario elegir la segunda opción.

Por ejemplo podríamos hacer que el usuario elija (a medida que está usando la aplicación), y descargue la .dll de logger para elegir usarla en la aplicación. La única forma de hacer esto es a partir de Reflection. De esta forma, podemos cargar ensamblados externos a nuestra aplicación, y cargar sus tipos en tiempo de ejecución.

## Favoreciendo el desacoplamiento

Lo que es importante para lograr el desacoplamiento de tipos externos, es que nuestro código referencie a una Interfaz, que es la que toda .dll externa va a tener que cumplir. Tiene que existir entonces ese contrato previo, de lo contrario, no sería posible saber de antemano qué métodos llamar de las librerías externas que poseen clases para usar loggers.

## Diagramas Clases y Paquetes Ejemplo [Ejemplo](../Codigo/Reflection)

![IMG](../imgs/DiagramaReflection.png)