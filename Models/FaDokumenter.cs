using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaDokumenter
    {
        public FaDokumenter()
        {
            FaEngasjementsavtales = new HashSet<FaEngasjementsavtale>();
            FaJournals = new HashSet<FaJournal>();
            FaMeldingers = new HashSet<FaMeldinger>();
            FaPostjournals = new HashSet<FaPostjournal>();
            FaRekvisisjoners = new HashSet<FaRekvisisjoner>();
            FaSaksjournals = new HashSet<FaSaksjournal>();
            FaSsbStatistikks = new HashSet<FaSsbStatistikk>();
            FaTekstmalers = new HashSet<FaTekstmaler>();
            FaTiltaks = new HashSet<FaTiltak>();
            FaTiltaksplanevalueringers = new HashSet<FaTiltaksplanevalueringer>();
            FaTiltaksplans = new HashSet<FaTiltaksplan>();
            FaUndersoekelserDokLoepenrNavigations = new HashSet<FaUndersoekelser>();
            FaUndersoekelserDokUplannrNavigations = new HashSet<FaUndersoekelser>();
            FaVedleggs = new HashSet<FaVedlegg>();
        }

        public decimal DokLoepenr { get; set; }
        public string SbhEndretav { get; set; }
        public string SbhUtsjekketavInitialer { get; set; }
        public string VinUtvnavn { get; set; }
        public string DokType { get; set; }
        public byte[] DokDokument { get; set; }
        public decimal DokProdusert { get; set; }
        public decimal DokLaast { get; set; }
        public string DokUtsjekketMaskin { get; set; }
        public string DokUtsjekketFilnavn { get; set; }
        public DateTime? DokUtsjekketDato { get; set; }
        public DateTime? DokEndretdato { get; set; }
        public string DokMimetype { get; set; }
        public int? DokPageorientation { get; set; }
        public Guid? DokLagetavtekstmalerId { get; set; }
        public string DokImportTag { get; set; }
        public string DokMetaData { get; set; }
        public Guid? DokSvarinnRef { get; set; }
        public string CheckOutSessionId { get; set; }
        public decimal? ArkPdfaStatus { get; set; }
        public DateTime? ArkPdfaStatusDate { get; set; }
        public string ArkPdfaStatusMessage { get; set; }
        public decimal? ArkPdfaStatusExceptionCount { get; set; }

        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhUtsjekketavInitialerNavigation { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtales { get; set; }
        public virtual ICollection<FaJournal> FaJournals { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingers { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournals { get; set; }
        public virtual ICollection<FaRekvisisjoner> FaRekvisisjoners { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournals { get; set; }
        public virtual ICollection<FaSsbStatistikk> FaSsbStatistikks { get; set; }
        public virtual ICollection<FaTekstmaler> FaTekstmalers { get; set; }
        public virtual ICollection<FaTiltak> FaTiltaks { get; set; }
        public virtual ICollection<FaTiltaksplanevalueringer> FaTiltaksplanevalueringers { get; set; }
        public virtual ICollection<FaTiltaksplan> FaTiltaksplans { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserDokLoepenrNavigations { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserDokUplannrNavigations { get; set; }
        public virtual ICollection<FaVedlegg> FaVedleggs { get; set; }
    }
}
