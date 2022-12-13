using System;

namespace UttrekkFamilia.Models
{
    public partial class FaKvelloTiltak
    {
        public decimal KvtLoepenr { get; set; }
        public decimal KveLoepenr { get; set; }
        public decimal? TilLoepenr { get; set; }
        public decimal? KvtType { get; set; }
        public DateTime? KvtFradato { get; set; }
        public DateTime? KvtTildato { get; set; }
        public string KvtTiltakstype { get; set; }
        public string KvtGjennomfoertav { get; set; }
        public string KvtBeskrivelse { get; set; }

        public virtual FaKvello KveLoepenrNavigation { get; set; }
    }
}
