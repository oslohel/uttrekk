using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVStatFmkontroll
    {
        public DateTime FmkFradato { get; set; }
        public string DisDistriktskode { get; set; }
        public decimal FmkSkjemanr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public decimal? MelLoepenr { get; set; }
        public decimal? SakAar { get; set; }
        public decimal? SakJournalnr { get; set; }
        public string Linje1 { get; set; }
        public string Linje2 { get; set; }
        public string Linje3 { get; set; }
    }
}
