using System.Reflection;

namespace SubeDocumentos.Model
{
    public class Utils
    {
        public List<LlavesRemplazo> GenerarListaRemplazos(object f)
        {
            List<LlavesRemplazo> rmp = new List<LlavesRemplazo>();
            Type t = f.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
            {
                rmp.Add(new LlavesRemplazo { Llave = "[" + prop.Name + "]", LlaveValor = prop.GetValue(f).ToString() });
            }
            return rmp;
        }
    }
}
