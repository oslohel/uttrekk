using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrKlientplassering
    {
        public decimal KliLoepenr { get; set; }
        public decimal? KomKommunenr { get; set; }
        public DateTime KplFradato { get; set; }
        public DateTime? KplTildato { get; set; }
        public decimal KplPlassert { get; set; }
        public string KplPlasseringbor { get; set; }
        public string KplBorhos { get; set; }
    }
}
