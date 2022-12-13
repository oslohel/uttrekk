using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrStatGrunnlag5Kobling
    {
        public Guid? Nr { get; set; }
        public decimal KliLoepenr { get; set; }
        public string Punkt { get; set; }
        public string Sf1Punkt { get; set; }
        public DateTime Sf1PeriodeFradato { get; set; }
        public string DisDistriktskode { get; set; }
        public decimal? Sf1Gsaaar { get; set; }
        public decimal? Sf1Gsajournalnr { get; set; }
    }
}
