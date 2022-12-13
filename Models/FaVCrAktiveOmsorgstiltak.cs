using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrAktiveOmsorgstiltak
    {
        public decimal TilLoepenr { get; set; }
        public string LovHovedparagraf { get; set; }
        public string LovJmfparagraf1 { get; set; }
        public string LovJmfparagraf2 { get; set; }
        public DateTime? SakSlutningdato { get; set; }
        public decimal KliLoepenr { get; set; }
        public DateTime? PlukkDato { get; set; }
    }
}
