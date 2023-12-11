using SubeDocumentos.Model.XmlModel;

namespace SubeDocumentos.Model
{
    public class DatosValidos
    {
        public string Rfc { get; set; }
        public string RegimenFiscalReceptor { get; set; }
        public List<string> UsoCFDIList { get; set; }
        public string MetodoPago { get; set; }
        public string FormaPago { get; set; }
    }
}