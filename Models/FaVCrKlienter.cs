using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrKlienter
    {
        public decimal KliLoepenr { get; set; }
        public string KliFornavn { get; set; }
        public string KliEtternavn { get; set; }
        public DateTime? KplFradato { get; set; }
        public DateTime? KplTildato { get; set; }
        public DateTime? KliFoedselsdato { get; set; }
        public decimal? KliPersonnr { get; set; }
        public string SbhInitialer { get; set; }
        public string KplPlasseringbor { get; set; }
        public string DisDistriktskode { get; set; }
    }
}
