using System.Diagnostics;
namespace WsTraza.Class
{
    public class Auxiliar
    {
        public string mensajeError(Exception ex,string Tipo)
        {
            var st = new StackTrace(ex, true);
            var fr = st.GetFrame(st.FrameCount - 1);

            var nombreArchivo = Path.GetFileName(fr.GetFileName());
            var nombreMetodo = "";
            var linea = fr.GetFileLineNumber();
            var Mesaje = "";
            if (Tipo == "W") { //Servicio
                nombreMetodo = fr.GetMethod().Name; // Extraer el nombre del método

                Mesaje= $"{ex.Message} Archivo: {nombreArchivo} Método: {nombreMetodo} Línea: {(linea > 0 ? linea.ToString() : "Desconocida")}";
            }
            if (Tipo == "A") //Aplicacion
            { 
                Mesaje = $"{ex.Message} <br/>Archivo: {nombreArchivo} <br/>Línea: {(linea > 0 ? linea.ToString() : "Desconocida")}";
            }

            return Mesaje;
        }


        //public string mensajeError(Exception ex)
        //{
        //    var st = new StackTrace(ex, true);
        //    var fr = st.GetFrame(st.FrameCount - 1);

        //    var nombreArchivo = Path.GetFileName(fr.GetFileName());
        //    var nombreMetodo = Path.GetFileName(fr.GetFileName());
        //    var linea = fr.GetFileLineNumber();

        //    return $"{ex.Message} {ex.StackTrace} <br/>Archivo: {nombreArchivo} <br/>Linea: {(linea > 0 ? linea.ToString() : "Desconocida")}";
        //}
    }
}
