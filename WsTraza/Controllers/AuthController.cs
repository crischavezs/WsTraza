using Microsoft.AspNetCore.Mvc;
using WsTraza.Class;

namespace WsTraza.Controllers
{
    public class AuthController : Controller
    {
        private readonly Consultas _dao;
        public AuthController(Consultas dao) => _dao = dao;

        // GET api/auth/getusuario?usuario=...&empresa=...&sede=...
        [HttpGet("GetUsuario")]
        public IActionResult GetUsuario(
            [FromQuery] string usuario,
            [FromQuery] string empresa,
            [FromQuery] string sede)
        {
            // Simulación: devolvemos de inmediato lo que llega
            return Ok(new
            {
                status = "0",
                message = "Simulación OK",
                usuario,
                empresa,
                sede,
                token = "simulado-token-123"
            });
        }
    }
}
