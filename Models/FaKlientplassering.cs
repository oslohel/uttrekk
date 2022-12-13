using System;

namespace UttrekkFamilia.Models
{
    public partial class FaKlientplassering
    {
        public decimal KliLoepenr { get; set; }
        public DateTime KplFradato { get; set; }
        public decimal? KomKommunenr { get; set; }
        public DateTime? KplTildato { get; set; }
        public decimal KplPlassert { get; set; }
        public string KplPlasseringbor { get; set; }
        public string KplBorhos { get; set; }
        public decimal KplPlassertannenbydel { get; set; }
        public string KplMerknad { get; set; }
        public decimal KplOppfoelgingsbesoek { get; set; }
        public decimal KplTilsynsbesoek { get; set; }
        public decimal KplTilsynsansvarSelv { get; set; }
        public string KplBorhosKategori { get; set; }
        public string KplPlasseringborKategori { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaKommuner KomKommunenrNavigation { get; set; }
    }
}
