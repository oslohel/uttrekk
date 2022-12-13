using System;

namespace UttrekkFamilia.Models
{
    public partial class FaTekstmaltyper
    {
        public string TmtGruppe { get; set; }
        public string TmtType { get; set; }
        public string TmtTypebeskrivelse { get; set; }
        public string TmtGruppebeskrivelse { get; set; }
        public DateTime? TmtPassivisertdato { get; set; }
    }
}
