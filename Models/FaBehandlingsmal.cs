using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaBehandlingsmal
    {
        public FaBehandlingsmal()
        {
            FaBehandlingsmallinjes = new HashSet<FaBehandlingsmallinje>();
        }

        public decimal BemLoepenr { get; set; }
        public string SatSakstype { get; set; }
        public string BemNavn { get; set; }
        public string BemBeskrivelse { get; set; }
        public DateTime? BemPassivisertdato { get; set; }
        public string BemBhmaate { get; set; }

        public virtual FaSakstype SatSakstypeNavigation { get; set; }
        public virtual ICollection<FaBehandlingsmallinje> FaBehandlingsmallinjes { get; set; }
    }
}
