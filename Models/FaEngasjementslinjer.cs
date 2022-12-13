using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaEngasjementslinjer
    {
        public FaEngasjementslinjer()
        {
            InverseEnlTilbakefompostertLoepenrNavigation = new HashSet<FaEngasjementslinjer>();
        }

        public decimal EnlLoepenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public string UaaIdent { get; set; }
        public decimal EnpLoepenr { get; set; }
        public string DisDistriktskode { get; set; }
        public string EnlBetalingstype { get; set; }
        public decimal? EnlPlanferiepenger { get; set; }
        public decimal? EnlPlanarbgiveravg { get; set; }
        public decimal? EnlPlanarbavgferiep { get; set; }
        public string EnlBalanse { get; set; }
        public string SbhAnvistav { get; set; }
        public string SbhEndretav { get; set; }
        public string KtpNoekkel1 { get; set; }
        public string KtnKontonummer1 { get; set; }
        public string KtpNoekkel2 { get; set; }
        public string KtnKontonummer2 { get; set; }
        public string KtpNoekkel3 { get; set; }
        public string KtnKontonummer3 { get; set; }
        public string KtpNoekkel4 { get; set; }
        public string KtnKontonummer4 { get; set; }
        public string KtpNoekkel5 { get; set; }
        public string KtnKontonummer5 { get; set; }
        public string KtpNoekkel6 { get; set; }
        public string KtnKontonummer6 { get; set; }
        public decimal? EnlTilbakefompostertLoepenr { get; set; }
        public string VikType { get; set; }
        public string EnlAnvistmaate { get; set; }
        public DateTime? EnlAnvistdato { get; set; }
        public string EnlBilagsserie { get; set; }
        public decimal? EnlAnvistaar { get; set; }
        public decimal? EnlAnvistnr { get; set; }
        public decimal EnlSkrevet { get; set; }
        public decimal? EnlBeloep { get; set; }
        public decimal? EnlPlanlagtbeloep { get; set; }
        public DateTime EnlForfallsdato { get; set; }
        public DateTime EnlPlanlagtforfallsdato { get; set; }
        public decimal? EnlAntall { get; set; }
        public decimal? EnlPlanlagtantall { get; set; }
        public decimal? EnlSats { get; set; }
        public decimal? EnlPlanlagtsats { get; set; }
        public string EnlBeskrivelse { get; set; }
        public DateTime? EnlRegistrertdato { get; set; }
        public DateTime? EnlEndretdato { get; set; }
        public decimal? EnlPeriodeaar { get; set; }
        public decimal? EnlPeriodemnd { get; set; }
        public decimal? EnlTilbakefoertbeloep { get; set; }
        public decimal? EnlFeriepenger { get; set; }
        public decimal? EnlArbgiveravgift { get; set; }
        public decimal? EnlArbavgferiepenger { get; set; }
        public decimal EnlVilkaaroppfylt { get; set; }
        public string EnlKontonrmottaker { get; set; }
        public string EnlMottakerAdresse { get; set; }
        public string EnlMottakerPostnr { get; set; }
        public string EnlMottakerPoststed { get; set; }
        public decimal? EnlLoennsfilnr { get; set; }
        public DateTime? EnlLoennsfildato { get; set; }
        public string EnlSatstype { get; set; }
        public string EnlSatsident { get; set; }
        public string EnlBegrTilbakefoert { get; set; }
        public decimal? EnlNyettertbf { get; set; }
        public decimal EnlAnvisPaagaar { get; set; }
        public string EnlFeilkode { get; set; }
        public string EnlFeilbeskrivelse { get; set; }
        public decimal? EnlTellnn { get; set; }
        public decimal? EnlForsterkFaktor { get; set; }
        public string EnlStatus { get; set; }
        public decimal? RerLoepenr { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual FaEngasjementslinjer EnlTilbakefompostertLoepenrNavigation { get; set; }
        public virtual FaEngasjementsplan EnpLoepenrNavigation { get; set; }
        public virtual FaKontoer Kt { get; set; }
        public virtual FaKontoer Kt1 { get; set; }
        public virtual FaKontoer Kt2 { get; set; }
        public virtual FaKontoer Kt3 { get; set; }
        public virtual FaKontoer Kt4 { get; set; }
        public virtual FaKontoer KtNavigation { get; set; }
        public virtual FaRefusjonskrav RerLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhAnvistavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaBetalingAarsak UaaIdentNavigation { get; set; }
        public virtual FaUtbetalingsvilkaar VikTypeNavigation { get; set; }
        public virtual ICollection<FaEngasjementslinjer> InverseEnlTilbakefompostertLoepenrNavigation { get; set; }
    }
}
