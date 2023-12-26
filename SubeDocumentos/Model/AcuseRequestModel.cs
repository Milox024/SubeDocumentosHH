using SubeDocumentos.Model.XmlModel;

namespace SubeDocumentos.Model
{
    public class AcuseRequestModel
    {
        public string TipoDocumento { get; set; }
        public Factura Factura { get; set; }
    }
}
