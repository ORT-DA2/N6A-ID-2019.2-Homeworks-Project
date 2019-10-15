# Clase 4 - Filters

Los filtros en ASP.NET Core permiten ejecutar código antes o después de etapas específicas en el pipeline de una request.

## Algunos filtros ya construidos

* Authorization (prevenir acceso a una ruta la cual el usuario no está autorizado)
* Asegurarce que todas las request usen HTTPS
* Response caching (shot-circuiting de la request pipeline para retornar una respuesta cachada)

Filters pueden ser creados para manejar 'preocupaciones' transversales. 
Ej: Manejo de excepciones la pueden realizar los filtros, si un método lanza una excepción es atrapado por un filtro y este retorna un 404, entonces con los filtros consolidamos el manejo de este error.

## Como funcionan los filtros

Filters corren entre la MVC Action invocation pipeline o fileter pipeliine. La filter pipeline corre después de que la API 
selecciona una acción que ejecutar.

![FILTERS-PIPELINE](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters/_static/filter-pipeline-1.png?view=aspnetcore-2.1)

## Tipos de filtros:
Tipo | Descripción
------------ | -------------
[Authorization filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#authorization-filters)| Se ejecutan primero y son usados para determinar si el usuario actual es autorizado para acceder al recurso actual.
[Resource filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#resource-filters)| Se ejecutan luego de la autorización. Y sirven para ejecutar código antes y después de que el pipeline termine. Son útiles para caching o shot-circuit la filter pipeline y así mejorar la performance.
[Action filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#action-filters)| Sirven para ejecutar código antes y después de una acción (método) es invocado. Son útiles para manipular argumentos pasados en la acción en particular.
[Exception filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#exception-filters)| Son usados para aplicar 'políticas' globales para manejar excepciones ocurridas antes de que cualquier cosa sea escrita en el body de la response.
[Result filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#result-filters)| Se ejecutan antes y después de la ejecución de un action results. Solo se ejecutan cuando un action method ha sido ejecutado exitosamente. Son útiles para crear formateadores.

![FILTERS-PIPELINE2](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters/_static/filter-pipeline-2.png?view=aspnetcore-2.1)

## Implementación de un Filtro

Vamos a crear ActionFilter para manejar la autehtificacion a nuestra api.
Esto no está bien ya que se debería de encargar un [Authorization filters](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/index?view=aspnetcore-2.1), para este ejemplo no lo utilizaremos ya que todo lo relacionado a la authorizacion de usuarios ya se encuentra todo muy digerido e implica otros temas como la generación de tokens con jwt, que prefiero mostrar un poco como funciona un authorization filter por detrás y simplificar los tokens, pero son bienvenidos a usarlo para el obl :smile:

## Creación de SessionLogic

Esta clase se encargará de hacer ABMs de tokens (guids) que usaremos para identificar que usuario está realizando la request.

```c#
public class SessionLogic : ISessionLogic
{
    private IRepository<User> repository;

    // TENDRÍA QUE SER UN SESSION REPOSITORY
    // SESSION = {
    //      token: Guid,  
    //      user: User
    // }
    // CUIDADO CON LAS VARIABLES ESTÁTICAS EN LA BUISSNESSLOGIC
    private static IDictionary<string, Guid?> TokenRepository = null;

    public SessionLogic(IRepository<User> repository) {
        this.repository = repository;
        if (TokenRepository == null) {
            TokenRepository = new Dictionary<string, Guid?>();
        }
    }

    public bool IsValidToken(string token)
    {
        // SI EL TOKEN EXISTE EN BD RETORNA TRUE
        return TokenRepository.ContainsKey(token);
    }

    public Guid? CreateToken(string userName, string password)
    {
        // SI EL USUARIO EXISTE Y LA PASS Y EL USERNAME SON EL MISMO
        // RETORNAR GUID
        var users = repository.GetAll();
        var user = users.FirstOrDefault(x => x.UserName == userName && x.Password == password);
        if (user == null)
        {
            return null;
        }
        var token = Guid.NewGuid();
        TokenRepository.Add(token.ToString(), user.Id);
        return token;
    }

    public bool HasLevel(string token, string role)
    {  
        var user = GetUser(token);
        if (user == null) {
            return false;
        }
        return user.IsAdmin;
    }

    public User GetUser(string token)
    {
        Guid? userId = null;
        if (TokenRepository.TryGetValue(token, out userId))
        {
            return repository.Get(userId.GetValueOrDefault());
        }
        return null;
    }

}
```

## Login de usuario

Agregaremos un controller que se encarga de hacer el login de usuarios. 
Este tiene un post que si el username y la password nos genere un token.

```c#
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private ISessionLogic sessions;

    public TokenController(ISessionLogic sessions) : base()
    {
        this.sessions = sessions;
    }

    [HttpPost]
    public IActionResult Login([FromBody]LoginModel model) {
        var token = sessions.CreateToken(model.UserName, model.Password);
        if (token == null) 
        {
            return BadRequest("Invalid user/password");
        }
        return Ok(token);
    }

    [ProtectFilter("Admin")]
    [HttpGet("Check")]
    public IActionResult CheckLogin() {
        return Ok(new UserModel(sessions.GetUser(Request.Headers["Authorization"])));
    }

}
```

## Creación del Filtro

Nuestro ActionFilter va a implementar la interfaz **IActionFilter** que tiene los siguientes métodos **OnActionExecuting** (Se ejecuta antes del action method y **OnActionExecuted** (Se ejecuta después del action method), y también va a heredar de **Attribute** que nos permitirá usarlo como **tag** en C#

El constructor va a recibir el role del usuario que tiene permitido ejecutar el action mehtod. Y solo implementaremos **OnActionExecuting** ya que solo nos interesa controlar si impedir o permitir el acceso al action method antes de que se ejecute.

```c#
public class ProtectFilter : Attribute, IActionFilter
{
    private readonly string _role;

    public ProtectFilter(string role) 
    {
        _role = role;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        string token = context.HttpContext.Request.Headers["Authorization"];
        if (token == null)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 400,
                Content = "Token is required",
            };
            return;
        }
        var sessions = GetSessions(context);
        if (!sessions.IsValidToken(token))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 400,
                Content = "Invalid Token",
            };
            return;
        }
        if (!sessions.HasLevel(token, _role))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 400,
                Content = "The user isen't " + _role,
            };
            return;
        }
    }

    private static ISessionLogic GetSessions(ActionExecutingContext context)
    {
        return (ISessionLogic)context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // do something after the action executes
    }
}
```

## Uso del filtro

Para usar el filtro simplemente debemos agregar el nombre del filtro como atributo.
Encima de un metodo si quien que se ejecute cuando ese metodo es invocado.

```c#
[ProtectFilter("Admin")]
[HttpGet("Check")]
public IActionResult CheckLogin() {
    return Ok(new UserModel(sessions.GetUser(Request.Headers["Authorization"])));
}
```

O encima de un controller si quieren que se ejecute para cada uno de los metodos de este.

```c#
[ProtectFilter("Admin")]
[Route("api/[controller]")]
public class HomeworksController : BaseController
{
    //...
}
```

# Por que usar Tokens (JWT)

La forma preferida hoy en día para autenticarse desde el front-end ya sea web o mobile es la de tokens por las siguientes razones:

**Escalabilidad de servidores**: El token que se envía al servidor es independiente, contiene toda la información necesaria para la autenticación del usuario, por lo que añadir más servidores a la granja es una tarea fácil ya que no depende de una sesión compartida.

**Bajo acoplamiento**: Su aplicación front-end no se acopla con el mecanismo de autenticación específico, el token se genera desde el servidor y su API se construye de una manera que se pueda entender y hacer la autenticación.

**Móvil amigable**: Al tener una forma estándar para autenticar a los usuarios va a simplificar nuestra vida si decidimos consumir la API de servicios de fondo desde aplicaciones nativas como IOS, Android y Windows Phone.

# Mas Info

* [Filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1)
* [Authorization in Core](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/index?view=aspnetcore-2.1)
* [JWT Framework](http://enmilocalfunciona.io/construyendo-una-web-api-rest-segura-con-json-web-token-en-net-parte-i/)
* [JWT Framework](http://codigoenpuntonet.blogspot.com/2016/09/inicio-de-sesion-basado-en-tokens-con.html)
