using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WsTraza.Class;
using WsTraza.Middleware;
using WsTraza.Models;

namespace WsTraza.Controllers;

[ServiceFilter(typeof(AuthorizeAttribute))]
[ApiController]
[Route("[Controller]/[action]")]
public class SolicitudesExtController : Controller
{
    #region "Log"
    private readonly ILogger<SolicitudesExtController> _logger;

    public SolicitudesExtController(ILogger<SolicitudesExtController> logger)
    {
        _logger = logger;
    }
    #endregion

    [HttpPost]
    public IActionResult InsertTraMagSolExt([FromBody] InsertSolicitudExt req)
    {
        try
        {
            var dao = new Insertar();
            dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");

            dao.InsertTraMagSolExt(req);

            return StatusCode(StatusCodes.Status200OK, new
            {
                status = 200,
                message = "Insertado correctamente",
                response=" "
            });
        }
        catch (Exception ex)
        {
            int NumError = ex.HResult;
            Auxiliar msgerror = new Auxiliar();
            string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

            _logger.LogError("Se ha presentado un error en la insercion de las solicitudes externas: {error}", Mesaje);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                status = StatusCodes.Status500InternalServerError,
                message = Mesaje,
                response = Array.Empty<object>()
            });
        }
    }

    [HttpGet]
    public IActionResult GetOrdenesExt([FromQuery] string empresa, [FromQuery] string sede, [FromQuery] string servicio)
    {
        try
        {
            Consultas dao = new Consultas();
            dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
            DataTable result = dao.GetOrdenesExt(empresa, sede, servicio);

            if (result.Rows.Count > 0)
            {
                string JSONresult1 = JsonConvert.SerializeObject(result);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = StatusCodes.Status200OK, 
                    message = "Ordenes Externas consultadas correctamente",
                    response = JSONresult1
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = StatusCodes.Status200OK,
                    message = "No se encontraron ordenes externas",
                    response = 0
                });
            }

        }
        catch (Exception ex)
        {
            int NumError = ex.HResult;
            Auxiliar msgerror = new Auxiliar();
            string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

            _logger.LogError("Se ha presentado un error en la consulta de Ordenes Externas: {error}", Mesaje);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                status = StatusCodes.Status500InternalServerError,
                message = Mesaje,
                response = Array.Empty<object>()
            });
        }
    }

    [HttpPost]
    public IActionResult CancelTraMagSolExt([FromBody] CancelarSolicitudExt req)
    {
        try
        {
            var dao = new Cancelar();
            dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");

            dao.CancelTraMagSolExt(req);

            return StatusCode(StatusCodes.Status200OK, new
            {
                status = 200,
                message = "Cancelado correctamente",
                response = " "
            });
        }
        catch (Exception ex)
        {
            int NumError = ex.HResult;
            Auxiliar msgerror = new Auxiliar();
            string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");
            // Guarda en los logs el mensaje + stacktrace
            _logger.LogError(ex,
           "Error en InsertTraMagSolExt: {Message}\nStackTrace: {StackTrace}",
           ex.Message, ex.StackTrace);

            return StatusCode(500, new
            {
                status = 500,
                message = "Error al cancelar",
                error = Mesaje,
                trace = ex.StackTrace
            });
        }
    }

}
