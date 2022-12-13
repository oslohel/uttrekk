using System;

namespace UttrekkFamilia.Models
{
    public partial class FaTilstand
    {
        public string TsaLoepenr { get; set; }
        public decimal? TsaKliLoepenr { get; set; }
        public decimal? TsaType { get; set; }
        public string TsaSbhInitialer { get; set; }
        public DateTime? TsaEndretdato { get; set; }
        public string TsaTilstand { get; set; }
    }
}
