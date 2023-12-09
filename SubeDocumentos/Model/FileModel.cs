using System.ComponentModel.DataAnnotations;

namespace SubeDocumentos.Model
{
    public class RequestModel
    {
        public int IdUsuario { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
