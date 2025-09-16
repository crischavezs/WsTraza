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

namespace WsTraza.Controllers
{
    [ServiceFilter(typeof(AuthorizeAttribute))]
    [ApiController]
    [Route("[Controller]/[action]")]
    public class MaestrosController : Controller
    {
        #region "Log"
        private readonly ILogger<MaestrosController> _logger;

        public MaestrosController(ILogger<MaestrosController> logger)
        {
            _logger = logger;
        }
        #endregion
        // TIPO DE DOCUMENTO
        [HttpGet]
        public IActionResult GetTipodeDocumento()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetTiposIdentificacion();

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    // Deserializar la lista
                    var deserializedList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JSONresult1);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Datos enviados correctamente por correo",
                        response = deserializedList
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron datos",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de tipo de documento: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        // MEDICAMENTOS
        [HttpGet]
        public IActionResult GetMezclas()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetMezclas();

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);

                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Mezclas consultadas correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron mezclas",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de mezclas: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        // INSUMOS
        [HttpGet]
        public IActionResult GetProductos()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetProductos();

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);

                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Productos consultados correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron productos",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de productos: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        // EMPRESAS
        [HttpGet]
        public IActionResult GetEmpresa()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetEmpresas();

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Empresas consultadas correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron empresas",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de empresas: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        //SEDES
        [HttpGet]
        public IActionResult GetSedes([FromQuery] string empresa)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetSedes(empresa);
                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Sedes consultadas correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron sedes",
                        response = Array.Empty<object>()
                    });
                }
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");
                _logger.LogError("Se ha presentado un error en la consulta de sedes: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        // SERVICIOS
        [HttpGet]
        public IActionResult GetServicios([FromQuery] string empresa, [FromQuery] string sede)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetServicios(empresa, sede);

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Productos consultados correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron servicios",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de servicios: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        // SERVICIOS EXTERNOS
        [HttpGet]
        public IActionResult GetServiciosExt([FromQuery] string empresa, [FromQuery] string sede)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetServiciosExt(empresa, sede);

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Servicios Ext consultados correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron servicios externos",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de servicios externos: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        // VEHICULOS
        [HttpGet]
        public IActionResult GetVehiculos()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetVehiculo();

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    // Deserializar la lista
                    var deserializedList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JSONresult1);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Vehículos consultados correctamente",
                        response = deserializedList
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron vehículos",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de vehiculos: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        //COLORES
        [HttpGet]
        public IActionResult GetColores()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetColores();

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Colores consultados correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron colores",
                        response = Array.Empty<object>()
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de colores: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        //QFS
        [HttpGet]
        public IActionResult GetQFS()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetQFS();
                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "QFS consultados correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron QFS",
                        response = Array.Empty<object>()
                    });
                }
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");
                _logger.LogError("Se ha presentado un error en la consulta de QFS: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }

        }

        //TIPOS DE PREPARACION
        [HttpGet]
        public IActionResult GetTipoPreparacion()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetTipoPreparacion();
                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Tipos de preparacion consultados correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron Tipos de preparacion",
                        response = Array.Empty<object>()
                    });
                }
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");
                _logger.LogError("Se ha presentado un error en la consulta de Tipos de preparacion: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }

        }

        //ESTADOS
        [HttpGet]
        public IActionResult GetEstados(string modCod)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetEstados(modCod);
                string JSONresult1 = JsonConvert.SerializeObject(result);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = StatusCodes.Status200OK,
                    message = "Estados consultados correctamente",
                    response = JSONresult1
                });

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");
                _logger.LogError("Se ha presentado un error en la consulta de estados: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        //MOTIVOS DE DEVOLUCION
        [HttpGet]
        public IActionResult GetMotDev()
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetMotDev();

                string JSONresult1 = JsonConvert.SerializeObject(result);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = StatusCodes.Status200OK,
                    message = "motivos de devolucion consultados correctamente",
                    response = JSONresult1
                });
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");
                _logger.LogError("Se ha presentado un error en la consulta de motivos de devolucion: {error}", Mesaje);
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
