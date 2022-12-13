using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaLogg
    {
        public FaLogg()
        {
            FaLogglinjers = new HashSet<FaLogglinjer>();
        }

        public decimal LogLoepenr { get; set; }
        public DateTime LogDatotid { get; set; }
        public string LogSaksbehandler { get; set; }
        public string LogSbhFornavn { get; set; }
        public string LogSbhEtternavn { get; set; }
        public decimal? LogKlientnr { get; set; }
        public string LogKlientfornavn { get; set; }
        public string LogKlientetternavn { get; set; }
        public string LogKategori { get; set; }
        public string LogType { get; set; }

        public virtual ICollection<FaLogglinjer> FaLogglinjers { get; set; }
    }
}
