using Microsoft.AspNetCore.Mvc;
using WsTraza.Class;
using WsTraza.Middleware;
using WsTraza.Models;
using Newtonsoft.Json;
using System.Data;


namespace WsTraza.Controllers
{
    [ServiceFilter(typeof(AuthorizeAttribute))]
    [ApiController]
    [Route("[Controller]/[action]")]
    public class AlistamientoController : Controller
    {
        #region "Log"
        private readonly ILogger<AlistamientoController> _logger;

        public AlistamientoController(ILogger<AlistamientoController> logger)

        {
            _logger = logger;

        }
        #endregion

        [HttpPost]
        public IActionResult InsertAlistamiento([FromBody] Alistamiento req)
        {
            try
            {
                var dao = new Insertar();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");

                dao.InsertTraMagAli(req);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = 200,
                    message = "Insertado correctamente",
                    response = " "
                });
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la insercion de las entradas: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        [HttpGet]
        public IActionResult GetTraMagAliLog(string prepcod)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetTraMagAliLog(prepcod);
                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Alistamiento consultado correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron registros de alistamiento",
                        response = 0
                    });
                }
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");
                _logger.LogError("Se ha presentado un error en la consulta del alistamiento: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        [HttpPost]
        public IActionResult CancelarAlistamiento([FromBody] CancelarAlistamiento req)
        {
            try
            {
                var dao = new Cancelar();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                dao.CancelarTraMagAli(req);


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

                _logger.LogError("Se ha presentado un error en la cancelacion de las entradas: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }
    }
}
