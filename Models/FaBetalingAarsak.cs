using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaBetalingAarsak
    {
        public FaBetalingAarsak()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
            FaEngasjementslinjers = new HashSet<FaEngasjementslinjer>();
        }

        public string UaaIdent { get; set; }
        public string UaaBeskrivelse { get; set; }
        public DateTime? UaaPassivisertdato { get; set; }

        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjers { get; set; }
    }
}
