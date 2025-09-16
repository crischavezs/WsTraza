using System.Data;
using System.Data.SqlClient;

namespace WsTraza.Class;

public class Conexion : IDisposable
{
    string cadenaConexion;
    private SqlConnection conexion_SQL;
    private SqlCommand comando_SQL;
    private SqlDataReader datos_SQL;
    private SqlTransaction transaccion_SQL;


    public Conexion(string conex)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();

        cadenaConexion = builder.GetSection("ConnectionStrings:" + conex + "").Value;
    }

    public string getCadenaSQL()
    {
        return cadenaConexion;
    }

    //----------------------
    //-------SQL     -------
    //----------------------
    public void iniciarConexion_SQL()
    {
        try
        {
            conexion_SQL = new SqlConnection(cadenaConexion);
            conexion_SQL.Open();
            comando_SQL = new SqlCommand();
            comando_SQL.Connection = conexion_SQL;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void iniciarTransaccion_SQL()
    {
        iniciarConexion_SQL();
        transaccion_SQL = conexion_SQL.BeginTransaction();
        comando_SQL.Transaction = transaccion_SQL;
    }

    public void comitarTransaccion_SQL()
    {
        transaccion_SQL.Commit();
    }

    public void regresarTransaccion_SQL()
    {
        transaccion_SQL.Rollback();
    }

    public void cerrarConexion_SQL()
    {
        try
        {
            if (conexion_SQL != null)
            {
                conexion_SQL.Close();
                conexion_SQL.Dispose();
            }

            if (comando_SQL != null)
                comando_SQL.Dispose();
            if (datos_SQL != null)
                datos_SQL.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable ejecutarQuery_SQL(string query)
    {
        DataTable dt = new DataTable();
        try
        {
            comando_SQL.CommandTimeout = 0;
            comando_SQL.CommandText = query;
            datos_SQL = comando_SQL.ExecuteReader();
            dt.Load(datos_SQL);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return dt;
    }

    public void ejecutarNonQuery_SQL(string query)
    {
        try
        {
            comando_SQL.CommandTimeout = 0;
            comando_SQL.CommandText = query;
            comando_SQL.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //----------------------

    public void cerrarConnection_SQL()
    {
        try
        {
            if (conexion_SQL.State == ConnectionState.Open)
            {
                conexion_SQL.Close();
                conexion_SQL.Dispose();
            }

            comando_SQL?.Dispose();
            transaccion_SQL?.Dispose();
        }
        catch (Exception ex)
        {
            throw new Exception("Error cerrando la conexión SQL", ex);
        }
    }
    
    public SqlConnection GetConnection()
    {
        if (conexion_SQL == null)
        {
            throw new InvalidOperationException("Connection is not initialized.");
        }

        return conexion_SQL;
    }

    // Implementación de IDisposable para liberar los recursos no administrados
    public void Dispose()
    {
        cerrarConnection_SQL(); // Asegura que la conexión y los recursos sean cerrados y liberados
    }
}