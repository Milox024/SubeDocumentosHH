using DocumentFormat.OpenXml.Office2010.Excel;
using SubeDocumentos.Model.XmlModel;

namespace SubeDocumentos.Model
{
    public class AcuseContratoModel : AcuseModel
    {
        public string Contrato { get; set; }
        public string Concepto { get; set; }
    }
}
