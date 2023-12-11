using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Configuration;
using SubeDocumentos.BS;
using SubeDocumentos.Model;
using SubeDocumentos.Model.XmlModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SubeDocumentos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment _env;

        public DocumentsController(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _env = env;
        }

        [HttpPost, Route("loadFile")]
        public IActionResult UploadFile([FromForm] RequestModel request)
        {
            try
            {
                IFormFile pdf = request.Files.Where(f => f.ContentType.Contains("pfd")).FirstOrDefault();
                IFormFile xml = request.Files.Where(f => f.ContentType.Contains("xml")).FirstOrDefault();
                CultureInfo ci = new CultureInfo("es-ES");
                List<string> filesToRead = new List<string>();
                List<string> filesPaths = new List<string>();

                var serializer = new XmlSerializer(typeof(XmlDocument));
                XmlDocument fact = (XmlDocument)serializer.Deserialize(xml.OpenReadStream());
                Factura fval = XmlBS.InstanceBS.ReadXmlToObjet(fact);

                bool ArchivosValidos = true;
                string msgValidacion = "";

                if (request.ValidaArchivos || request.EsRembolso) 
                {
                    if (request.ValidaArchivos)
                    {
                        string valresult = XmlBS.InstanceBS.ValidaFactura(fval);
                        msgValidacion = !valresult.Equals("Ok") ? valresult : "El CFDi cumple con las validaciones internas del portal.";
                        ArchivosValidos = valresult.Equals("Ok");
                    }
                    if (request.EsRembolso && ArchivosValidos)
                    {
                        string valresult = XmlBS.InstanceBS.ValidaRembolso(fval);
                        msgValidacion = !valresult.Equals("Ok") ? valresult : "El CFDi cumple con las validaciones internas del portal.";
                        ArchivosValidos = valresult.Equals("Ok");
                    }
                }
                if (!ArchivosValidos)
                {
                    return BadRequest(new
                    {
                        Status = 400,
                        Message = "",
                        RutaArchivos = new List<string>(),
                        XmlObject = new object { },
                        MsgValidacion = msgValidacion
                    });
                }
                else
                {
                    foreach (var file in request.Files)
                    {

                        string RutaCompleta = Configuration.GetSection("RutaArchivos").Value;
                        RutaCompleta = RutaCompleta.Replace("*idUsuario*", request.IdUsuario.ToString());
                        RutaCompleta = RutaCompleta.Replace("*anio*", DateTime.Now.ToString("yyyy"));
                        RutaCompleta = RutaCompleta.Replace("*mes*", DateTime.Now.ToString("MMMM", ci));
                        if (!Directory.Exists(RutaCompleta))
                        {
                            Directory.CreateDirectory(RutaCompleta);
                        }
                        if (file.Length > 0)
                        {
                            string NombreArchivo = file.FileName;
                            string RutaFullCompleta = Path.Combine(RutaCompleta, NombreArchivo);
                            using (var stream = new FileStream(RutaFullCompleta, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                filesPaths.Add(RutaFullCompleta);
                            }
                        }
                    }
                }
                return Ok(new
                {
                    Status = 200,
                    Message = "",
                    RutaArchivos = filesPaths,
                    XmlObject = fval,
                    MsgValidacion = msgValidacion
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 500,
                    Message = ex.Message,
                    Response = new object { }
                });
            }
        }
    }
}
