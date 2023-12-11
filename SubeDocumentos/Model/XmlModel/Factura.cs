using System;

namespace SubeDocumentos.Model.XmlModel
{
    public class Factura
    {
        public string RutaFisica { get; set; }
        public string Fecha { get; set; }
        public string Sello { get; set; }
        public string NoCertificado { get; set; }
        public string Certificado { get; set; }
        public string Moneda { get; set; }
        public string TipoDeComprobante { get; set; }
        public string Exportacion { get; set; }
        public string MetodoPago { get; set; }
        public string Serie { get; set; }
        public string Folio { get; set; }
        public string LugarExpedicion { get; set; }
        public string SubTotal { get; set; }
        public string Descuento { get; set; }
        public string Total { get; set; }
        public Emisor Emisor { get; set; }
        public Receptor Receptor { get; set; }
        public List<Concepto> Conceptos { get; set; }
        public List<Impuesto> Impuestos { get; set; }
        public Complemento Complemento { get; set; }
        public string Version { get; set; }
        public string FormaPago { get; set; }
        public string CondicionesDePago { get; set; }
    }
    public class Emisor
    {
        public string RegimenFiscal { get; set; }
        public string Rfc { get; set; }
        public string Nombre { get; set; }

    }
    public class Receptor
    {
        public string Rfc { get; set; }
        public string Nombre { get; set; }
        public string DomicilioFiscalReceptor { get; set; }
        public string RegimenFiscalReceptor { get; set; }
        public string UsoCFDI { get; set; }
    }
    public class Concepto
    {
        public string Cantidad { get; set; }
        public string ClaveProdServ { get; set; }
        public string ClaveUnidad { get; set; }
        public string Descripcion { get; set; }
        public string Importe { get; set; }
        public string NoIdentificacion { get; set; }
        public string ObjetoImp { get; set; }
        public string Unidad { get; set; }
        public string ValorUnitario { get; set; }
        public List<Impuesto> Impuestos { get; set; }
    }
    public class Impuesto
    {
        public string TotalImpuestosTrasladados { get; set; }
        public List<Traslado> Traslados { get; set; }
    }
    public class Traslado
    {
        public string Base { get; set; }
        public string Importe { get; set; }
        public string Impuesto { get; set; }
        public string TasaOCuota { get; set; }
        public string TipoFactor { get; set; }

    }
    public class Complemento
    {
        public TimbreFiscalDigital TimbreFiscalDigital { get; set; }
    }
    public class TimbreFiscalDigital
    {
        public string Version { get; set; }
        public string UUID { get; set; }
        public string FechaTimbrado { get; set; }
        public string RfcProvCertif { get; set; }
        public string SelloCFD { get; set; }
        public string NoCertificadoSAT { get; set; }
        public string SelloSAT { get; set; }

    }
}
