
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic;
using SubeDocumentos.Model;
using SubeDocumentos.Model.XmlModel;
using System;
using System.Globalization;

namespace SubeDocumentos.BS
{
    public class AcuseOrdenTrabajoBS : IAcuse
    {
        private static AcuseOrdenTrabajoBS instanceBS;
        public static AcuseOrdenTrabajoBS InstanceBS
        {
            get
            {
                if (instanceBS == null)
                {
                    return new AcuseOrdenTrabajoBS();
                }
                return instanceBS;
            }
        }

        public string GenerarAcuse(string rutaPlantilla, string rutaSalidaAcuse, Factura fac, string TipoDocumento)
        {
            string rutaArchivoSalida = rutaSalidaAcuse + "/" + TipoDocumento + "_" + fac.Emisor.Rfc + ".docx";
            using (var plantilla = File.Open(rutaPlantilla, FileMode.Open, FileAccess.ReadWrite)) 
            {
                using (var stream = new MemoryStream()) 
                {
                    plantilla.CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    if (!Directory.Exists(rutaSalidaAcuse)) 
                    {
                        Directory.CreateDirectory(rutaSalidaAcuse);
                    }
                    using (var fileStream = File.Create(rutaArchivoSalida)) 
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
            using (WordprocessingDocument wDocument = WordprocessingDocument.Open(rutaArchivoSalida, true)) 
            {
                var body = wDocument.MainDocumentPart.Document.Body;
                var paras = body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                List<LlavesRemplazo> rmp = GenerarListaRemplazos(fac);
                foreach (var llr in rmp)
                {
                    foreach (var para in paras)
                    {
                        foreach (var run in para.Elements<Run>())
                        {
                            foreach (var text in run.Elements<Text>())
                            {
                                if (text.Text.Contains(llr.Llave))
                                {
                                    text.Text = text.Text.Replace(llr.Llave, llr.LlaveValor);
                                }
                            }

                        }
                    }
                }
                wDocument.MainDocumentPart.Document.Save();
            }
            if (File.Exists(rutaArchivoSalida)) 
            {
                var wordApp = new Microsoft.Office.Interop.Word.Application();
                var wordAppDocument = wordApp.Documents.Open(rutaArchivoSalida.Replace("/", "\\"));

                wordAppDocument.ExportAsFixedFormat(rutaArchivoSalida.Replace(".docx", ".pdf"), WdExportFormat.wdExportFormatPDF);

                wordAppDocument.Close();
                wordApp.Quit();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return "";
        }

        public List<LlavesRemplazo> GenerarListaRemplazos(Factura f) 
        {
            f.Fecha = "2023-12-01";
            string conceptos = "";
            f.Conceptos.ForEach(concep => { conceptos += concep.Descripcion + ","; });

            List<LlavesRemplazo> rmp = new List<LlavesRemplazo>
            {
                new LlavesRemplazo { Llave = "[RRFC]", LlaveValor = f.Receptor.Rfc },
                new LlavesRemplazo { Llave = "[RRazonSocial]", LlaveValor = f.Receptor.Nombre },
                new LlavesRemplazo { Llave = "[ERFC]", LlaveValor = f.Emisor.Rfc },
                new LlavesRemplazo { Llave = "[ERazonSocial]", LlaveValor = f.Emisor.Nombre },
                new LlavesRemplazo { Llave = "[Mes]", LlaveValor = Convert.ToDateTime(f.Fecha).ToString("MMMM",CultureInfo.CreateSpecificCulture("es-ES")) },
                new LlavesRemplazo { Llave = "[Anio]", LlaveValor = Convert.ToDateTime(f.Fecha).ToString("yyyy") },
                new LlavesRemplazo { Llave = "[Uuid]", LlaveValor = f.Complemento.TimbreFiscalDigital.UUID },
                new LlavesRemplazo { Llave = "[Id]", LlaveValor = f.Complemento.TimbreFiscalDigital.NoCertificadoSAT },
                new LlavesRemplazo { Llave = "[FHRegistro]", LlaveValor = f.Fecha },
                new LlavesRemplazo { Llave = "[Respuesta]", LlaveValor = "" },
                new LlavesRemplazo { Llave = "[ODT]", LlaveValor = "" },
                new LlavesRemplazo { Llave = "[Conceptos]", LlaveValor = "" },
                new LlavesRemplazo { Llave = "[Contrato]", LlaveValor = "" },
                new LlavesRemplazo { Llave = "[Pedido]", LlaveValor = "" },
                new LlavesRemplazo { Llave = "[Articulo]", LlaveValor = "" }
            };

            return rmp;

        }
    }
}
