using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrMangelfullbetaling
    {
        public decimal UtbLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public decimal? UtpLoepenr { get; set; }
        public string DisDistriktskode { get; set; }
        public decimal? UtbBeloep { get; set; }
        public DateTime UtbForfallsdato { get; set; }
        public DateTime? UtbAnvistdato { get; set; }
        public string UtbUBetalingsmaate { get; set; }
        public string UtbBetalingstype { get; set; }
    }
}
