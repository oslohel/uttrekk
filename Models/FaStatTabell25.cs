using System;

namespace UttrekkFamilia.Models
{
    public partial class FaStatTabell25
    {
        public DateTime D25PeriodeStartdato { get; set; }
        public DateTime D25PeriodeSluttdato { get; set; }
        public decimal KliLoepenr { get; set; }
        public string D25Distrikter { get; set; }
        public decimal? D25Egenbydel { get; set; }
        public decimal? D25AntmndFh { get; set; }
        public decimal? D25Statistikkbesoek { get; set; }
        public decimal? D25Antbesoek { get; set; }
        public decimal? D25KravTilsynsbesoek { get; set; }
        public decimal? D25KravOppfoelgbesoek { get; set; }
        public decimal? D25AntOppfoelgbesoek { get; set; }
        public decimal? D25StatistikkOppfoelgbesoek { get; set; }
        public string D25Klientgrupper { get; set; }
        public string D25Saksbehandlere { get; set; }
        public string D25Distriktsnavn { get; set; }
    }
}
