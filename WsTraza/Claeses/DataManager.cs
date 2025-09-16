using System.Data;
using System.Data.SqlClient;
using System.Text;
using Serilog;

namespace WsTraza.Class;

public class DataManager
{
    private Conexion conexionManager_SQL;
    private DataTable datos;
    private readonly string _cadenaConexion;

    public DataManager(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    //-----------------------
    //---------SQL-----------
    //-----------------------
    public DataTable ejecutarQuery_SQL(string query)
    {
        conexionManager_SQL = new Conexion(_cadenaConexion);
        try
        {
            conexionManager_SQL.iniciarConexion_SQL();
            datos = conexionManager_SQL.ejecutarQuery_SQL(query);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conexionManager_SQL.cerrarConexion_SQL();
        }

        return datos;
    }

    public void ejecutarTransaccion_SQL(List<string> querys)
    {
        conexionManager_SQL = new Conexion(_cadenaConexion);
        try
        {
            conexionManager_SQL.iniciarTransaccion_SQL();
            foreach (var q in querys)
                conexionManager_SQL.ejecutarNonQuery_SQL(q);
            conexionManager_SQL.comitarTransaccion_SQL();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conexionManager_SQL.cerrarConexion_SQL();
        }
    }

    public string ejecutarQuery_SQL_Json(string query)
    {
        var result = new StringBuilder();

        using var connectionManagerSql = new Conexion(_cadenaConexion);
        try
        {
            // Crear la conexión
            connectionManagerSql.iniciarConexion_SQL();

            // Validar si la conexión está abierta antes de ejecutar la consulta
            if (connectionManagerSql.GetConnection().State != ConnectionState.Open)
            {
                connectionManagerSql.GetConnection().Open();
            }

            using var command = new SqlCommand(query, connectionManagerSql.GetConnection());
            using var reader = command.ExecuteReader();

            // Leer los datos y construir el JSON
            while (reader.Read())
            {
                result.Append(reader.GetString(0)); // Asumimos que la consulta solo retorna una columna con el JSON.
            }
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Error executing SQL query.", ex);
        }
        finally
        {
            // Cerrar la conexión (se cierra automáticamente en el bloque using)
            connectionManagerSql.cerrarConexion_SQL();
        }

        return result.ToString();
    }
    
    private class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}