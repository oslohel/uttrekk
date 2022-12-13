using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVAktiveTiltak
    {
        public decimal TilLoepenr { get; set; }
        public string LovHovedparagraf { get; set; }
        public string LovJmfparagraf1 { get; set; }
        public string LovJmfparagraf2 { get; set; }
        public decimal KliLoepenr { get; set; }
        public DateTime? PlukkDato { get; set; }
    }
}
