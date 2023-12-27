using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using SubeDocumentos.Model;

namespace SubeDocumentos.BS
{
    public class AcuseContratoBS
    {
        private static AcuseContratoBS instanceBS;
        public static AcuseContratoBS InstanceBS
        {
            get
            {
                if (instanceBS == null)
                {
                    return new AcuseContratoBS();
                }
                return instanceBS;
            }
        }

        public string GenerarAcuse(string rutaPlantilla, string rutaSalidaAcuse, AcuseContratoModel model)
        {
            string rutaArchivoSalida = rutaSalidaAcuse + "/" + model.Emisor + "_" + "ACUSE_CONTRATO_" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".docx";
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
                var wordApp = new Microsoft.Office.Interop.Word.Application();
                var wordAppDocument = wordApp.Documents.Open(rutaArchivoSalida.Replace("/", "\\"));

                wordAppDocument.ExportAsFixedFormat(rutaArchivoSalida.Replace(".docx", ".pdf"), WdExportFormat.wdExportFormatPDF);

                wordAppDocument.Close();
                wordApp.Quit();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return rutaArchivoSalida.Replace(".docx", ".pdf");
        }
    }
}
