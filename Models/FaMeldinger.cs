using System;

namespace UttrekkFamilia.Models
{
    public partial class FaMeldinger
    {
        public decimal MelLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhMottattav { get; set; }
        public string FroType { get; set; }
        public string DisDistriktskode { get; set; }
        public string RodIdent { get; set; }
        public decimal? DokLoepenr { get; set; }
        public DateTime MelMottattdato { get; set; }
        public string MelMeldingstype { get; set; }
        public string MelMottattmaate { get; set; }
        public DateTime? MelBehandlesinnen { get; set; }
        public string MelMerknadfristoversittelse { get; set; }
        public decimal? MelDokumentnr { get; set; }
        public decimal MelAnonymmelder { get; set; }
        public decimal MelKonklusjontilmelder { get; set; }
        public string MelKonklTilmelderbegrunnelse { get; set; }
        public string MelSvarmaatemelder { get; set; }
        public string MelMelderetternavn { get; set; }
        public string MelMelderfornavn { get; set; }
        public string MelMelderadresse { get; set; }
        public string MelMeldertelefonprivat { get; set; }
        public string MelMeldertelefonarbeid { get; set; }
        public string MelMeldertelefonmobil { get; set; }
        public decimal MelTattoppForeldre { get; set; }
        public decimal MelTattoppAndre { get; set; }
        public string MelTattoppHvem { get; set; }
        public string MelTidligerekjennskap { get; set; }
        public string MelHypotese { get; set; }
        public DateTime? MelPaabegyntdato { get; set; }
        public DateTime? MelAvsluttetgjennomgang { get; set; }
        public string MelKonklusjon { get; set; }
        public DateTime? MelRegistrertdato { get; set; }
        public DateTime? MelEndretdato { get; set; }
        public string MelBarnopplFritekst { get; set; }
        public string MelMoropplFritekst { get; set; }
        public string MelFaropplFritekst { get; set; }
        public decimal MelInnhOmsorg { get; set; }
        public decimal MelInnhForhold { get; set; }
        public decimal MelInnhAdferd { get; set; }
        public decimal MelInnhAnnet { get; set; }
        public decimal MelMeldtBarnet { get; set; }
        public decimal MelMeldtForeldre { get; set; }
        public decimal MelMeldtFamilie { get; set; }
        public decimal MelMeldtNaboer { get; set; }
        public decimal MelMeldtSosialkt { get; set; }
        public decimal MelMeldtBarnevt { get; set; }
        public decimal MelMeldtBarnevvakt { get; set; }
        public decimal MelMeldtBarnehage { get; set; }
        public decimal MelMeldtHelsestasjon { get; set; }
        public decimal MelMeldtLege { get; set; }
        public decimal MelMeldtSkole { get; set; }
        public decimal MelMeldtPedPpt { get; set; }
        public decimal MelMeldtPoliti { get; set; }
        public decimal MelMeldtBup { get; set; }
        public decimal MelMeldtAndre { get; set; }
        public string MelMelding { get; set; }
        public decimal MelEttdokument { get; set; }
        public decimal MelKtrlskjemaLevertiaby { get; set; }
        public string MelGmlreferanse { get; set; }
        public decimal MelMeldtUdi { get; set; }
        public decimal MelMeldtKrisesenter { get; set; }
        public decimal MelMeldtUtekontakt { get; set; }
        public decimal MelMeldtFrivillige { get; set; }
        public decimal MelMeldtOffentlig { get; set; }
        public decimal MelDokProdusert { get; set; }
        public decimal MelDokFlett { get; set; }
        public decimal MelHenlagtPgaUtenforbvl { get; set; }
        public decimal MelHenlagtAnnenInstans { get; set; }
        public decimal MelHenlagtTilAnnenBv { get; set; }
        public string MelFornavn { get; set; }
        public string MelEtternavn { get; set; }
        public DateTime? MelFoedselsdato { get; set; }
        public decimal? MelPersonnr { get; set; }
        public string SbhEndretav { get; set; }
        public string SbhRegistrertav { get; set; }
        public string PnrMelderPostnr { get; set; }
        public decimal? PosMottattbrevAar { get; set; }
        public decimal? PosMottattbrevLoepenr { get; set; }
        public decimal? PosSendtkonklAar { get; set; }
        public decimal? PosSendtkonklLoepenr { get; set; }
        public string FroKode1 { get; set; }
        public string FroKode2 { get; set; }
        public string FroKode3 { get; set; }
        public decimal? ArkMeldingSystemid { get; set; }
        public DateTime? ArkDato { get; set; }
        public decimal ArkStopp { get; set; }
        public decimal ArkSjekkIVsa { get; set; }
        public decimal? ArkMappeSystemid { get; set; }
        public DateTime? ArkMappeDato { get; set; }
        public decimal ArkMappeNavnEndret { get; set; }
        public decimal ArkMappeFnrEndret { get; set; }
        public decimal ArkMappeStopp { get; set; }
        public decimal MelMeldtPsykiskHelseBarn { get; set; }
        public decimal MelMeldtPsykiskHelseVoksne { get; set; }
        public decimal MelMeldtFamilievernkontor { get; set; }
        public decimal MelMeldtTjenesteInstans { get; set; }
        public string MelMeldtPresAndreOffent { get; set; }
        public string MelMeldtPresAndre { get; set; }
        public decimal MelInnhForeSomatiskSykdom { get; set; }
        public decimal MelInnhForePsykiskProblem { get; set; }
        public decimal MelInnhForeRusmisbruk { get; set; }
        public decimal MelInnhForeManglerFerdigh { get; set; }
        public decimal MelInnhForeKriminalitet { get; set; }
        public decimal MelInnhKonfliktHjemme { get; set; }
        public decimal MelInnhVoldHjemme { get; set; }
        public decimal MelInnhBarnVansjotsel { get; set; }
        public decimal MelInnhBarnFysiskMish { get; set; }
        public decimal MelInnhBarnPsykiskMish { get; set; }
        public decimal MelInnhBarnSeksuOvergr { get; set; }
        public decimal MelInnhBarnMangOmsorgp { get; set; }
        public decimal MelInnhBarnNedsFunk { get; set; }
        public decimal MelInnhBarnPsykProb { get; set; }
        public decimal MelInnhBarnRusmisbruk { get; set; }
        public decimal MelInnhBarnAdferdKrim { get; set; }
        public decimal MelInnhBarnRelasvansker { get; set; }
        public decimal MelInnhAndreForeFami { get; set; }
        public decimal MelInnhAndreBarnSitu { get; set; }
        public string MelInnhPresFamilie { get; set; }
        public string MelInnhPresBarnet { get; set; }
        public DateTime? MelGjennomgangsdokferdig { get; set; }
        public string MelStatusKategori { get; set; }
        public string MelStatusConclusion { get; set; }
        public decimal ArkDocumentSjekkIVsa { get; set; }
        public decimal? PosGjennomdokAar { get; set; }
        public decimal? PosGjennomdokLoepenr { get; set; }
        public decimal? MelInnhOffentMelder { get; set; }
        public decimal MelInnhForeManglBeskyt { get; set; }
        public decimal MelInnhForeManglStimu { get; set; }
        public decimal MelInnhForeTilgjeng { get; set; }
        public decimal MelInnhForeOppfoelgBehov { get; set; }
        public decimal MelInnhBarnForeKonflikt { get; set; }
        public decimal MelInnhBarnAdferd { get; set; }
        public decimal MelInnhBarnKrim { get; set; }
        public decimal MelInnhBarnMennHandel { get; set; }
        public decimal MelInnhPunkt28 { get; set; }
        public decimal MelInnhPunkt29 { get; set; }
        public decimal MelInnhPunkt30 { get; set; }
        public decimal MelInnhPunkt31 { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaRoder FaRoder { get; set; }
        public virtual FaFristoversittelser Fro { get; set; }
        public virtual FaFristoversittelser Fro1 { get; set; }
        public virtual FaFristoversittelser FroNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaPostadresser PnrMelderPostnrNavigation { get; set; }
        public virtual FaPostjournal PosGjennomdok { get; set; }
        public virtual FaPostjournal PosMottattbrev { get; set; }
        public virtual FaPostjournal PosSendtkonkl { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhMottattavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaUndersoekelser FaUndersoekelser { get; set; }
    }
}
