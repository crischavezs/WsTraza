using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using WsTraza.Class;
using WsTraza.Middleware;
using WsTraza.Models;

namespace WsTraza.Class
{
    public class Insertar
    {
        public DataManager gestorBaseDatos { get; set; }

        #region Produccion
        //INSERTAR TRAMAG
        public void InsertTraMagPro(InsertProduccion req)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DECLARE @NewCsc BIGINT;");
            sb.AppendLine("EXEC dbo.OCGN_Proce_TraMag_FullInsert");
            sb.AppendLine($"    @CodEmp    = '{req.Empresa}',");
            sb.AppendLine($"    @Sede      = '{req.Sede}',");
            sb.AppendLine($"    @Servicio  = '{req.Servicio}',");
            sb.AppendLine($"    @Ingreso   = {req.Ingreso},");
            sb.AppendLine($"    @Folio     = {req.Folio},");
            sb.AppendLine($"    @TipoIde   = '{req.TipoIde}',");
            sb.AppendLine($"    @Ide       = '{req.Ide}',");
            sb.AppendLine($"    @MSCodi    = '{req.MSCodi}',");
            sb.AppendLine($"    @MSPrAc    = '{req.MSPrAc}',");
            sb.AppendLine($"    @CncCd     = '{req.CncCd}',");
            sb.AppendLine($"    @MSForm    = '{req.MSForm}',");
            sb.AppendLine($"    @ProcEst   = '{req.ProcEst}',");
            sb.AppendLine($"    @ImpEst    = '{req.ImpEst}',");
            sb.AppendLine($"    @MotDev    = '{req.MotDev}',");
            sb.AppendLine($"    @FotoSen   = '{req.FotoSen}',");
            sb.AppendLine($"    @Color     = '{req.Color}',");
            sb.AppendLine($"    @VehRec    = '{req.VehRec}',");
            sb.AppendLine($"    @VolRec    = {req.VolRec},");
            sb.AppendLine($"    @ConRec    = {req.ConRec},");
            sb.AppendLine($"    @VehDil    = '{req.VehDil}',");
            sb.AppendLine($"    @VolDil    = {req.VolDil},");
            sb.AppendLine($"    @ConDil    = {req.ConDil},");
            sb.AppendLine($"    @Via       = {req.Via},");
            sb.AppendLine($"    @Flu       = '{req.Flu}',");
            sb.AppendLine($"    @LeyAlm    = '{req.LeyAlm}',");
            sb.AppendLine($"    @LeyAdm    = '{req.LeyAdm}',");
            sb.AppendLine($"    @NomPac    = '{req.NomPac}',");
            sb.AppendLine($"    @Hab       = '{req.Hab}',");
            sb.AppendLine($"    @DosPre    =  {req.DosPre.ToString(CultureInfo.InvariantCulture)},");
            sb.AppendLine($"    @Dosis24H  =  {req.Dosis24H},");
            sb.AppendLine($"    @Obs       = '{req.Obs}',");
            sb.AppendLine($"    @ObsProc       = '{req.ObsProc}',");
            sb.AppendLine($"    @Estado    = '{req.Estado}',");
            sb.AppendLine($"    @UsuAdd    = '{req.UsuAdd}',");
            sb.AppendLine($"    @FecAdd    = '{req.FecAdd}',");
            sb.AppendLine($"    @UsuMod    = '{(req.UsuMod ?? "")}',");
            sb.AppendLine($"    @FecMod    = {(string.IsNullOrEmpty(req.FecMod) ? "NULL" : $"'{req.FecMod}'")},");
            sb.AppendLine("    @CscOutput = @NewCsc OUTPUT;");
            sb.AppendLine("SELECT @NewCsc AS Csc;");

            var dt = gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
            if (dt.Rows.Count > 0 && dt.Rows[0]["Csc"] != DBNull.Value)
            {
                req.Csc = Convert.ToInt64(dt.Rows[0]["Csc"]);
                if (req.IdSol > 0 && req.CscSol > 0)
                {
                    var sc = new StringBuilder();
                    sc.AppendLine("EXEC dbo.OCGN_Proce_TraMagOrdExt_Update");
                    sc.AppendLine($"    @CodEmp    = '{req.Empresa}',");
                    sc.AppendLine($"    @Sede      = '{req.Sede}',");
                    sc.AppendLine($"    @Servicio  = '{req.Servicio}',");
                    sc.AppendLine($"    @CscTraMag   = {req.Csc},");
                    sc.AppendLine($"    @CscSol   = {req.CscSol},");
                    sc.AppendLine($"    @IdSol   = {req.IdSol},");
                    sc.AppendLine($"    @UsuMod   = {req.UsuAdd}");

                    var dd = gestorBaseDatos.ejecutarQuery_SQL(sc.ToString());
                }
            }
            else
            {
                throw new Exception("No se pudo generar el consecutivo ni completar la inserción.");
            }
        }

