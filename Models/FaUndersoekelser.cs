using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaUndersoekelser
    {
        public FaUndersoekelser()
        {
            FaSaksjournals = new HashSet<FaSaksjournal>();
            FaUndersoekelseslinjers = new HashSet<FaUndersoekelseslinjer>();
        }

        public decimal MelLoepenr { get; set; }
        public decimal? PosSendtmelderAar { get; set; }
        public decimal? PosSendtmelderLoepenr { get; set; }
        public decimal? DokLoepenr { get; set; }
        public string FroKode1 { get; set; }
        public decimal? DokUplannr { get; set; }
        public string FroKode2 { get; set; }
        public string FroType { get; set; }
        public string FroKode3 { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhInitialer2 { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public DateTime? UndStartdato { get; set; }
        public string UndBehandlingstid { get; set; }
        public string Und6mndbegrunnelse { get; set; }
        public string UndKonklusjontekst { get; set; }
        public DateTime? UndFerdigdato { get; set; }
        public decimal? UndDokumentnr { get; set; }
        public DateTime? UndHenlagtdato { get; set; }
        public DateTime? UndFerdigdatoUplan { get; set; }
        public string UndKonklusjon { get; set; }
        public DateTime? UndRegistrertdato { get; set; }
        public DateTime? UndFristdato { get; set; }
        public string UndMerknadfristoversittelse { get; set; }
        public DateTime? UndEndretdato { get; set; }
        public decimal? UndDokumentnruplan { get; set; }
        public string UndGmlreferanse { get; set; }
        public decimal UndGjenapnes6mnd { get; set; }
        public decimal? ArkDokuplanSystemid { get; set; }
        public DateTime? ArkDokuplanDato { get; set; }
        public decimal ArkDokuplanStopp { get; set; }
        public decimal ArkDokuplanSjekkIVsa { get; set; }
        public decimal? ArkDoksrapportSystemid { get; set; }
        public DateTime? ArkDoksrapportDato { get; set; }
        public decimal ArkDoksrapportStopp { get; set; }
        public decimal ArkDoksrapportSjekkIVsa { get; set; }
        public decimal UndInnhForeSomatiskSykdom { get; set; }
        public decimal UndInnhForePsykiskProblem { get; set; }
        public decimal UndInnhForeRusmisbruk { get; set; }
        public decimal UndInnhForeManglerFerdigh { get; set; }
        public decimal UndInnhForeKriminalitet { get; set; }
        public decimal UndInnhKonfliktHjemme { get; set; }
        public decimal UndInnhVoldHjemme { get; set; }
        public decimal UndInnhBarnVansjotsel { get; set; }
        public decimal UndInnhBarnFysiskMish { get; set; }
        public decimal UndInnhBarnPsykiskMish { get; set; }
        public decimal UndInnhBarnSeksuOvergr { get; set; }
        public decimal UndInnhBarnMangOmsorgp { get; set; }
        public decimal UndInnhBarnNedsFunk { get; set; }
        public decimal UndInnhBarnPsykProb { get; set; }
        public decimal UndInnhBarnRusmisbruk { get; set; }
        public decimal UndInnhBarnAdferdKrim { get; set; }
        public decimal UndInnhBarnRelasvansker { get; set; }
        public decimal UndInnhAndreForeFami { get; set; }
        public decimal UndInnhAndreBarnSitu { get; set; }
        public string UndInnhPresFamilie { get; set; }
        public string UndInnhPresBarnet { get; set; }
        public decimal UndMeldtnykommune { get; set; }
        public string UndKonklusjonKategori { get; set; }
        public decimal UndGjenapnet { get; set; }
        public string SbhGodkjennSluttrapport { get; set; }
        public decimal UndInnhForeManglBeskyt { get; set; }
        public decimal UndInnhForeManglStimu { get; set; }
        public decimal UndInnhForeTilgjeng { get; set; }
        public decimal UndInnhForeOppfoelgBehov { get; set; }
        public decimal UndInnhBarnForeKonflikt { get; set; }
        public decimal UndInnhBarnAdferd { get; set; }
        public decimal UndInnhBarnKrim { get; set; }
        public decimal UndInnhBarnMennHandel { get; set; }
        public decimal UndInnhPunkt28 { get; set; }
        public decimal UndInnhPunkt29 { get; set; }
        public decimal UndInnhPunkt30 { get; set; }
        public decimal UndInnhPunkt31 { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaDokumenter DokUplannrNavigation { get; set; }
        public virtual FaFristoversittelser Fro { get; set; }
        public virtual FaFristoversittelser Fro1 { get; set; }
        public virtual FaFristoversittelser FroNavigation { get; set; }
        public virtual FaMeldinger MelLoepenrNavigation { get; set; }
        public virtual FaPostjournal PosSendtmelder { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhGodkjennSluttrapportNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialer2Navigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournals { get; set; }
        public virtual ICollection<FaUndersoekelseslinjer> FaUndersoekelseslinjers { get; set; }
    }
}
