using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubeDocumentos.BS;
using SubeDocumentos.Model;
using static SubeDocumentos.Model.Utils;

namespace SubeDocumentos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AcuseController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment _env;

        public AcuseController(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _env = env;
        }
        [HttpPost, Route("GenerateDocument")]
        public IActionResult GenerateDocument([FromBody] AcuseRequestModel request)
        {
            string rutaSalida = Configuration.GetSection("RUTA_ACUSE_GENERADOS").Value;
            string rutaPlantilla = Configuration.GetSection(request.TipoDocumento).Value;
            IAcuse acuse = new AcuseOrdenTrabajoBS();
            acuse.GenerarAcuse(rutaPlantilla, rutaSalida, request.Factura, request.TipoDocumento);

            AcuseBS.InstanceBS.GenerarAcuse();
            return Ok();
        }
    }
}
