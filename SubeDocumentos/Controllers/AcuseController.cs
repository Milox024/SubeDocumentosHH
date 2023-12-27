using DocumentFormat.OpenXml.Vml;
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
        [HttpPost, Route("GenerarAcuseOrdenTrabajo")]
        public IActionResult GenerarAcuseOrdenTrabajo([FromBody] AcuseOrdenTrabajoModel request)
        {
            try
            {
                string rutaSalida = Configuration.GetSection("RUTA_ACUSE_GENERADOS").Value;
                string rutaPlantilla = Configuration.GetSection("ACUSE_ORDEN_TRABAJO").Value;
                string rutaAcuseGen = new AcuseOrdenTrabajoBS().GenerarAcuse(rutaPlantilla, rutaSalida, request);

                return Ok(new
                {
                    Status = 200,
                    AcuseGenerado = rutaAcuseGen,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = ex.Message
                });
            }
        }
        [HttpPost, Route("GenerarAcuseContrato")]
        public IActionResult GenerarAcuseContrato([FromBody] AcuseContratoModel request)
        {
            try
            {
                string rutaSalida = Configuration.GetSection("RUTA_ACUSE_GENERADOS").Value;
                string rutaPlantilla = Configuration.GetSection("ACUSE_CONTRATO").Value;
                string rutaAcuseGen = new AcuseContratoBS().GenerarAcuse(rutaPlantilla, rutaSalida, request);

                return Ok(new
                {
                    Status = 200,
                    AcuseGenerado = rutaAcuseGen,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = ex.Message
                });
            }
        }
        [HttpPost, Route("GenerarAcusePedido")]
        public IActionResult GenerarAcusePedido([FromBody] AcusePedidoModel request)
        {
            try
            {
                string rutaSalida = Configuration.GetSection("RUTA_ACUSE_GENERADOS").Value;
                string rutaPlantilla = Configuration.GetSection("ACUSE_PEDIDO").Value;
                string rutaAcuseGen = new AcusePedidoBS().GenerarAcuse(rutaPlantilla, rutaSalida, request);

                return Ok(new
                {
                    Status = 200,
                    AcuseGenerado = rutaAcuseGen,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = ex.Message
                });
            }
        }
    }
}
