using System;

namespace UttrekkFamilia.Models
{
    public partial class FaSsbbegrunnelser
    {
        public string SbbIdent { get; set; }
        public decimal? SbbKode { get; set; }
        public string SbbBeskrivelse { get; set; }
        public DateTime? SbbPassivisertdato { get; set; }
    }
}
