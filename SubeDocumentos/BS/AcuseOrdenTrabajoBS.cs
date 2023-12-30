
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualBasic;
using Spire.Doc.Documents;
using SubeDocumentos.Model;
using DocumentSpire = Spire.Doc.Document;
using SubeDocumentos.Model.XmlModel;
using System;
using System.Globalization;
using System.Reflection;

namespace SubeDocumentos.BS
{
    public class AcuseOrdenTrabajoBS
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

        public string GenerarAcuse(string rutaPlantilla, string rutaSalidaAcuse, AcuseOrdenTrabajoModel model)
        {
            string rutaArchivoSalida = rutaSalidaAcuse + "/" + model.Emisor + "_" + "ACUSE_ORDEN_TRABAJO_" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".docx";
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
                List<LlavesRemplazo> rmp = new Utils().GenerarListaRemplazos(model);
                foreach (var llr in rmp)
                {
                    foreach (var para in paras)
                    {
                        if (para.InnerText.Contains(llr.Llave))
                        {
                            string x = para.InnerText;
                            para.AddChild(new Run(new Text(llr.LlaveValor)));
                        }
                    }
                }
                wDocument.MainDocumentPart.Document.Save();
            }
            if (File.Exists(rutaArchivoSalida))
            {
                DocumentSpire document = new DocumentSpire();
                document.LoadFromFile(rutaArchivoSalida);
                //Convert Word to PDF
                document.SaveToFile(rutaArchivoSalida.Replace(".docx", ".pdf"), FileFormat.PDF);

                if (File.Exists(rutaArchivoSalida))
                    File.Delete(rutaArchivoSalida);
            }
            return rutaArchivoSalida.Replace(".docx", ".pdf");
        }

    }
}
