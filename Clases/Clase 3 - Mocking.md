# Clase 3 - Mocking y Testing

Vamos a estudiar cómo podemos probar nuestro código evitando probar también sus dependencias, asegurándonos que los errores se restringen únicamente a la sección de código que efectivamente queremos probar. Para ello, utilizaremos una herramienta que nos permitirá crear Mocks. La herramienta será Moq.

## ¿Qué son los Mocks?

Los mocks son una de las varios "test doubles" (es decir, objetos que no son reales respecto a nuestro dominio, y que se usan con finalidades de testing) que existen para probar nuestros sistemas. Los más conocidos son los Mocks y los Stubs, siendo la principal diferencia en ellos, el foco de lo que se está testeando.

Antes de hacer énfasis en tal diferencia, es importante aclarar que nos referiremos a la sección del sistema a probar como SUT -System under test-). Los Mocks, nos permiten verificar la interacción del SUT con sus dependencias. Los Stubs, nos permiten verificar el estado de los objetos que se pasan. Como queremos testear el comportamiento de nuestro código, utilizaremos los primeros.

## Tipos de Test Doubles

Tipo | Descripción
------------ | -------------
**Dummy** | Son objetos se pasan, pero nunca se usan. Por lo general, solo se utilizan para llenar listas de parámetros.
**Fake** | Son objetos funcionales, pero generalmente toman algún atajo que los hace inadecuados para la producción (una base de datos en la memoria es un buen ejemplo).
**Stubs** | Brindan respuestas predefinidas a las llamadas realizadas en el test, por lo general no responden a nada que no se use en el test.
**Spies** | Son Stubs pero que también registran cierta información cuando son invocados.
**Mocks** | Son objetos pre-programados con expetativas (son las llamadas que se espera que reciban), de todos estos objetos Mocks son los unicos que verifican el comportamiento. Los otros, solo verifican el estado.

## ¿Por qué los queremos usar?

Porque queremos probar objetos y la forma en que estos interactúan con otros objetos. Para ello crearemos instancias de Mocks, es decir, objetos que simulen el comportamiento externo (es decir, la interfaz), de un cierto objeto. Son objetos tontos, que no dependen de nadie, siendo útiles para aislar una cierta parte de la aplicación que queramos probar.

Hay ciertos casos en los que incluso los mocks son realmente la forma más adecuada de llevar a cabo pruebas unitarias.
Para esto deberemos modificar nuestro proyecto en lugar de atar los objetos dependientes en nuestros constructores los, "inyectaremos". Para esto todo objeto que quereamos mockear debe tener una interfaz definida. Ej: si queremos probar de manera independiente UserLogic, vamos a tener que sacar el constructor sin parámetros y agregar uno que reciba UserRepository pero como no tiene una interfaz bien definida la crearemos también y nuestro constructor de UserLogic recibirá esta interfaz como parámetro.

![alt text](http://tutorials.jenkov.com/images/java-unit-testing/testing-with-di-containers.png)

En consecuencia, generamos un **bajo acoplamiento** entre una clase y sus dependencias, lo cual nos facilita utilizar un framework de mocking. Especialmente para aquellos objetos que dependen de un recurso externo (una red, un archivo o una base de datos).

## Antes de Empezar

Instalaremos la extensión para VS Code: .NET Core Test Explorer, esta nos permitirá explorar las pruebas creadas con mayor facilidad.

![alt text](../imgs/netCoreTestExplorer.PNG)

Luego lo configuraremos para que detecte nuestros proyectos de prueba para esto agregaremos la siguiente configuración a nuestro espacio de trabajo:

![alt text](../imgs/testExplorerConfig.PNG)

## Empezando con Moq

## BusinessLogic

Para comenzar a utilizar Moq, comenzaremos probando nuestro paquete de BusinessLogic. Para ello, debemos crear un nuevo proyecto de tipo Librería de Clases (Homeworks.BusinessLogic.Tests) e instalarle Moq.

```PowerShell
dotnet new mstest -n Homeworks.BusinessLogic.Tests
dotnet sln add Homeworks.BusinessLogic.Tests
cd Homeworks.BusinessLogic.Tests
dotnet add package Moq
```

Agregamos las referencias a BusinessLogic, Domain y finalmente a DataAccess.Interface

Una vez que estos pasos estén prontos, podemos comenzar a realizar nuestro primer test. Creamos entonces la clase UserLogicTests, y en ella escribimos el primer `TestMethod`.

## Probando el Create User

```C#
[TestClass]
public class UserLogicTests
{
    [TestMethod]
    public void CreateValidUserTest()
    {
        //Arrange

        //Act

        //Assert
    }

}
```

Para ello seguiremos la metodología **AAA: Arrange, Act, Assert**.
En la sección de **Arrange**, construiremos los el objeto mock y se lo pasaremos al sistema a probar. En la sección de **Act**, ejecutaremos el sistema a probar. Por último, en la sección de **Assert**, verificaremos la interacción del SUT con el objeto mock.

Entonces:

1) Primero vamos a decirle que esperamos que sobre nuestro Mock que se llame a la función Add().
2) Luego vamos a indicarle que esperamos que se llame la función Save().
3) Invocamos Create
4) Verificamos que se hicieron las llamadas pertinentes, y realizamos Asserts

```C#
[TestMethod]
public void CreateValidUserTest()
{
    var user = new User
    {
        UserName = "Hola",
        Password = "Hola"
    };
    var mock = new Mock<IRepository<User>>(MockBehavior.Strict);
    mock.Setup(m => m.Add(It.IsAny<User>()));
    mock.Setup(m => m.Save());
    var userLogic = new UserLogic(mock.Object);

    var result = userLogic.Create(user);

    mock.VerifyAll();
    Assert.AreEqual(user.UserName, result.UserName);
}
```

