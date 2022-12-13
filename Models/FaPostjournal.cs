using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaPostjournal
    {
        public FaPostjournal()
        {
            FaAktivitets = new HashSet<FaAktivitet>();
            FaGenerellsaks = new HashSet<FaGenerellsak>();
            FaKvellos = new HashSet<FaKvello>();
            FaMeldingerPosGjennomdoks = new HashSet<FaMeldinger>();
            FaMeldingerPosMottattbrevs = new HashSet<FaMeldinger>();
            FaMeldingerPosSendtkonkls = new HashSet<FaMeldinger>();
            FaPostjournalkopitils = new HashSet<FaPostjournalkopitil>();
            FaRefusjonerPos = new HashSet<FaRefusjoner>();
            FaRefusjonerPosNavigations = new HashSet<FaRefusjoner>();
            FaRefusjonskravs = new HashSet<FaRefusjonskrav>();
            FaSaksjournals = new HashSet<FaSaksjournal>();
            FaUndersoekelsers = new HashSet<FaUndersoekelser>();
            FaVedleggs = new HashSet<FaVedlegg>();
            FaVurdegenbets = new HashSet<FaVurdegenbet>();
            InversePosAvskriver = new HashSet<FaPostjournal>();
        }

        public decimal PosAar { get; set; }
        public decimal PosLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string DisDistriktskode { get; set; }
        public string KgrGruppeid { get; set; }
        public string SbhInitialer { get; set; }
        public string PnrPostnr { get; set; }
        public decimal? SakAar { get; set; }
        public decimal? SakJournalnr { get; set; }
        public string RodIdent { get; set; }
        public string PosEtternavn { get; set; }
        public decimal PosFleradressater { get; set; }
        public string KkoKodeFag { get; set; }
        public string KkoKodeFelles { get; set; }
        public decimal? DokLoepenr { get; set; }
        public decimal? PosAvskriverAar { get; set; }
        public decimal? PosAvskriverLoepenr { get; set; }
        public string KkoKodeTillegg { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public decimal? SakUnndrAar { get; set; }
        public decimal? SakUnndrJournalnr { get; set; }
        public DateTime? PosRegistrertdato { get; set; }
        public string PosPosttype { get; set; }
        public string PosBrevtype { get; set; }
        public string PosEmne { get; set; }
        public string PosFornavn { get; set; }
        public string PosPostadresse { get; set; }
        public string PosBesoeksadresse { get; set; }
        public decimal PosBesvares { get; set; }
        public decimal PosBesvart { get; set; }
        public decimal PosOpprettsak { get; set; }
        public string PosAbtype { get; set; }
        public string PosDeresref { get; set; }
        public string PosVaarref { get; set; }
        public string PosMerknad { get; set; }
        public string PosOppfoelging { get; set; }
        public DateTime? PosFrist { get; set; }
        public string PosBegrSlettet { get; set; }
        public DateTime? PosOppfyltdato { get; set; }
        public decimal? PosDokumentnr { get; set; }
        public decimal PosUnndrattinnsyn { get; set; }
        public decimal PosSlettet { get; set; }
        public string PosUnndrattbegrunnelse { get; set; }
        public DateTime? PosFerdigdato { get; set; }
        public string PosAtt { get; set; }
        public DateTime? PosEndretdato { get; set; }
        public DateTime? PosSendtMottattDato { get; set; }
        public DateTime PosDato { get; set; }
        public decimal PosUnndrattinnsynIs { get; set; }
        public decimal PosSendKlient { get; set; }
        public string PosGmlreferanse { get; set; }
        public decimal? ArkPostSystemid { get; set; }
        public DateTime? ArkDato { get; set; }
        public decimal ArkStopp { get; set; }
        public decimal ArkSjekkIVsa { get; set; }
        public string PosNotat { get; set; }
        public decimal PosDigitalpost { get; set; }
        public decimal? PosForbLoepenr { get; set; }
        public string PosDigitalpostRef { get; set; }
        public decimal PosTmpDigitalstatus { get; set; }
        public string SbhGodkjennInitialer { get; set; }
        public decimal PosVurderUnndratt { get; set; }
        public bool? TriggerFlagInsertNewDocumnet { get; set; }
        public decimal? PosReconnectedDocnrFlag { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaRoder FaRoder { get; set; }
        public virtual FaKlientgrupper KgrGruppe { get; set; }
        public virtual FaKkoder KkoKodeFagNavigation { get; set; }
        public virtual FaKkoder KkoKodeFellesNavigation { get; set; }
        public virtual FaKkoder KkoKodeTilleggNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual FaPostjournal PosAvskriver { get; set; }
        public virtual FaSaksjournal Sak { get; set; }
        public virtual FaSaksjournal SakUnndr { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhGodkjennInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaAktivitet> FaAktivitets { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsaks { get; set; }
        public virtual ICollection<FaKvello> FaKvellos { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerPosGjennomdoks { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerPosMottattbrevs { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerPosSendtkonkls { get; set; }
        public virtual ICollection<FaPostjournalkopitil> FaPostjournalkopitils { get; set; }
        public virtual ICollection<FaRefusjoner> FaRefusjonerPos { get; set; }
        public virtual ICollection<FaRefusjoner> FaRefusjonerPosNavigations { get; set; }
        public virtual ICollection<FaRefusjonskrav> FaRefusjonskravs { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournals { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelsers { get; set; }
        public virtual ICollection<FaVedlegg> FaVedleggs { get; set; }
        public virtual ICollection<FaVurdegenbet> FaVurdegenbets { get; set; }
        public virtual ICollection<FaPostjournal> InversePosAvskriver { get; set; }
    }
}
