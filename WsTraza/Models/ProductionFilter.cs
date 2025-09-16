namespace WsTraza.Models
{
    public class ProductionFilter
    {
        public string Servicio { get; set; }
        public string Cama { get; set; }
        public string OrdenNumero { get; set; }
        public string PacienteId { get; set; }
        public string Consecutivo { get; set; }
        public bool IncluirNutriciones { get; set; }
    }
}
