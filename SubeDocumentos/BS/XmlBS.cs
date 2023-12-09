using Microsoft.AspNetCore.Hosting.Server;
using SubeDocumentos.Model.XmlModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SubeDocumentos.BS
{
    public class XmlBS
    {
        private static XmlBS instanceBS;
        public static XmlBS InstanceBS
        {
            get
            {
                if (instanceBS == null)
                {
                    return new XmlBS();
                }
                return instanceBS;
            }
        }
        public List<Factura> ReadXml(List<string> xmlList)
        {
            List<Factura> facturasList = new List<Factura>();
            foreach (string xml in xmlList)
            {
                XmlDocument xmlFactura = new XmlDocument();
                xmlFactura.Load(xml);

                XmlNodeList xmlComprobante = xmlFactura.GetElementsByTagName("cfdi:Comprobante");
                Console.WriteLine(xmlComprobante[0].Attributes["Fecha"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Sello"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["NoCertificado"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Certificado"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Moneda"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["TipoDeComprobante"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Exportacion"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["MetodoPago"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Serie"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Folio"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["LugarExpedicion"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["SubTotal"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Descuento"].Value);
                Console.WriteLine(xmlComprobante[0].Attributes["Total"].Value);

                XmlNodeList xmlEmisor = xmlFactura.GetElementsByTagName("cfdi:Emisor");
                Console.WriteLine(xmlEmisor[0].Attributes["Nombre"].Value);
                Console.WriteLine(xmlEmisor[0].Attributes["Rfc"].Value);
                Console.WriteLine(xmlEmisor[0].Attributes["Nombre"].Value);

                XmlNodeList xmlReceptor = xmlFactura.GetElementsByTagName("cfdi:Receptor");
                Console.WriteLine(xmlReceptor[0].Attributes["Rfc"].Value);
                Console.WriteLine(xmlReceptor[0].Attributes["Nombre"].Value);
                Console.WriteLine(xmlReceptor[0].Attributes["DomicilioFiscalReceptor"].Value);
                Console.WriteLine(xmlReceptor[0].Attributes["RegimenFiscalReceptor"].Value);

                XmlNodeList xmlConceptos = xmlFactura.GetElementsByTagName("cfdi:Conceptos");
                foreach (XmlNode xmlConcepto in xmlConceptos)
                {
                    Console.WriteLine(xmlConcepto.Attributes["ClaveProdServ"].Value);
                    Console.WriteLine(xmlConcepto.Attributes["Cantidad"].Value);
                    Console.WriteLine(xmlConcepto.Attributes["ObjetoImp"].Value);
                    Console.WriteLine(xmlConcepto.Attributes["ValorUnitario"].Value);
                    Console.WriteLine(xmlConcepto.Attributes["Importe"].Value);
                    Console.WriteLine(xmlConcepto.Attributes["Descuento"].Value);
                }



            }
            return facturasList;
        }

    }
}