        #endregion

        #region Solicitudes Externas
        //CSC DE SOLICITUDES EXTERNAS
        public long UpdateCscSolExt(InsertSolicitudExt req)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DECLARE @Csc BIGINT;");
            sb.AppendLine("EXEC dbo.OCGN_Proce_TraMag_Csc_SolExt");
            sb.AppendLine($" @Empresa  = '{req.Empresa}',");
            sb.AppendLine($" @Sede     = '{req.Sede}',");
            sb.AppendLine($" @Servicio = {req.Servicio},");
            sb.AppendLine($" @UsuAdd   = '{req.UsuAdd}',");
            sb.AppendLine(" @Csc      = @Csc OUTPUT;");
            sb.AppendLine("SELECT @Csc AS Csc;");

            var dt = gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
            if (dt.Rows.Count == 0) return 0;

            return (long)dt.Rows[0]["Csc"];

        }

        //INSERTAR SOLICITUD EXT
        public void InsertTraMagSolExt(InsertSolicitudExt req)
        {
            long csc = UpdateCscSolExt(req);

            var comandos = req.Solicitudes.Select(it =>
                "EXEC dbo.OCGN_Proce_TraMagOrdExt_Insert " +
                $"@CscSol={csc}, " +
                $"@Id={it.Id}, " +
                $"@CodEmp='{req.Empresa}', " +
                $"@Sede='{req.Sede}', " +
                $"@Servicio={req.Servicio}, " +
                $"@NomServicio='{it.NomServicio}', " +
                $"@MSCodi='{it.MSCodi}', " +
                $"@MSPrAc='{it.MSPrAc}', " +
                $"@CncCd='{it.CncCd}', " +
                $"@MSForm='{it.MSForm}', " +
                $"@DosisPrescrita={it.DosisPrescrita.ToString(CultureInfo.InvariantCulture)}, " +
                $"@Unidad='{it.Unidad}', " +
                $"@DosisMinPresentacion={it.DosisMinPresentacion.ToString(CultureInfo.InvariantCulture)}, " +
                $"@Cantidad={it.Cantidad}, " +
                $"@Obs='{it.Obs}', " +
                $"@UsuAdd='{req.UsuAdd}'"
            ).ToList();


            // Ejecuta TODOS en una sola transacción
            gestorBaseDatos.ejecutarTransaccion_SQL(comandos);

        }
        #endregion

        #region Alistamiento
        //CSC DE Alistamiento
        public long UpdateCscAli(Alistamiento req)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DECLARE @Consec BIGINT;");
            sb.AppendLine("EXEC dbo.OCGN_Proce_TraMagFueAli_Csc");
            sb.AppendLine($" @TipoPrep  = '{req.prepCod}',");
            sb.AppendLine($" @UsuAdd   = '{req.usuAdd}',");
            sb.AppendLine(" @Csc      = @Consec OUTPUT;");
            sb.AppendLine("SELECT @Consec AS NuevoConsecutivo;");

            var dt = gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
            if (dt.Rows.Count == 0) return 0;

            return (long)dt.Rows[0]["Csc"];
        }


        //INSERTAR ENTRADA
        public void InsertTraMagAli(Alistamiento req)
        {
            long csc = UpdateCscAli(req);

            var queries = new List<string>();

            foreach (var it in req.Entradas)
            {
                // 1) LOG
                queries.Add($@"
                EXEC dbo.OCGN_Proce_TraMagAli_Insert 
                  @Csc      = {csc},
                  @Id       = {it.id},
                  @PrepCod  = '{req.prepCod}',
                  @CodProd  = '{it.codProd}',
                  @Nombre   = '{it.nombre}',
                  @Lote     = '{it.lote}',
                  @FechVenc = '{it.fechVenc}',
                  @RegInvima= '{it.regInvima}',
                  @CantSol  = {it.cantSol},
                  @UsuAdd   = '{req.usuAdd}'");

                // 2) STOCK (upsert)
                queries.Add($@"
                EXEC dbo.OCGN_Proce_TraMagAli_Upsert
                  @PrepCod   = '{req.prepCod}',
                  @CodProd   = '{it.codProd}',
                  @Nombre    = '{it.nombre}',
                  @Lote      = '{it.lote}',
                  @FechVenc  = '{it.fechVenc}',
                  @RegInvima = '{it.regInvima}',
                  @CantSol   = {it.cantSol},
                  @UsuAdd    = '{req.usuAdd}'");
            }

            gestorBaseDatos.ejecutarTransaccion_SQL(queries);
        }
    }
    #endregion
}

