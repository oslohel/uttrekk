using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaTiltak
    {
        public FaTiltak()
        {
            FaBetalingsplaners = new HashSet<FaBetalingsplaner>();
            FaEngasjementsavtales = new HashSet<FaEngasjementsavtale>();
            FaTiltaksevaluerings = new HashSet<FaTiltaksevaluering>();
            FaTiltakslinjers = new HashSet<FaTiltakslinjer>();
            FaVurdegenbets = new HashSet<FaVurdegenbet>();
        }

        public decimal TilLoepenr { get; set; }
        public string TttTiltakstype { get; set; }
        public decimal KliLoepenr { get; set; }
        public decimal? TtpLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public decimal? SakAar { get; set; }
        public decimal? SakJournalnr { get; set; }
        public decimal? DokLoepenr { get; set; }
        public string FroType { get; set; }
        public string FroKode1 { get; set; }
        public string FroKode2 { get; set; }
        public string FroKode3 { get; set; }
        public string KttTiltakskode { get; set; }
        public string FttFriTiltakstype { get; set; }
        public DateTime TilFradato { get; set; }
        public DateTime? TilTildato { get; set; }
        public string TilKommentar { get; set; }
        public string TilKategori { get; set; }
        public decimal TilOmsorgsovertakelse { get; set; }
        public decimal TilUtenforhjemmet { get; set; }
        public string TilHovedgrunnavsluttet { get; set; }
        public decimal TilFratattforeldreansvar { get; set; }
        public DateTime? TilIverksattdato { get; set; }
        public DateTime? TilAvsluttetdato { get; set; }
        public decimal TilBortfalt { get; set; }
        public string TilTiltaksmaal { get; set; }
        public string TilMerknadfristoversittelse { get; set; }
        public DateTime? TilRegistrertdato { get; set; }
        public DateTime? TilEndretdato { get; set; }
        public decimal? TilDokumentnr { get; set; }
        public decimal TilEgenbetvurdert { get; set; }
        public decimal TilHovedtiltak { get; set; }
        public DateTime? TilBortfaltdato { get; set; }
        public string TilGmlreferanse { get; set; }
        public decimal TilFlyttet { get; set; }
        public decimal TilUtilsiktet { get; set; }
        public decimal TilHuskeliste { get; set; }
        public string TilEttervern { get; set; }
        public string LovHovedParagraf { get; set; }
        public string LovJmfParagraf1 { get; set; }
        public string LovJmfParagraf2 { get; set; }
        public string SbhInitialer2 { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public string TilPresisering { get; set; }
        public string TilTiltakstypePres { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaFristoversittelser Fro { get; set; }
        public virtual FaFristoversittelser Fro1 { get; set; }
        public virtual FaFristoversittelser FroNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaKontotiltakstype KttTiltakskodeNavigation { get; set; }
        public virtual FaLovtekst LovHovedParagrafNavigation { get; set; }
        public virtual FaLovtekst LovJmfParagraf1Navigation { get; set; }
        public virtual FaLovtekst LovJmfParagraf2Navigation { get; set; }
        public virtual FaSaksjournal Sak { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialer2Navigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaTiltaksplan TtpLoepenrNavigation { get; set; }
        public virtual FaTiltakstyper TttTiltakstypeNavigation { get; set; }
        public virtual FaTiltakGmltype FaTiltakGmltype { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplaners { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtales { get; set; }
        public virtual ICollection<FaTiltaksevaluering> FaTiltaksevaluerings { get; set; }
        public virtual ICollection<FaTiltakslinjer> FaTiltakslinjers { get; set; }
        public virtual ICollection<FaVurdegenbet> FaVurdegenbets { get; set; }
    }
}
