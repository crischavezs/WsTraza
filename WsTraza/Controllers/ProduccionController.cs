using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json;
using System.Data;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Text;
using WsTraza.Class;
using WsTraza.Middleware;
using WsTraza.Models;

namespace WsTraza.Controllers
{
    [ServiceFilter(typeof(AuthorizeAttribute))]
    [ApiController]
    [Route("[Controller]/[action]")]
    public class ProduccionController : Controller
    {
        #region "Log"
        private readonly ILogger<ProduccionController> _logger;

        public ProduccionController(ILogger<ProduccionController> logger)

        {
            _logger = logger;

        }
        #endregion

        //Insertar a tabla principal nuevo metodo 
        [HttpPost]
        public IActionResult InsertTraMagPro([FromBody] InsertProduccion req)
        {
            try
            {
                var dao = new Insertar();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");

                dao.InsertTraMagPro(req);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = 200,
                    message = "Insertado correctamente",
                    Consecutivo = req.Csc
                });
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la Insertar la orden: {error}", Mesaje);

                return StatusCode(500, new
                {
                    status = 500,
                    message = "Error al insertar",
                    error = Mesaje,
                    trace = ex.StackTrace
                });
            }
        }

        // Obtener ordenes del día por empresa, sede y servicio
        [HttpGet]
        public IActionResult GetOrdenes([FromQuery] string empresa, [FromQuery] string sede, [FromQuery] string servicio)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetOrdenes(empresa, sede, servicio);

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Ordenes consultadas correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron ordenes",
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

        // Obtener infromacion de TraMag1 por empresa, sede y csc
        [HttpGet]
        public IActionResult GetTraMag1([FromQuery] string empresa, [FromQuery] string sede, [FromQuery] long csc)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetTraMag1(empresa, sede, csc);

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Ordenes consultadas correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron ordenes",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de GetTraMag1: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        //Atualizar TraMagPro
        [HttpPost]
        public IActionResult UpdateTraMagPro([FromBody] InsertProduccion req)
        {
            try
            {
                var dao = new Actualizar();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");

                dao.UpdateTraMagPro(req);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = 200,
                    message = "Insertado correctamente",
                    Consecutivo = req.Csc
                });
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la actualizacíon de la orden: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        //Cancelar TraMagPro
        [HttpPost]
        public IActionResult CancelTraMagPro([FromBody] CancelarProduccion req)
        {
            try
            {
                var dao = new Cancelar();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");

                dao.CancelarTraMagPro(req);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = 200,
                    message = "Cancelado correctamente",
                    Consecutivo = req.Csc
                });
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la cancelacion de la orden: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        //Imprimir TraMag1
        [HttpPost]
        public IActionResult GetTraMag1Eti([FromBody] List<TraMag1Consulta> requests)
        {
            try
            {
                var dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");

                var aggregate = new List<Dictionary<string, object>>();

                foreach (var req in requests)
                {
                    DataTable result = dao.GetTraMag1Eti(req.Empresa, req.Sede, req.Csc);

                    if (result.Rows.Count > 0)
                    {
                        var rows = result.AsEnumerable()
                            .Select(row => result.Columns.Cast<DataColumn>()
                                .ToDictionary(col => col.ColumnName, col => row[col.ColumnName]))
                            .ToList();

                        aggregate.AddRange(rows); 
                    }
                }

                // Serializamos una sola vez todo el conjunto
                string jsonFinal = JsonConvert.SerializeObject(aggregate);

                return Ok(new
                {
                    status = 200,
                    message = "Consultas procesadas correctamente",
                    response = jsonFinal
                });
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de informacion de etiquetas: {error}", Mesaje);
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
