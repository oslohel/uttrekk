using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaSakstype
    {
        public FaSakstype()
        {
            FaBehandlingsmals = new HashSet<FaBehandlingsmal>();
            FaGenerellsaks = new HashSet<FaGenerellsak>();
        }

        public string SatSakstype { get; set; }
        public string SatBeskrivelse { get; set; }
        public DateTime? SatPassivisertdato { get; set; }
        public decimal SatGodkjennFhEgen { get; set; }
        public decimal SatGodkjennFhAnnen { get; set; }

        public virtual ICollection<FaBehandlingsmal> FaBehandlingsmals { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsaks { get; set; }
    }
}
