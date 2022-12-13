using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaEngasjementsavtale
    {
        public FaEngasjementsavtale()
        {
            FaEngasjementsplans = new HashSet<FaEngasjementsplan>();
        }

        public decimal EngLoepenr { get; set; }
        public decimal KliLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public decimal SakAar { get; set; }
        public decimal SakJournalnr { get; set; }
        public decimal? DokLoepenr { get; set; }
        public string KmgIdent { get; set; }
        public string LoeTrinn { get; set; }
        public decimal TilLoepenr { get; set; }
        public string EngFosterLoeTrinn { get; set; }
        public decimal ForLoepenr { get; set; }
        public DateTime? EngInngaattdato { get; set; }
        public DateTime EngFradato { get; set; }
        public DateTime EngTildato { get; set; }
        public string EngVeileder { get; set; }
        public string EngKontaktperson { get; set; }
        public string EngOppgaver { get; set; }
        public string EngRapportfritekst { get; set; }
        public decimal? EngDagnrtimelister { get; set; }
        public decimal? EngDokumentnr { get; set; }
        public DateTime? EngRegistrertdato { get; set; }
        public DateTime? EngEndretdato { get; set; }
        public string EngLoennGodtype { get; set; }
        public string EngFosterGodtype { get; set; }
        public string EngBesoekGodtype { get; set; }
        public string EngLoennUtdektype { get; set; }
        public string EngFosterUtdektype { get; set; }
        public string EngBesoekUtdektype { get; set; }
        public string EngKmUtdektype { get; set; }
        public string EngPassUtdektype { get; set; }
        public decimal? EngIndgodLoenn { get; set; }
        public decimal? EngIndgodFoster { get; set; }
        public decimal? EngIndgodBesoek { get; set; }
        public decimal? EngIndutgLoenn { get; set; }
        public decimal? EngIndutgFoster { get; set; }
        public decimal? EngIndutgBesoek { get; set; }
        public decimal? EngIndutgBil { get; set; }
        public decimal? EngIndutgPass { get; set; }
        public decimal? EngLoennTimerMnd { get; set; }
        public decimal? EngBilKmPrtur { get; set; }
        public decimal? EngBesoekDoegn { get; set; }
        public decimal? EngPassKmPrtur { get; set; }
        public string EngPassKmgIdent { get; set; }
        public decimal? EngLoennUtgdprosent { get; set; }
        public DateTime? EngKlargjortdato { get; set; }
        public DateTime? EngAvgjortdato { get; set; }
        public DateTime? EngStoppetdato { get; set; }
        public decimal EngOpphevunder { get; set; }
        public string EngInterntStillingsnr { get; set; }
        public string EngStatus { get; set; }
        public string EngGmlreferanse { get; set; }
        public decimal? EngRammebeloep { get; set; }
        public decimal EngGodkjennunder { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public string SbhKlargjortavInitialer { get; set; }
        public string SbhAvgjortavInitialer { get; set; }
        public string SbhStoppetavInitialer { get; set; }
        public decimal EngHuskeliste { get; set; }
        public string EngParkeringUtdektype { get; set; }
        public string EngBompengeUtdektype { get; set; }
        public decimal? EngAntParkeringer { get; set; }
        public decimal? EngSatsParkering { get; set; }
        public decimal? EngAntBompengepasseringer { get; set; }
        public decimal? EngSatsBompenger { get; set; }
        public decimal? EngParkeringsbeloep { get; set; }
        public decimal? EngBompengebeloep { get; set; }
        public decimal? EngGodFrikjoepFoster { get; set; }
        public decimal? EngFosterFaktor { get; set; }
        public decimal? EngBesoekFaktor { get; set; }
        public string EngInterntStillingstekst { get; set; }
        public decimal EngRefusjonskandidat { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaLoennstrinn EngFosterLoeTrinnNavigation { get; set; }
        public virtual FaMedarbeidere ForLoepenrNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaKilometergodtgjoerelse KmgIdentNavigation { get; set; }
        public virtual FaLoennstrinn LoeTrinnNavigation { get; set; }
        public virtual FaSaksjournal Sak { get; set; }
        public virtual FaSaksbehandlere SbhAvgjortavInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhKlargjortavInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhStoppetavInitialerNavigation { get; set; }
        public virtual FaTiltak TilLoepenrNavigation { get; set; }
        public virtual ICollection<FaEngasjementsplan> FaEngasjementsplans { get; set; }
    }
}
