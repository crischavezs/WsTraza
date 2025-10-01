using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WsTraza.Class;
using WsTraza.Middleware;
using WsTraza.Models;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace WsTraza.Controllers
{
    [ServiceFilter(typeof(AuthorizeAttribute))]
    [ApiController]
    [Route("[Controller]/[action]")]
    public class ReportesController : Controller
    {
        #region "Log"
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(ILogger<ReportesController> logger)
        {
            _logger = logger;
        }
        #endregion

        [HttpGet]
        public IActionResult GetOrdDeProd1(string empresa, string sede, string servicio, string fecha)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetOrdDeProd1(empresa, sede, servicio, fecha);

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Orden de produccion buscada correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron ordenes de produccion",
                        response = 0
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de Ordenes de Produccion: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        [HttpGet]
        public IActionResult GetOrdDeProdExt1(string empresa, string sede, string servicio, string fecha)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable result = dao.GetOrdDeProdExt1(empresa, sede, servicio, fecha);

                if (result.Rows.Count > 0)
                {
                    string JSONresult1 = JsonConvert.SerializeObject(result);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Orden de produccion de solicitud externa buscada correctamente",
                        response = JSONresult1
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraron ordenes de produccion de solicitud externa",
                        response = 0
                    });
                }

            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de Ordenes de Produccion de solicitud externa: {error}", Mesaje);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = Mesaje,
                    response = Array.Empty<object>()
                });
            }
        }

        [HttpGet]
        public IActionResult GetInfoReporteOrdProd(string empresa, string sede, string servicio, string codapli, string fecha, string tipoOrden)
        {
            try
            {
                Consultas dao = new Consultas();
                dao.gestorBaseDatos = new DataManager("CONEX_SQL_basges_Repli");
                DataTable Logo = dao.GetLogoEmp(empresa); //Logo de la empresa

                DataTable InfoVersion = dao.GetInfoVersion(empresa, sede, codapli); //Info de la version

                DataTable InfoServicio = dao.GetNomServicio(empresa, sede, servicio); // Nombre del servicio
                var medicamentos = new List<object>();
                if (tipoOrden == "OrdProd")
                {
                    DataTable orden = dao.GetOrdDeProd1(empresa, sede, servicio, fecha);

                    //objeto de medicamentos
                    
                    foreach (DataRow row in orden.Rows)
                    {
                        var medicamentoObj = new
                        {
                            medicamento = row["Medicamento"].ToString(),
                            dosis = row["Cantidad"].ToString(),
                            frecuencia = row["Frecuencia"].ToString(),
                            dosisUnidad = row["Dosis"].ToString(),
                            päciente = row["NomPaciente"].ToString(),
                            habitacion = row["Habitacion"].ToString(),
                            volumen = "",
                            dispositivo = "",
                            interaccion = "",
                            observacion = "",
                            colorC = "",
                            colorCN = "",
                            particulasC = "",
                            particulasCN = "",
                            integridadC = "",
                            integridadCN = "",
                            etiquetaC = "",
                            etiquetaCN = "",
                        };
                        medicamentos.Add(medicamentoObj);
                    }
                }
                else
                {
                    DataTable orden = dao.GetOrdDeProdExt1(empresa, sede, servicio, fecha);
                }

                if (Logo.Rows.Count == 0 || InfoVersion.Rows.Count == 0 || InfoServicio.Rows.Count == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "No se encontraro Informacion del formato",
                        response = 0
                    });
                }

               
                
                var infoList = new List<object>();
                    
                    infoList.Add(new
                    {
                        logo = Logo.Rows[0]["Url"],
                        version = InfoVersion.Rows[0]["Version"].ToString(),
                        fechVersion = InfoVersion.Rows[0]["Fecha"].ToString(),
                        servcio = InfoServicio.Rows[0]["Nombre"].ToString(),
                        FirmaRegente = "",
                        firmaAprueba = "",
                        firmaPreparado = "",
                        firmaCondicionado = "",
                        firmaVerificado = "",
                        datosOrdenees = new
                        {
                            medicamento = medicamentos
                        }
                    }
                    );

                    string JSONresult1 = JsonConvert.SerializeObject(infoList);
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = StatusCodes.Status200OK,
                        message = "Informacion del formato obtenida correctamente",
                        response = JSONresult1
                    });
 
            }
            catch (Exception ex)
            {
                int NumError = ex.HResult;
                Auxiliar msgerror = new Auxiliar();
                string Mesaje = msgerror.mensajeError(ex, "W").ToString().Replace("'", "");

                _logger.LogError("Se ha presentado un error en la consulta de Ordenes de Produccion de solicitud externa: {error}", Mesaje);
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

