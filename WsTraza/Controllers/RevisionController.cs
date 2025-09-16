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
    public class RevisionController : Controller
    {
        #region "Log"
        private readonly ILogger<RevisionController> _logger;

        public RevisionController(ILogger<RevisionController> logger)

        {
            _logger = logger;

        }
        #endregion

        [HttpGet]
        public IActionResult GetTraMagRev(string empresa, string sede, string servicio)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetTraMagRev(empresa, sede, servicio);

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Ordenes para revisar consultadas correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron ordenes para revisar",
                        response = 0
                    });
                }
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de Ordenes: {error}", Mesaje);

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