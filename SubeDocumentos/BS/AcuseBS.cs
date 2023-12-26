using static SubeDocumentos.Model.Utils;

namespace SubeDocumentos.BS
{
    public class AcuseBS
    {
        private static AcuseBS instanceBS;
        public static AcuseBS InstanceBS
        {
            get
            {
                if (instanceBS == null)
                {
                    return new AcuseBS();
                }
                return instanceBS;
            }
        }
        public string GenerarAcuse()
        {

            return "OK;";
        }
    }
}
