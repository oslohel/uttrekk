using System;

namespace UttrekkFamilia.Models
{
    public partial class FaStatTabell25sum
    {
        public DateTime S25PeriodeStartdato { get; set; }
        public DateTime S25PeriodeSluttdato { get; set; }
        public string S25Distrikter { get; set; }
        public decimal? S25Fh1PlassertEgen { get; set; }
        public decimal? S25Fh1SumBesoek { get; set; }
        public decimal? S25Fh1SnittBesoek { get; set; }
        public decimal? S25Fh2PlassertAndre { get; set; }
        public decimal? S25Fh2SumBesoek { get; set; }
        public decimal? S25Fh2SnittBesoek { get; set; }
        public decimal? S25Fh3SumOppfoelgbesoek { get; set; }
        public decimal? S25Fh3SnittOppfoelgbesoek { get; set; }
        public string S25Klientgrupper { get; set; }
        public string S25Saksbehandlere { get; set; }
        public string S25Distriktsnavn { get; set; }
    }
}
