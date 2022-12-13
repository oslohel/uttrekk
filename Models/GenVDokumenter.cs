namespace UttrekkFamilia.Models
{
    public partial class GenVDokumenter
    {
        public decimal DokLoepenr { get; set; }
        public string SbhUtsjekketavInitialer { get; set; }
        public string DokType { get; set; }
        public decimal DokProdusert { get; set; }
        public decimal DokLaast { get; set; }
        public byte[] DokDokument { get; set; }
    }
}
