using SubeDocumentos.Model.XmlModel;

namespace SubeDocumentos.BS
{
    public interface IAcuse
    {
        public string GenerarAcuse(string rutaPlantilla, string rutaSalida, Factura fac, string tDoc);
    }
}
