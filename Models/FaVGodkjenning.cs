using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVGodkjenning
    {
        public decimal? SakAar { get; set; }
        public decimal? SakJournalnr { get; set; }
        public decimal EngLoepenr { get; set; }
        public decimal PlanLoepenr { get; set; }
        public decimal? TilLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public decimal? ForLoepenr { get; set; }
        public decimal UtbAnvisttilklient { get; set; }
        public string BktType { get; set; }
        public string BktKode { get; set; }
        public string SbhKlargjortavInitialer { get; set; }
        public string SbhAvgjortavInitialer { get; set; }
        public string SbhStoppetavInitialer { get; set; }
        public DateTime PlanVarighetfra { get; set; }
        public DateTime? PlanVarighettil { get; set; }
        public string PlanBeskrivelse { get; set; }
        public string UtbAarsaker { get; set; }
        public string EnpOppgaver { get; set; }
        public DateTime? PlanKlargjortdato { get; set; }
        public DateTime? PlanAvgjortdato { get; set; }
        public DateTime? PlanStoppetdato { get; set; }
        public string PlanStatus { get; set; }
        public string PlanFormaal { get; set; }
        public string EnpPlantype { get; set; }
        public decimal LinjeLoepenr { get; set; }
        public string LinjeBetalingstype { get; set; }
        public string LinjeSbhAnvistav { get; set; }
        public DateTime? LinjeAnvistdato { get; set; }
        public string LinjeAnvistmaate { get; set; }
        public string LinjeBilagsserie { get; set; }
        public decimal? LinjeAnvistaar { get; set; }
        public decimal? LinjeAnvistnr { get; set; }
        public DateTime LinjeForfallsdato { get; set; }
        public decimal? LinjeBeloep { get; set; }
        public decimal LinjePlanlagtbeloep { get; set; }
        public decimal LinjeTilbakefoertbeloep { get; set; }
        public decimal? EnlFeriepenger { get; set; }
        public decimal? EnlArbgiveravgift { get; set; }
        public decimal? EnlBeloep { get; set; }
        public decimal? EnlArbavgferiepenger { get; set; }
        public decimal? LinjeTbfompostertLoepenr { get; set; }
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
        public decimal? UtbRekAar { get; set; }
        public decimal? UtbRekLoepenr { get; set; }
        public decimal LinjeSkrevet { get; set; }
        public string LinjeKontonrmottaker { get; set; }
        public decimal LinjeVilkaaroppfylt { get; set; }
        public decimal? LinjeTbfbeloep { get; set; }
        public decimal? LinjePeriodeaar { get; set; }
        public decimal? LinjePeriodemnd { get; set; }
        public string DisDistriktskode { get; set; }
    }
}
