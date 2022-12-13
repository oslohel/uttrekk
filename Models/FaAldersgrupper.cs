using System;

namespace UttrekkFamilia.Models
{
    public partial class FaAldersgrupper
    {
        public string AldIdent { get; set; }
        public string AldBeskrivelse { get; set; }
        public decimal AldFraaar { get; set; }
        public decimal? AldTilaar { get; set; }
        public DateTime? AldPassivisertdato { get; set; }
    }
}
