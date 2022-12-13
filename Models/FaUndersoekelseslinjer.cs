using System;

namespace UttrekkFamilia.Models
{
    public partial class FaUndersoekelseslinjer
    {
        public decimal UnpLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhInitialer2 { get; set; }
        public decimal MelLoepenr { get; set; }
        public DateTime UnpDato { get; set; }
        public string UnpBeskrivelse { get; set; }
        public string UnpEkstern { get; set; }
        public decimal UnpInternaktivitet { get; set; }
        public DateTime? UnpFrist { get; set; }
        public DateTime? UnpOppfyltdato { get; set; }
        public string UnpMerknad { get; set; }
        public decimal UnpTilhuskeliste { get; set; }

        public virtual FaUndersoekelser MelLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialer2Navigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
