using System;

namespace UttrekkFamilia.Models
{
    public partial class FaJournal
    {
        public decimal JouLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public decimal? DokLoepenr { get; set; }
        public string JotIdent { get; set; }
        public decimal? SakUnndrAar { get; set; }
        public decimal? SakUnndrJournalnr { get; set; }
        public decimal? ProLoepenr { get; set; }
        public decimal? ForLoepenr { get; set; }
        public DateTime JouDatonotat { get; set; }
        public string JouEmne { get; set; }
        public string JouNotat { get; set; }
        public DateTime? JouFerdigdato { get; set; }
        public string JouOppfoelging { get; set; }
        public DateTime? JouFrist { get; set; }
        public DateTime? JouOppfyltdato { get; set; }
        public DateTime? JouRegistrertdato { get; set; }
        public DateTime? JouEndretdato { get; set; }
        public decimal JouUnndrattinnsyn { get; set; }
        public string JouUnndrattbegrunnelse { get; set; }
        public decimal JouEttdokument { get; set; }
        public decimal? JouDokumentnr { get; set; }
        public decimal? JouTimerforbruk { get; set; }
        public decimal? JouMinutterforbruk { get; set; }
        public decimal JouUnndrattinnsynIs { get; set; }
        public string JouBegrSlettet { get; set; }
        public decimal JouSlettet { get; set; }
        public string JouGmlreferanse { get; set; }
        public decimal? ArkJourSystemid { get; set; }
        public DateTime? ArkDato { get; set; }
        public decimal ArkStopp { get; set; }
        public decimal ArkSjekkIVsa { get; set; }
        public decimal JouVurderUnndratt { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
        public virtual FaJournaltype JotIdentNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaProsjekt ProLoepenrNavigation { get; set; }
        public virtual FaSaksjournal SakUnndr { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
    }
}
