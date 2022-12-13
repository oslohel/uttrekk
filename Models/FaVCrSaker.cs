using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrSaker
    {
        public string Tabell { get; set; }
        public decimal? KliLoepenr { get; set; }
        public decimal MelLoepenr { get; set; }
        public decimal? UmelLoepenr { get; set; }
        public decimal? TilLoepenr { get; set; }
        public DateTime? Startdato { get; set; }
        public DateTime? Sluttdato { get; set; }
        public string Konklusjon { get; set; }
        public string Status { get; set; }
        public string Sbh2 { get; set; }
    }
}
