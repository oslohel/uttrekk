using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrMangelfullenglinje
    {
        public decimal EnlLoepenr { get; set; }
        public decimal KliLoepenr { get; set; }
        public decimal EnpLoepenr { get; set; }
        public string DisDistriktskode { get; set; }
        public decimal? EnlBeloep { get; set; }
        public DateTime EnlForfallsdato { get; set; }
        public DateTime? EnlAnvistdato { get; set; }
        public string EnlBetalingstype { get; set; }
    }
}
