using System;

namespace UttrekkFamilia.Models
{
    public partial class FaLoggNoark
    {
        public decimal LogLoepenr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string LogType { get; set; }
        public DateTime? LogDato { get; set; }
        public string LogBeskrivelse { get; set; }
        public string LogDetaljbeskrivelse { get; set; }
        public decimal? LogAar { get; set; }
        public decimal? LogNr { get; set; }
    }
}
