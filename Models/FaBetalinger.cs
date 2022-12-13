using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaBetalinger
    {
        public FaBetalinger()
        {
            InverseUtbTilbakefompostertLoepenrNavigation = new HashSet<FaBetalinger>();
        }

        public decimal UtbLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string SbhAnvistav { get; set; }
        public string SbhRegistrertav { get; set; }
        public string DisDistriktskode { get; set; }
        public string SbhEndretav { get; set; }
        public decimal? UtpLoepenr { get; set; }
        public decimal? ForLoepenr { get; set; }
        public decimal? UtbTilbakefompostertLoepenr { get; set; }
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
        public string VikType { get; set; }
        public decimal? RekAar { get; set; }
        public decimal? RekLoepenr { get; set; }
        public decimal? ProLoepenr { get; set; }
        public string BktType { get; set; }
        public string BktKode { get; set; }
        public string UaaIdent { get; set; }
        public string UtbBetalingstype { get; set; }
        public DateTime? UtbRegistrertdato { get; set; }
        public DateTime? UtbEndretdato { get; set; }
        public DateTime? UtbAnvistdato { get; set; }
        public string UtbAnvistmaate { get; set; }
        public string UtbBilagsserie { get; set; }
        public decimal? UtbAnvistaar { get; set; }
        public decimal? UtbAnvistnr { get; set; }
        public decimal? UtbBeloep { get; set; }
        public decimal UtbSkrevet { get; set; }
        public decimal? UtbPlanlagtbeloep { get; set; }
        public DateTime UtbForfallsdato { get; set; }
        public string UtbUBetalingsmaate { get; set; }
        public string UtbUKontonrmottaker { get; set; }
        public decimal UtbUVilkaaroppfylt { get; set; }
        public string UtbUKid { get; set; }
        public string UtbUMeldingmottaker { get; set; }
        public decimal? UtbURemitteringsnr { get; set; }
        public decimal? UtbTilbakefoertbeloep { get; set; }
        public decimal? UtbOverfoertregnskap { get; set; }
        public decimal? UtbPeriodeaar { get; set; }
        public decimal? UtbPeriodemnd { get; set; }
        public decimal UtbAnvisttilklient { get; set; }
        public string UtbUAvregningsnr { get; set; }
        public string UtbBalanse { get; set; }
        public string UtbUMottakerAdresse { get; set; }
        public string UtbUMottakerPostnr { get; set; }
        public string UtbUMottakerPoststed { get; set; }
        public DateTime? UtbFakturadato { get; set; }
        public DateTime? UtbURemitteringsdato { get; set; }
        public DateTime? UtbURegnskapsfildato { get; set; }
        public string UtbBegrTilbakefoert { get; set; }
        public decimal UtbNyettertbf { get; set; }
        public string UtbGmlreferanse { get; set; }
        public decimal UtbAnvisPaagaar { get; set; }
        public string UtbMvakode { get; set; }
        public string UtbStatus { get; set; }
        public string UtbRetkode { get; set; }
        public decimal? UtbTelrgn { get; set; }
        public decimal? UtbTelrem { get; set; }
        public decimal? UtbEksternReferanse { get; set; }
        public decimal? RerLoepenr { get; set; }
        public string KttTiltakskode { get; set; }
        public bool UtbDisabledRegnskap { get; set; }

        public virtual FaBetalingskategorier Bkt { get; set; }
        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaKontoer Kt { get; set; }
        public virtual FaKontoer Kt1 { get; set; }
        public virtual FaKontoer Kt2 { get; set; }
        public virtual FaKontoer Kt3 { get; set; }
        public virtual FaKontoer Kt4 { get; set; }
        public virtual FaKontoer KtNavigation { get; set; }
        public virtual FaProsjekt ProLoepenrNavigation { get; set; }
        public virtual FaRekvisisjoner Rek { get; set; }
        public virtual FaRefusjonskrav RerLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhAnvistavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaBetalingAarsak UaaIdentNavigation { get; set; }
        public virtual FaBetalinger UtbTilbakefompostertLoepenrNavigation { get; set; }
        public virtual FaBetalingsplaner UtpLoepenrNavigation { get; set; }
        public virtual FaUtbetalingsvilkaar VikTypeNavigation { get; set; }
        public virtual ICollection<FaBetalinger> InverseUtbTilbakefompostertLoepenrNavigation { get; set; }
    }
}
