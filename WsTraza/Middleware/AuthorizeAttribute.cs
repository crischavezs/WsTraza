using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WsTraza.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public AuthorizeAttribute(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            context.Result = new JsonResult(new { status= StatusCodes.Status401Unauthorized, message = "Token de autorización no proporcionado.", response = Array.Empty<object>() })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

        try
        {
            var key = _configuration["Jwt:Key"];
            var principal = JwtManager.ValidateToken(token, key);

            // Guardar el usuario en el contexto para que pueda ser accedido por el controlador
            context.HttpContext.Items["UserToken"] = principal;
        }
        catch (Exception ex)
        {
            context.Result = new JsonResult(new { status = 401, message = "Error al validar el token: " + ex.Message })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}