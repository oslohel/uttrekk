using System;

namespace UttrekkFamilia.Models
{
    public partial class FaHuskelapp
    {
        public decimal HusLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public decimal? KliLoepenr { get; set; }
        public decimal? SakAar { get; set; }
        public decimal? SakJournalnr { get; set; }
        public DateTime? HusDato { get; set; }
        public decimal HusInternt { get; set; }
        public string HusEmne { get; set; }
        public string HusNotat { get; set; }
        public string HusOppfoelging { get; set; }
        public DateTime? HusOppfoelgingsdato { get; set; }
        public DateTime? HusOppfyltdato { get; set; }
        public DateTime? HusFrist { get; set; }
        public DateTime? HusRegistrertdato { get; set; }
        public DateTime? HusEndretdato { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaSaksjournal Sak { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
    }
}
