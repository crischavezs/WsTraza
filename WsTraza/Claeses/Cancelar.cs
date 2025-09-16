using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using WsTraza.Models;

namespace WsTraza.Class

{
    public class Cancelar
    {
        public DataManager gestorBaseDatos { get; set; }
        //CANCELAR PRODUCCION
        public void CancelarTraMagPro(CancelarProduccion req) {
            var queries = new List<string>();
            queries.Add($@"
                EXEC dbo.OCGN_Proce_TraMagLog1_Cancel
                @Csc      = {req.Csc},
                @CodEmp   = '{req.Empresa}',
                @Sede     = '{req.Sede}',
                @UsuMod   = '{req.UsuMod}'
            ");

            queries.Add($@"
                EXEC dbo.OCGN_Proce_TraMag1_Cancel
                @Csc      = {req.Csc},
                @CodEmp   = '{req.Empresa}',
                @Sede     = '{req.Sede}',
                @UsuMod   = '{req.UsuMod}'
            ");

            queries.Add($@"
                EXEC dbo.OCGN_Proce_TraMag_Cancel
                @Csc      = {req.Csc},
                @CodEmp   = '{req.Empresa}',
                @Sede     = '{req.Sede}',
                @UsuMod   = '{req.UsuMod}'
            ");
            gestorBaseDatos.ejecutarTransaccion_SQL(queries);

        }

        //CANCELAR SOLICITUD EXT
        public void CancelTraMagSolExt(CancelarSolicitudExt req)
        {          
            var comandos = req.Solicitudes.Select(it =>
                "EXEC dbo.OCGN_Proce_TraMagOrdExt_Cancel " +
                $"@CscSol={it.CscSol}, " +
                $"@Id={it.Id}, " +
                $"@CodEmp='{req.Empresa}', " +
                $"@Sede='{req.Sede}', " +
                $"@Servicio={req.Servicio}, " +
                $"@UsuMod='{req.UsuMod}'"
            ).ToList();

            // Ejecuta TODOS en una sola transacción
            gestorBaseDatos.ejecutarTransaccion_SQL(comandos);

        }

        //CANCELAR ALISTAMIENTO
         public void CancelarTraMagAli(CancelarAlistamiento req)
        {
            var comandos = req.Ent.Select(it =>
                $"EXEC dbo.OCGN_Proce_TraMagAli_Cancelar " +
                $"@Csc={it.csc}, " +
                $"@Id={it.id}, " +
                $"@PrepCod={req.prepCod}, " +
                $"@UsuMod='{req.usuMod}'"
            ).ToList();

            // Ejecuta TODOS en una sola transacción
            gestorBaseDatos.ejecutarTransaccion_SQL(comandos);

        }
    }
}
