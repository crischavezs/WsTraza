using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using WsTraza.Class;
using WsTraza.Middleware;
using WsTraza.Models;

namespace WsTraza.Class
{
    public class Actualizar
    {
        public DataManager gestorBaseDatos { get; set; }

        public void UpdateTraMagPro(InsertProduccion req)
        {
            var queries = new List<string>();
            queries.Add($@"
                 EXEC dbo.OCGN_Proce_TraMagLog1_Update
                @Csc      = {req.Csc},
                @CodEmp   = '{req.Empresa}',
                @Sede     = '{req.Sede}',
                @ProcEst  = '{req.ProcEst}',
                @MotDev   = '{req.MotDev}',
                @FotoSen  = '{req.FotoSen}',
                @Color    = '{req.Color}',
                @VehRec   = '{req.VehRec}',
                @VolRec   = {req.VolRec},
                @ConRec   = {req.ConRec},
                @VehDil   = '{req.VehDil}',
                @VolDil   = {req.VolDil},
                @ConDil   = {req.ConDil},
                @Flu      = '{req.Flu}',
                @LeyAlm   = '{req.LeyAlm}',
                @LeyAdm   = '{req.LeyAdm}',
                @Obs      = '{req.Obs}',
                @Estado   = '{req.Estado}',
                @UsuAdd   = '{req.UsuAdd}',
                @FecAdd   = '{req.FecAdd}',
                @UsuMod   = '{(req.UsuMod ?? "")}',
                @FecMod   = {(string.IsNullOrEmpty(req.FecMod) ? "NULL" : $"'{req.FecMod}'")}
        ");

            queries.Add($@"
                 EXEC dbo.OCGN_Proce_TraMag1_Update
                @Csc      = {req.Csc},
                @CodEmp   = '{req.Empresa}',
                @Sede     = '{req.Sede}',
                @ProcEst  = '{req.ProcEst}',
                @MotDev   = '{req.MotDev}',
                @FotoSen  = '{req.FotoSen}',
                @Color    = '{req.Color}',
                @VehRec   = '{req.VehRec}',
                @VolRec   = {req.VolRec},
                @ConRec   = {req.ConRec},
                @VehDil   = '{req.VehDil}',
                @VolDil   = {req.VolDil},
                @ConDil   = {req.ConDil},
                @Flu      = '{req.Flu}',
                @LeyAlm   = '{req.LeyAlm}',
                @LeyAdm   = '{req.LeyAdm}',
                @Obs      = '{req.Obs}',
                @Estado   = '{req.Estado}',
                @UsuMod   = '{req.UsuAdd}',
                @FecMod   = '{req.FecAdd}'
        ");

            gestorBaseDatos.ejecutarTransaccion_SQL(queries);
        }

       
    }
}
