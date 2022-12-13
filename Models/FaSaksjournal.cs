using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaSaksjournal
    {
        public FaSaksjournal()
        {
            FaBetalingsplaners = new HashSet<FaBetalingsplaner>();
            FaEngasjementsavtales = new HashSet<FaEngasjementsavtale>();
            FaHuskelapps = new HashSet<FaHuskelapp>();
            FaJournals = new HashSet<FaJournal>();
            FaPostjournalSakUnndrs = new HashSet<FaPostjournal>();
            FaPostjournalSaks = new HashSet<FaPostjournal>();
            FaRekvisisjoners = new HashSet<FaRekvisisjoner>();
            FaTiltaks = new HashSet<FaTiltak>();
            InverseSakErstattetav = new HashSet<FaSaksjournal>();
        }

        public decimal SakAar { get; set; }
        public decimal SakJournalnr { get; set; }
        public string MynVedtakstype { get; set; }
        public decimal KliLoepenr { get; set; }
        public decimal? SakErstattetavAar { get; set; }
        public decimal? SakErstattetavJournalnr { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhAvgjortavInitialer { get; set; }
        public string SbhEndretav { get; set; }
        public string SbhRegistrertav { get; set; }
        public string LovHovedParagraf { get; set; }
        public string LovJmfParagraf1 { get; set; }
        public decimal? DokLoepenr { get; set; }
        public decimal? PosAar { get; set; }
        public decimal? PosLoepenr { get; set; }
        public decimal? MelLoepenr { get; set; }
        public DateTime SakDato { get; set; }
        public string SakStatus { get; set; }
        public DateTime? SakAvgjortdato { get; set; }
        public string SakEmne { get; set; }
        public string SakKlageaarsak { get; set; }
        public DateTime? SakIverksattdato { get; set; }
        public DateTime? SakVarighettil { get; set; }
        public decimal? SakUtbetalingsramme { get; set; }
        public DateTime? SakSendtadvokat { get; set; }
        public DateTime? SakSendtfylke { get; set; }
        public DateTime? SakVideresendtdato { get; set; }
        public string SakAvgjortetat { get; set; }
        public DateTime? SakKlargjortdato { get; set; }
        public DateTime? SakRegistrertdato { get; set; }
        public DateTime? SakEndretdato { get; set; }
        public decimal? SakDokumentnr { get; set; }
        public DateTime? SakOpphevetdato { get; set; }
        public string SakOpphevetaarsak { get; set; }
        public DateTime? SakBortfaltdato { get; set; }
        public string SakBortfaltaarsak { get; set; }
        public string SakGodkjenningstype { get; set; }
        public decimal SakOpphevunder { get; set; }
        public string SakMerknaderAvslag { get; set; }
        public decimal SakHovedvedtak { get; set; }
        public string SakBehFylkesnemnda { get; set; }
        public DateTime? SakSlutningdato { get; set; }
        public string SakGmlreferanse { get; set; }
        public string LovJmfParagraf2 { get; set; }
        public string SbhOpphevetInitialer { get; set; }
        public string SbhBortfaltInitialer { get; set; }
        public string SakStatusKategori { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaLovtekst LovHovedParagrafNavigation { get; set; }
        public virtual FaLovtekst LovJmfParagraf1Navigation { get; set; }
        public virtual FaLovtekst LovJmfParagraf2Navigation { get; set; }
        public virtual FaUndersoekelser MelLoepenrNavigation { get; set; }
        public virtual FaVedtaksmyndighet MynVedtakstypeNavigation { get; set; }
        public virtual FaPostjournal Pos { get; set; }
        public virtual FaSaksjournal SakErstattetav { get; set; }
        public virtual FaSaksbehandlere SbhAvgjortavInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhBortfaltInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhOpphevetInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplaners { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtales { get; set; }
        public virtual ICollection<FaHuskelapp> FaHuskelapps { get; set; }
        public virtual ICollection<FaJournal> FaJournals { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalSakUnndrs { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalSaks { get; set; }
        public virtual ICollection<FaRekvisisjoner> FaRekvisisjoners { get; set; }
        public virtual ICollection<FaTiltak> FaTiltaks { get; set; }
        public virtual ICollection<FaSaksjournal> InverseSakErstattetav { get; set; }
    }
}
