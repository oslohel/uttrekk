using System;

namespace UttrekkFamilia.Models
{
    public partial class FaTiltaksplanevalueringer
    {
        public decimal TtpLoepenr { get; set; }
        public decimal EvaLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public decimal? DokLoepenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public DateTime EvaPlanlagtdato { get; set; }
        public DateTime? EvaUtfoertdato { get; set; }
        public decimal? EvaFraklokken { get; set; }
        public decimal? EvaTilklokken { get; set; }
        public string EvaSted { get; set; }
        public string EvaTilstede { get; set; }
        public string EvaKommentar { get; set; }
        public decimal? EvaDokumentnr { get; set; }
        public DateTime? EvaFerdigdato { get; set; }
        public string EvaEmne { get; set; }
        public DateTime? EvaRegistrertdato { get; set; }
        public DateTime? EvaEndretdato { get; set; }
        public decimal? ArkTiltakevaSystemid { get; set; }
        public DateTime? ArkDato { get; set; }
        public decimal ArkStopp { get; set; }
        public decimal ArkSjekkIVsa { get; set; }
        public string EvaNotat { get; set; }
        public decimal EvaSlettet { get; set; }
        public string EvaBegrSlettet { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaTiltaksplan TtpLoepenrNavigation { get; set; }
    }
}
