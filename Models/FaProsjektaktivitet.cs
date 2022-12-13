using System;

namespace UttrekkFamilia.Models
{
    public partial class FaProsjektaktivitet
    {
        public decimal PraLoepenr { get; set; }
        public decimal ProLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string PraBeskrivelse { get; set; }
        public DateTime PraStartdato { get; set; }
        public DateTime? PraSluttdato { get; set; }
        public string PraStatus { get; set; }
        public string PraEksternansvarlig { get; set; }
        public decimal PraInternt { get; set; }

        public virtual FaProsjekt ProLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
