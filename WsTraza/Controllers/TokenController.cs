using Microsoft.AspNetCore.Mvc;
using System.Data;
using WsTraza.Class;
using WsTraza.Middleware;

namespace WsTraza.Controllers;

[ApiController]
[Route("[Controller]/[action]")]
public class TokenController : Controller
{
    #region "Log"
    private readonly ILogger<TokenController> _logger;

    public TokenController(ILogger<TokenController> logger)
    {
        _logger = logger;
    }
    #endregion

    [HttpGet]
    public IActionResult GetToken(string usuario, string clave)
    {
        try
        {
            if (string.IsNullOrEmpty(usuario))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "usuario es requerido",
                    Response = Array.Empty<object>()
                });
            }
            if (string.IsNullOrEmpty(clave))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "clave es requerido",
                    Response = Array.Empty<object>()
                });
            }

            Consultas dao = new Consultas();
            dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
            DataTable result = dao.consultar_usuario_token(usuario, clave, "WsTraza");

            if (result.Rows.Count > 0)
            {
                var token = JwtManager.GenerateToken(
              username: result.Rows[0]["Usuario"].ToString(),
              key: result.Rows[0]["AplKey"].ToString()!,
              tiempo: Convert.ToInt32(result.Rows[0]["Duracion"].ToString())
          );

                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = StatusCodes.Status200OK,
                    message = "Token generado correctamente",
                    response = token
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = StatusCodes.Status401Unauthorized,
                    message = "Usuario o Contraseña incorrectos.",
                    response = Array.Empty<object>()
                });
            }


        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                status = StatusCodes.Status500InternalServerError,
                message = $"Ocurrió un error en el servidor: {ex.Message}"
            });
        }
    }
}
