using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVStatKontrollskjema
    {
        public decimal? KliLoepenr { get; set; }
        public string DisDistriktskode { get; set; }
        public decimal? MelLoepenr { get; set; }
        public decimal? SakJournalnr { get; set; }
        public decimal? SakAar { get; set; }
        public DateTime? DatoSkjema { get; set; }
    }
}