Probando el caso particular el cual se recibe un usuario nulo:

```C#
[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void CreateNullUserTest()
{
    //var mock = new Mock<IRepository<User>>(MockBehavior.Strict);
    //var userLogic = new UserLogic(mock.Object);
    var userLogic = new UserLogic(null);

    var result = userLogic.Create(null);
}
```

## WebApi

Creamos nuestro proyecto:

```PowerShell
dotnet new mstest -n Homeworks.BusinessLogic.Tests
cd Homeworks.WebApi.Tests
dotnet add package Moq
```

Ademas vamos a necesitar **referencias a los paquetes de AspNet.Core**

```PowerShell
dotnet add package Microsoft.AspNetCore.Mvc.Abstractions
dotnet add package Microsoft.AspNetCore.Mvc.Core
```

Luego al proyecto de tests le agregaremos las referencias a WebApi, Domain y BusinessLogic.Interface
Creamos entonces la clase UsersControllerTests.

## Probando el Post

```C#
[TestMethod]
public void CreateValidUserTest()
{
    //Arrange
    var user = new UserModel
    {
        UserName = "Hola",
        Password = "Hola"
    };
    var mock = new Mock<IUserLogic>(MockBehavior.Strict);
    mock.Setup(m => m.Create(It.IsAny<User>())).Returns(user.ToEntity());
    var controller = new UsersController(mock.Object);

    //Act
    var result = controller.Post(user);
    var createdResult = result as CreatedAtRouteResult;
    var model = createdResult.Value as UserModel;

    //Assert
}
```

Sin embargo, nos falta definir el comportamiento que debe tener el mock del nuestro IUserLogic. Esto es lo que llamamos **expectativas** y lo que vamos asegurarnos que se cumpla al final de la prueba. Recordemos, los mocks simulan el comportamiento de nuestros objetos, siendo ese comportamiento lo que vamos a especificar a partir de expectativas. Para ello, usamos el método **Setup**.

### ¿Cómo saber qué expectativas asignar?

Esto va en función del método de prueba. Las expectativas se corresponden al caso de uso particular que estamos probando dentro de nuestro método de prueba. Si esperamos probar el Post() de nuestro UsersController, y queremos mockear la clase UserLogic, entonces las expectativas se corresponden a las llamadas que hace UsersController sobre UserLogic. Veamos el método a probar:

```C#
[HttpPost]
public IActionResult Post([FromBody]UserModel model)
{
    try {
        var user = users.Create(UserModel.ToEntity(model));
        return CreatedAtRoute("Get", new { id = user.Id }, UserModel.ToModel(user));
    } catch(ArgumentException e) {
        return BadRequest(e.Message);
    }
}
```

La línea que queremos mockear es la de:

```C#
var user = users.Create(UserModel.ToEntity(model));
```

Entonces:

1) Primero vamos a decirle que esperamos que sobre nuestro Mock que se llame a la función Create().
2) Luego vamos a indicarle que esperamos que tal función se retorne un user que definimos en otro lado.

Una vez que ejecutamos el método que queremos probar, también debemos verificar que se hicieron las llamadas pertinentes. Para esto usamos el método VerifyAll del mock.
Además, realizamos asserts (aquí estamos probando estado), para ver que los objetos usados son consistentes de acuerdo al resultado esperado.

```C#
[TestMethod]
public void CreateValidUserTest()
{
    var user = new UserModel
    {
        UserName = "Hola",
        Password = "Hola"
    };
    var mock = new Mock<IUserLogic>(MockBehavior.Strict);
    mock.Setup(m => m.Create(It.IsAny<User>())).Returns(user.ToEntity());
    var controller = new UsersController(mock.Object);

    var result = controller.Post(user);
    var createdResult = result as CreatedAtRouteResult;
    var model = createdResult.Value as UserModel;

    mock.VerifyAll();
    Assert.AreEqual(user.UserName, model.UserName);
}
```

Y voilá. Vemos que nuestro test pasa 😎!

## Mockeando excepciones

Ahora veamos como probar otros casos particulares, por ejemplo cuando nuestro ```Post()``` del Controller nos devuelve una **BadRequest**.

Particularmente, en el caso que hemos visto antes nuestro Controller retornaba CreatedAtRoute para dicho. Ahora, nos interesa probar el caso en el que nuestro Controller retorna una BadRequest. Particularmente esto se da cuando el método ```Create()``` recibe null. Seteamos entonces dichas expectativas y probemos.

```C#
[TestMethod]
public void CreateInvalidUserBadRequestTest()
{
    var mock = new Mock<IUserLogic>(MockBehavior.Strict);
    mock.Setup(m => m.Create(null)).Throws(new ArgumentException());
    var controller = new UsersController(mock.Object);

    var result = controller.Post(null);

    mock.VerifyAll();
    Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
}
```

Lo que hicimos fue indicar que cuando se invoque Create se lanze ```ArgumentException```. En consecuencia, nuestro controller al llamar a este mock, se lanzara ```ArgumentException``` causando que nuestro controller la capture y retorne ```BadRequest```.

Finalmente entonces, verificamos que las expectativas se hayan cumplido (con el ```VerifyAll()```), y luego que el resultado obtenido sea un ```BadRequestObjectResult```

## Mas Info

* [MOQ](https://github.com/moq/moq4)
* [Mocks Aren't Stubs](https://martinfowler.com/articles/mocksArentStubs.html)
