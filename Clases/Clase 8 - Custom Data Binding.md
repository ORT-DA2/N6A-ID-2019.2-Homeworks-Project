# Custom Data Binding

Los Data Binding son el mecanismo que tiene ASP.NET Core WebApi para convertir (parsear) el contenido json de nuestras request a nuestros objetos.
En la mayoría de los casos el conversor predeterminado es suficiente, pero hay algunas request de conversion que este no sabe que objeto crear por ejemplo cuando existe polimorfismo.

Por ejemplo en nuestra webapi de ejemplo tenemos nuestro modelo de usuario (**UserModel**) pero ahora queremos tener:

1) **TeacherModel** que hereda de UserModel y tiene una propiedad extra llamada TeacherId.
2) **StudentModel** que también hereda de UserModel y tiene una propiedad extra llamada StudentId.

Y el siguiente endpoint en UserController:

```C#
[HttpPost("Type")]
public IActionResult GetUserType([FromBody]UserModel model)
{
    if (model == null)
    {
        return BadRequest("The user is null");
    }
    if (model is TeacherModel) {
        return Ok(new object[] { "TeacherModel", model});
    }
    if (model is StudentModel) {
        return Ok(new object[] { "StudentModel", model});
    }
    return Ok(new object[] { "UserModel", model});
}
```

Al hacer las siguientes request por postman:

```json
{
  "userName": "admin",
  "name": "juan",
  "password": "admin",
  "isAdmin": true,
}
```

```json
{
  "userName": "admin",
  "name": "juan",
  "password": "admin",
  "isAdmin": true,
  "teacherId" "1",
}
```

```json
{
  "userName": "admin",
  "name": "juan",
  "password": "admin",
  "isAdmin": true,
  "studentId" "2",
}
```

Siempre nos va retornar:

```json
[
  "UserModel",
  {
    "userName": "admin",
    "name": "juan",
    "password": "admin",
    "isAdmin": true,
  }
]
```

Esto a que se debe a que nuestro parser predeterminado nos genera los objetos de la clase padre y no de sus hijos. Para resolver esto vamos a implementar nuestro propio JsonConverter para casos que necesitemos un Data Binding polimorfico.

## Creando nuestro JsonConverter

Para eso vamos a crear la clase **JsonCreationConverter** dentro de un namespace llamado Parsers en nuestra webapi, esta va a heredar de JsonConverter.

```C#
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Homeworks.WebApi.Parsers
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite { get => false; }

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("Reader");
            }
            if (serializer == null)
            {
                throw new ArgumentNullException("Serializer");
            }
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            JObject jObject = JObject.Load(reader);
            T target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected abstract T Create(Type objectType, JObject jObject);

    }
}
```

Esta clase va a ser abstracta y de ella van a heredar todos nuestros conversores polimorficos.
Tenemos que implementar los siguientes metodos:

1) **CanConvert**: Este método dice que tipo de clases puede convertir el conversor, nos interesa decir que va a convertir todos los tipos T y sus hijos.

2) **WriteJson**: Este metodo se invoca cuando se va a convertir de un objeto a json, como el conversor predeterminado es suficiente no vamos a implementarla.

3) **CanWrite**: Esta property establece si nuestro conversor puede convertir de un objeto a json y como fue dicho en el punto anterior, no va a ser en nuestro caso por ende va a retornar siempre **false**.

4) **ReadJson**: Este método se llama para convertir de json a un objeto, como la lógica de conversion va a ser común, vamos a generar un método **Create** abstracto que se va a encargar de crear el tipo de objeto que tenemos que rellenar con el json y esto se hace con el método **Populate**.

Ahora crearemos el Converter concreto que se va a encargar de convertir todos los tipo UserModel.
Para eso vamos a crear la clase **UserModelJsonConverter** dentro de un namespace llamado Parsers en nuestra webapi.

```C#
using System;
using Homeworks.WebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Homeworks.WebApi.Parsers
{
    public class UserModelJsonConverter : JsonCreationConverter<UserModel>
    {
        protected override UserModel Create(Type objectType, JObject jObject)
        {
            if (jObject == null)
            {
                throw new ArgumentNullException("jObject");
            }
            if (jObject["StudentId"] != null)
            {
                return new StudentModel();
            }
            if (jObject["TeacherId"] != null)
            {
                return new TeacherModel();
            }
            return new UserModel();
        }
    }
}
```

Ahora simplemente hacemos *override* de método **Create** y devolvemos el tipo correcto según el json que recibamos.
Entonces si nuestro json tiene:

* La property **StudentId**: sabemos que es un StudentModel;
* La property **TeacherId**: sabemos que es un TeacherModel;
* Y por ultimo si no tiene ninguna de estas supondremos que es un UserModel.

*Pregunta*: que pasaría si nuestro json tiene tanto la property **StudentId** como **TeacherId**? En este caso por como hicimos el método retornaría un StudentModel perdiendo la información de TeacherId.

Por ultimo nos falta indicarle a la webapi cuando usar el JsonConverter, para esto iremos a la clase UserModel y agregaremos la correspondiente Data Annotation:

```C# 
[JsonConverter(typeof(UserModelJsonConverter))]
```

```c#
// codigo...
namespace Homeworks.WebApi.Models
{
    [JsonConverter(typeof(UserModelJsonConverter))]
    public class UserModel : Model<User, UserModel>
    {
        // codigo...
    }
}
```

Ahora si realizamos las 3 request de postman del principio obtenemos:

```json
[
  "UserModel",
  {
    "userName": "admin",
    "name": "juan",
    "password": "admin",
    "isAdmin": true,
  }
]
```

```json
[
  "TeacherModel",
  {
    "userName": "admin",
    "name": "juan",
    "password": "admin",
    "isAdmin": true,
    "teacherId" "1",
  }
]
```

```json
[
  "StudentModel",
  {
    "userName": "admin",
    "name": "juan",
    "password": "admin",
    "isAdmin": true,
    "studentId" "2",
  }
]
```

Como debería ser :)

## Mas Info

* [WebApi DataBinding](https://www.tutorialdocs.com/article/webapi-data-binding.html)