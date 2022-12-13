using System;

namespace UttrekkFamilia.Models
{
    public partial class FaStatFylkesmannKtrskjema
    {
        public DateTime SfkPeriodeFradato { get; set; }
        public string DisDistriktskode { get; set; }
        public decimal SfkSkjemanr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public decimal? MelLoepenr { get; set; }
        public decimal? SakAar { get; set; }
        public decimal? SakJournalnr { get; set; }
        public DateTime? SfkP3MelMottattdato { get; set; }
        public DateTime? SfkP4MelKonkludertdato { get; set; }
        public string SfkP4MelKonklusjon { get; set; }
        public DateTime? SfkP5UndIverksattdato { get; set; }
        public DateTime? SfkP6UndHenlagtdato { get; set; }
        public DateTime? SfkP7SakAvgjortdato { get; set; }
        public DateTime? SfkP8SakSendtfylkedato { get; set; }
        public string SfkP9MelFristoversittelse { get; set; }
        public string SfkP10UndFristutvidelse { get; set; }
        public string SfkP11UndFristoversittelse { get; set; }
        public string SfkP12Henlagt1 { get; set; }
        public string SfkP12Henlagt2 { get; set; }
        public string SfkP12Henlagt3 { get; set; }
        public decimal? SfkTotAntall { get; set; }
        public decimal? SfkUtvidetUstid { get; set; }
        public decimal? SfkOversittMTot { get; set; }
        public decimal? SfkOversittM15 { get; set; }
        public decimal? SfkOversittM610 { get; set; }
        public decimal? SfkOversittM1120 { get; set; }
        public decimal? SfkOversittM20 { get; set; }
        public decimal? SfkOversittUTot { get; set; }
        public decimal? SfkOversittU15 { get; set; }
        public decimal? SfkOversittU615 { get; set; }
        public decimal? SfkOversittU1630 { get; set; }
        public decimal? SfkOversittU3160 { get; set; }
        public decimal? SfkOversittU6190 { get; set; }
        public decimal? SfkOversittU90 { get; set; }
    }
}
