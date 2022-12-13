using System;

namespace UttrekkFamilia.Models
{
    public partial class FaUndersoekelserSlettet
    {
        public decimal UnsLoepenr { get; set; }
        public decimal MelLoepenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public decimal KliLoepenr { get; set; }
        public string UnsBegrSlettet { get; set; }
        public decimal? UndDokumentnr { get; set; }
        public DateTime? UnsRegistrertdato { get; set; }
        public decimal? UndDokumentnruplan { get; set; }
        public decimal? ArkDokuplanSystemid { get; set; }
        public decimal ArkDokuplanStopp { get; set; }
        public decimal? ArkDoksrapportSystemid { get; set; }
        public decimal ArkDoksrapportStopp { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
    }
}
