using System;

namespace UttrekkFamilia.Models
{
    public partial class FaTiltaksevaluering
    {
        public decimal TevLoepenr { get; set; }
        public decimal TilLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public DateTime TevPlanlagtdato { get; set; }
        public DateTime? TevUtfoertdato { get; set; }
        public decimal? TevFraklokken { get; set; }
        public decimal? TevTilklokken { get; set; }
        public string TevSted { get; set; }
        public string TevTilstede { get; set; }
        public string TevKommentar { get; set; }
        public string TevEvalueringskode { get; set; }

        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaTiltak TilLoepenrNavigation { get; set; }
    }
}
