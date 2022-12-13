using System;

namespace UttrekkFamilia.Models
{
    public partial class FaTiltakslinjer
    {
        public decimal TtlLoepenr { get; set; }
        public decimal TilLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public decimal KliLoepenr { get; set; }
        public string TtlBeskrivelse { get; set; }
        public DateTime? TtlFrist { get; set; }
        public DateTime? TtlOppfyltdato { get; set; }
        public decimal TtlInternt { get; set; }
        public string TtlEksternansvarlig { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaTiltak TilLoepenrNavigation { get; set; }
    }
}
