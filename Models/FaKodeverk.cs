namespace UttrekkFamilia.Models
{
    public partial class FaKodeverk
    {
        public string KodKategori { get; set; }
        public string KodKode { get; set; }
        public decimal? KodSsbkode { get; set; }
        public string KodLangtekst { get; set; }
        public string KodKorttekst { get; set; }
        public int? KodSubcategory { get; set; }
    }
}
