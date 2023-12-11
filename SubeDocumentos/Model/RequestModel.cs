using System.ComponentModel.DataAnnotations;

namespace SubeDocumentos.Model
{
    public class RequestModel
    {
        public int IdUsuario { get; set; }
        public bool EsRembolso { get; set; }
        public bool ValidaArchivos { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
