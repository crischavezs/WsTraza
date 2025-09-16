namespace WsTraza.Models
{
    public class Etiquetas
    {
        public string EtiEmp { get; set; }
        public string EtiSed { get; set; }
        public long EtiSer { get; set; }
        public long EtiFolio { get; set; }
        public string EtiMSCodi { get; set; }
        public string EtiMSPrAc { get; set; }
        public string EtiCncCd { get; set; }
        public string EtiMSForm { get; set; }
        public string EtiDosis24H { get; set; }
        public string EtiFotosen { get; set; }
        public string EtiVehRec { get; set; }
        public string EtiVolRec { get; set; }
        public string EtiConRec { get; set; }
        public string EtiVehDil { get; set; }
        public string EtiVolDil { get; set; }
        public string EtiConDil { get; set; }
        public string EtiFlu { get; set; }
        public string EtiVia { get; set; }
        public string EtiLeyAlm { get; set; }
        public string EtiLeyAdm { get; set; }
        public string EtiColor { get; set; }
        public long EstImpr { get; set; }
        public string? Obs { get; set; }
        public string Estado { get; set; }
        public string UsuAdd { get; set; }
        public string? FecAdd { get; set; }
        public string? UsaMod { get; set; }
        public string? FecMod { get; set; }
    }

    public class TraMag1Consulta
    {
        public string Empresa { get; set; }
        public string Sede { get; set; }
        public long Csc { get; set; }
    }
}

