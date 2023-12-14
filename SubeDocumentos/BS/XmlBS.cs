using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using SubeDocumentos.Model;
using SubeDocumentos.Model.XmlModel;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
        public string IsNull(XmlAttribute att, string type = "string") 
        {
            if (att != null)
            {
                return att.Value;
            }
            else
            {
                if (type.Equals("decimal")) return "0";
                if (type.Equals("string")) return "";
            }
            return "";
        }

        public string ValidarVersion(Factura factura)
        {
            if (factura.Version.Equals("3.3") && Convert.ToDateTime(factura.Fecha) >= (new DateTime(2023, 4, 1)))
            {
                return "A partir del 01 de abril del 2023, solo se permite version 4.0";
            }
            return "Ok";
        }

        public string ValidaFactura(Factura factura, DatosValidos dv)
        {
            if (!factura.Receptor.Rfc.Equals(dv.Rfc))
            {
                return "El RFC del Emisor no es Valido";
            }
            if (!factura.Receptor.RegimenFiscalReceptor.Equals(dv.RegimenFiscalReceptor))
            {
                return "El Regimen Fiscal del Emisor no es Valido";
            }
            if (!factura.Version.Equals("4.0"))
            {
                return "La Version del documento es incorrecta";
            }
            if (!dv.UsoCFDIList.Contains(factura.Receptor.UsoCFDI))
            {
                return "El Uso de CFDI no es Valido.";
            }
            if (!factura.MetodoPago.Equals(dv.MetodoPago))
            {
                return "El MetodoPago no es Valido";
            }
            if (!factura.FormaPago.Equals(dv.FormaPago))
            {
                return "El FormaPago no es Valido";
            }

            return "Ok";
        }

        public string ValidaRembolso(Factura factura, DatosValidos dv)
        {
            if (!factura.Receptor.Rfc.Equals(dv.Rfc))
            {
                return "El RFC del Emisor no es Valido";
            }
            if (!factura.Receptor.RegimenFiscalReceptor.Equals(dv.RegimenFiscalReceptor))
            {
                return "El Regimen Fiscal del Emisor no es Valido";
            }
            if (!dv.UsoCFDIList.Contains(factura.Receptor.UsoCFDI))
            {
                return "El Uso de CFDI no es Valido.";
            }
            return "Ok";
        }

        public Factura ReadXmlToObjet(XmlDocument xmlFactura)
        {
            Factura f = new Factura();
            XmlElement xmlComprobante = xmlFactura.DocumentElement;
            f.Fecha = IsNull(xmlComprobante.Attributes["Fecha"]);
            f.Sello = IsNull(xmlComprobante.Attributes["Sello"]);
            f.NoCertificado = IsNull(xmlComprobante.Attributes["NoCertificado"]);
            f.Certificado = IsNull(xmlComprobante.Attributes["Certificado"]);
            f.Moneda = IsNull(xmlComprobante.Attributes["Moneda"]);
            f.TipoDeComprobante = IsNull(xmlComprobante.Attributes["TipoDeComprobante"]);
            f.Exportacion = IsNull(xmlComprobante.Attributes["Exportacion"]);
            f.MetodoPago = IsNull(xmlComprobante.Attributes["MetodoPago"]);
            f.Serie = IsNull(xmlComprobante.Attributes["Serie"]);
            f.Folio = IsNull(xmlComprobante.Attributes["Folio"]);
            f.LugarExpedicion = IsNull(xmlComprobante.Attributes["LugarExpedicion"]);
            f.SubTotal = IsNull(xmlComprobante.Attributes["SubTotal"], "decimal");
            f.Descuento = IsNull(xmlComprobante.Attributes["Descuento"], "decimal");
            f.Total = IsNull(xmlComprobante.Attributes["Total"], "decimal");
            f.Version = IsNull(xmlComprobante.Attributes["Version"]);
            f.FormaPago = IsNull(xmlComprobante.Attributes["FormaPago"]);
            f.CondicionesDePago = IsNull(xmlComprobante.Attributes["CondicionesDePago"]);
            XmlNodeList ComprobanteNodes = xmlComprobante.ChildNodes;
            foreach (XmlNode NodosHijo in ComprobanteNodes)
            {
                string nodo = NodosHijo.Name;
                switch (nodo)
                {
                    case "cfdi:Emisor":
                        f.Emisor = new Emisor
                        {
                            RegimenFiscal = IsNull(NodosHijo.Attributes["RegimenFiscal"]),
                            Rfc = IsNull(NodosHijo.Attributes["Rfc"]),
                            Nombre = IsNull(NodosHijo.Attributes["Nombre"])
                        };
                        break;
                    case "cfdi:Receptor":
                        f.Receptor = new Receptor
                        {
                            Rfc = IsNull(NodosHijo.Attributes["Rfc"]),
                            Nombre = IsNull(NodosHijo.Attributes["Nombre"]),
                            DomicilioFiscalReceptor = IsNull(NodosHijo.Attributes["DomicilioFiscalReceptor"]),
                            RegimenFiscalReceptor = IsNull(NodosHijo.Attributes["RegimenFiscalReceptor"]),
                            UsoCFDI = IsNull(NodosHijo.Attributes["UsoCFDI"])
                        };
                        break;
                    case "cfdi:Conceptos":
                        XmlNodeList Conceptos = NodosHijo.ChildNodes;
                        f.Conceptos = new List<Concepto>();
                        foreach (XmlNode ConceptoHijo in Conceptos)
                        {
                            Concepto c = new Concepto
                            {
                                Cantidad = IsNull(ConceptoHijo.Attributes["Cantidad"]),
                                ClaveProdServ = IsNull(ConceptoHijo.Attributes["ClaveProdServ"]),
                                ClaveUnidad = IsNull(ConceptoHijo.Attributes["ClaveUnidad"]),
                                Descripcion = IsNull(ConceptoHijo.Attributes["Descripcion"]),
                                Importe = IsNull(ConceptoHijo.Attributes["Importe"]),
                                NoIdentificacion = IsNull(ConceptoHijo.Attributes["NoIdentificacion"]),
                                ObjetoImp = IsNull(ConceptoHijo.Attributes["ObjetoImp"]),
                                Unidad = IsNull(ConceptoHijo.Attributes["Unidad"]),
                                ValorUnitario = IsNull(ConceptoHijo.Attributes["ValorUnitario"]),
                            };
                            c.Impuestos = new List<Impuesto>();
                            XmlNodeList ImpuestosConceptos = ConceptoHijo.ChildNodes;
                            foreach (XmlNode ImpuestoConcepto in ImpuestosConceptos)
                            {
                                Impuesto ic = new Impuesto { Traslados = new List<Traslado>() };
                                XmlNodeList TrasladosImpuestos = ImpuestoConcepto.ChildNodes;
                                foreach (XmlNode TrasladoImpuesto in TrasladosImpuestos)
                                {
                                    List<Traslado> traslados = new List<Traslado>();
                                    XmlNodeList Traslados = TrasladoImpuesto.ChildNodes;
                                    foreach (XmlNode t in Traslados)
                                    {
                                        traslados.Add(new Traslado
                                        {
                                            Base = IsNull(t.Attributes["Base"]),
                                            Importe = IsNull(t.Attributes["Importe"]),
                                            Impuesto = IsNull(t.Attributes["Impuesto"]),
                                            TasaOCuota = IsNull(t.Attributes["TasaOCuota"]),
                                            TipoFactor = IsNull(t.Attributes["TipoFactor"])
                                        });
                                    }
                                    ic.Traslados = traslados;
                                }
                                c.Impuestos.Add(ic);
                            }
                            f.Conceptos.Add(c);
                        }
                        break;
                    case "cfdi:Impuestos":
                        f.Impuestos = new List<Impuesto>();
                        Impuesto impuesto = new Impuesto();
                        impuesto.TotalImpuestosTrasladados = IsNull(NodosHijo.Attributes["TasaOCuota"]);
                        XmlNodeList Impuestos = NodosHijo.ChildNodes;
                        foreach (XmlNode Impuesto in Impuestos)
                        {
                            List<Traslado> traslados = new List<Traslado>();
                            XmlNodeList Traslados = Impuesto.ChildNodes;
                            foreach (XmlNode t in Traslados)
                            {
                                traslados.Add(new Traslado
                                {
                                    Base = IsNull(t.Attributes["Base"]),
                                    Importe = IsNull(t.Attributes["Importe"]),
                                    Impuesto = IsNull(t.Attributes["Impuesto"]),
                                    TasaOCuota = IsNull(t.Attributes["TasaOCuota"]),
                                    TipoFactor = IsNull(t.Attributes["TipoFactor"])
                                });
                            }
                            impuesto.Traslados = traslados;
                        }
                        f.Impuestos.Add(impuesto);
                        break;
                    case "cfdi:Complemento":
                        f.Complemento = new Complemento();
                        XmlNodeList TimbreFiscalDigital = NodosHijo.ChildNodes;
                        foreach(XmlNode tfd in TimbreFiscalDigital) 
                        {
                            f.Complemento.TimbreFiscalDigital = new TimbreFiscalDigital
                            {
                                Version = IsNull(tfd.Attributes["Version"]),
                                UUID = IsNull(tfd.Attributes["UUID"]),
                                FechaTimbrado = IsNull(tfd.Attributes["FechaTimbrado"]),
                                RfcProvCertif = IsNull(tfd.Attributes["RfcProvCertif"]),
                                SelloCFD = IsNull(tfd.Attributes["SelloCFD"]),
                                NoCertificadoSAT = IsNull(tfd.Attributes["NoCertificadoSAT"]),
                                SelloSAT = IsNull(tfd.Attributes["SelloSAT"])
                            };
                        }
                        break;
                }
            }
            return f;
        }

    }
}
