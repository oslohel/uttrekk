using System;

namespace UttrekkFamilia.Models
{
    public partial class FaMeldingerSlettet
    {
        public decimal MelLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public string MesBegrSlettet { get; set; }
        public decimal? MelDokumentnr { get; set; }
        public DateTime? MesRegistrertdato { get; set; }
        public decimal? ArkMeldingSystemid { get; set; }
        public decimal ArkStopp { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
    }
}
