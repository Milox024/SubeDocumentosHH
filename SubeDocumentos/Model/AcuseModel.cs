namespace SubeDocumentos.Model
{
    public class AcuseModel
    {
        public int AcuseCCFDIID { get; set; }
        public int CfdiID { get; set; }
        public int CatTipoDocumentoID { get; set; }
        public int ProveedorID { get; set; }
        public int UsuarioID { get; set; }
        public string Receptor { get; set; }
        public string RazonSocial_R { get; set; }
        public string Emisor { get; set; }
        public string RazonSocial_E { get; set; }
        public string Mes { get; set; }
        public int Año { get; set; }
        public string FolioFiscal { get; set; }
        public int Id { get; set; }
        public string FechaHora { get; set; }
        public string Respuesta { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public int UsuarioID_Alta { get; set; }

    }
}
