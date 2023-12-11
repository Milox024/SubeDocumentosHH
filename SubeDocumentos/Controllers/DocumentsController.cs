using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SubeDocumentos.BS;
using SubeDocumentos.Model;
using SubeDocumentos.Model.XmlModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

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
                CultureInfo ci = new CultureInfo("es-ES");
                List<string> filesToRead = new List<string>();
                List<string> filesPaths = new List<string>();
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
                            if (file.ContentType.Contains("xml")) 
                            {
                                filesToRead.Add(RutaFullCompleta);
                            }
                        }
                    }
                }
                List<Factura> XmlList = XmlBS.InstanceBS.ReadXml(filesToRead);
                return Ok(new
                {
                    Status = 200,
                    Message = "",
                    RutaArchivos = filesPaths,
                    XmlList = XmlList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = ex.Message,
                    Response = new object { }
                });
            }
        }
    }
}
