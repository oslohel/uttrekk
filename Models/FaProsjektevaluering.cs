using System;

namespace UttrekkFamilia.Models
{
    public partial class FaProsjektevaluering
    {
        public decimal PreLoepenr { get; set; }
        public decimal ProLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public DateTime PreDato { get; set; }
        public string PreEvaluering { get; set; }

        public virtual FaProsjekt ProLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
