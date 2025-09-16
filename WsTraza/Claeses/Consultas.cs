using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace WsTraza.Class;

public class Consultas
{
    public DataManager gestorBaseDatos { get; set; }

    #region "Token"
    public DataTable consultar_usuario_token(string usuario, string clave, string CodApl)
    {
        var query = new StringBuilder();
        query.AppendLine("DECLARE ");
        query.AppendLine("@CodApl  VARCHAR(10)='" + CodApl + "',");
        query.AppendLine("@Estado char(1)='0',");
        query.AppendLine("@usuario VARCHAR(20)='" + usuario + "',");
        query.AppendLine("@clave VARCHAR(20)='" + clave + "'");
        query.AppendLine("EXEC OCGN_proce_sw_user");
        query.AppendLine("@CodApl,");
        query.AppendLine("@Estado,");
        query.AppendLine("@usuario,");
        query.AppendLine("@clave");
        return gestorBaseDatos.ejecutarQuery_SQL(query.ToString());
    }

    #endregion

    #region Maestros
    //TIPOS DE DOCUMENTO
    public DataTable GetTiposIdentificacion()
    {
        StringBuilder Querys = new StringBuilder();
        Querys.AppendLine("EXEC OCGN_Proce_Tipo_Identificacion");
        return gestorBaseDatos.ejecutarQuery_SQL(Querys.ToString());

    }

    //MEZCLAS
    public DataTable GetMezclas()
    {
        StringBuilder Querys = new StringBuilder();
        Querys.AppendLine(" EXEC ocgn_proce_Traza_maemezcla_select");
        return gestorBaseDatos.ejecutarQuery_SQL(Querys.ToString());
    }

    //INSUMOS
    public DataTable GetProductos()
    {
        StringBuilder Querys = new StringBuilder();
        Querys.AppendLine("Ocgn_Proce_Traza_Productos_select");
        return gestorBaseDatos.ejecutarQuery_SQL(Querys.ToString());
    }

    //EMPRESAS
    public DataTable GetEmpresas()
    {
        StringBuilder Querys = new StringBuilder();
        Querys.AppendLine("EXEC OCGN_Proce_Traza_empresa_select");
        return gestorBaseDatos.ejecutarQuery_SQL(Querys.ToString());
    }

