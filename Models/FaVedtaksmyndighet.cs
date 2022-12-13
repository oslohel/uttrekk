using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaVedtaksmyndighet
    {
        public FaVedtaksmyndighet()
        {
            FaGenerellsaks = new HashSet<FaGenerellsak>();
            FaSaksjournals = new HashSet<FaSaksjournal>();
        }

        public string MynVedtakstype { get; set; }
        public string MynMyndenavn { get; set; }
        public string MynMyndighetsnivaa { get; set; }
        public DateTime? MynPassivisertdato { get; set; }

        public virtual ICollection<FaGenerellsak> FaGenerellsaks { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournals { get; set; }
    }
}
