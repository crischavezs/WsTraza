namespace WsTraza.Models
{
    public class Ordenes
    {
        public string Empresa { get; set; }  
        public string Sede { get; set; }  
        public long? Servicio { get; set; }       
        public string TipoIde { get; set; }  
        public string Identificacion { get; set; }  
        public long? Ingreso { get; set; }  
        public long? Folio { get; set; }  
        public string Estado { get; set; }  
        public string UsuarioAdd { get; set; }  
        public string FechaAdd { get; set; }  
        public string? UsuarioMod { get; set; }  
        public string? FechaMod { get; set; }  
    }
}