    //SEDES
    public DataTable GetSedes(string empresa)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_Traza_sedes_select");
        sb.AppendLine($" @Empresa = '{empresa}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //SERVICIOS
    public DataTable GetServicios(string empresa, string sede)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_Traza_servicio_select");
        sb.AppendLine($" @Empresa = '{empresa}',");
        sb.AppendLine($" @Sede    = '{sede}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //SERVICIOS EXTERNOS
    public DataTable GetServiciosExt(string empresa, string sede)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_ServiciosExt");
        sb.AppendLine($" @Empresa = '{empresa}',");
        sb.AppendLine($" @Sede    = '{sede}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //ORDENAR POR
    public DataTable GetOrdenarPor()
    {
        StringBuilder Querys = new StringBuilder();
        Querys.AppendLine("EXEC OCGN_Proce_Traza_OrdenarPor_Select");
        return gestorBaseDatos.ejecutarQuery_SQL(Querys.ToString());
    }

    //VEHICULOS
    public DataTable GetVehiculo()
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_VehSelect");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //COLORES
    public DataTable GetColores()
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_Color");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //QFS
    public DataTable GetQFS()
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_QF");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //TIPOS DE PREPARACION
    public DataTable GetTipoPreparacion()
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC dbo.Ocgn_Proce_TraMag_TipoPrep");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //ESTADOS
    public DataTable GetEstados(string modCod)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC dbo.OCGN_Proce_TraMag_EstxMod");
        sb.AppendLine($" @ModCod = '{modCod}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //MOTIVOS DE DEVOLUCION
    public DataTable GetMotDev()
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC dbo.OCGN_Proce_TraMag_MotDev");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    #endregion

    #region Alistamiento
    public DataTable GetTraMagAliLog(string prepcod)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMagAliLog");
        sb.AppendLine($" @PrepCod = '{prepcod}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }
    #endregion

    #region Solicitudes
    //ORDENES EXTERNAS
    public DataTable GetOrdenesExt(string empresa, string sede, string servicio)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_OrdenesExt");
        sb.AppendLine($" @empresa = '{empresa}',");
        sb.AppendLine($" @sede    = '{sede}',");
        sb.AppendLine($" @Servicio = '{servicio}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    #endregion

    #region Produccion
    //ORDENES DE PRODUCCION
    public DataTable GetOrdenes(string empresa, string sede, string servicio)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_Ordenes");
        sb.AppendLine($" @empresa = '{empresa}',");
        sb.AppendLine($" @sede    = '{sede}',");
        sb.AppendLine($" @Servicio = '{servicio}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }


    //BUSCAR SI YA TIENE INFO DE ETIQUETA
    public DataTable GetTraMag1(string empresa, string sede, long csc)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_GetTraMag1");
        sb.AppendLine($"  @Empresa    = '{empresa}',");
        sb.AppendLine($"  @Sede      = '{sede}',");
        sb.AppendLine($"  @Csc      = {csc}");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //TRAMAG1 ETIQUETA PARA IMPRIMIR
    public DataTable GetTraMag1Eti(string empresa, string sede, long csc)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag1_Eti");
        sb.AppendLine($"  @Empresa    = '{empresa}',");
        sb.AppendLine($"  @Sede      = '{sede}',");
        sb.AppendLine($"  @Csc      = {csc}");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    #endregion

    #region Revision
    //OBTENER INFO PARA TABLA DE REVISION
    public DataTable GetTraMagRev(string empresa, string sede, string servicio)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_Rev");
        sb.AppendLine($"  @empresa    = '{empresa}',");
        sb.AppendLine($"  @sede      = '{sede}',");
        sb.AppendLine($"  @servicio  = '{servicio}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    #endregion

    #region Reportes
    //OBTENER INFO PARA TABLA DE REPORTE DE ORDEN DE PRODUCCION
    public DataTable GetOrdDeProd1(string empresa, string sede, string servicio, string fecha)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC Ocgn_Proce_TraMag_OrdenDeProd");
        sb.AppendLine($"  @Fecha    = '{fecha}',");
        sb.AppendLine($"  @Empresa    = '{empresa}',");
        sb.AppendLine($"  @Sede      = '{sede}',");
        sb.AppendLine($"  @Servicio  = '{servicio}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //OBTENER INFO PARA TABLA DE REPORTE DE ORDEN DE PRODUCCION DE SOLICITUDES EXTERNAS
    public DataTable GetOrdDeProdExt1(string empresa, string sede, string servicio, string fecha)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC Ocgn_Proce_TraMag_OrdenDeProdExt");
        sb.AppendLine($"  @Fecha    = '{fecha}',");
        sb.AppendLine($"  @Empresa    = '{empresa}',");
        sb.AppendLine($"  @Sede      = '{sede}',");
        sb.AppendLine($"  @Servicio  = '{servicio}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //OBTENER LOGO DE EMPRESA
    public DataTable GetLogoEmp(string empresa)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC Ocgn_Proce_TraMag_Logo");
        sb.AppendLine($"  @Empresa    = '{empresa}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //OBTENER INFO DE VERSION
    public DataTable GetInfoVersion(string empresa, string sede, string codapli)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC OCGN_Proce_TraMag_version");
        sb.AppendLine($"  @Empresa    = '{empresa}',");
        sb.AppendLine($"  @sede    = '{sede}',");
        sb.AppendLine($"  @codigo    = '{codapli}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }

    //OBTENER NOMBRE DE EMPRESA
    public DataTable GetNomServicio(string empresa, string sede, string servicio)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EXEC Ocgn_Proce_TraMag_Servicio");
        sb.AppendLine($"  @Empresa    = '{empresa}',");
        sb.AppendLine($"  @Sede    = '{sede}',");
        sb.AppendLine($"  @Servicio    = '{servicio}'");
        return gestorBaseDatos.ejecutarQuery_SQL(sb.ToString());
    }
    #endregion

}