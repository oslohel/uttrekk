using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaProsjekttype
    {
        public FaProsjekttype()
        {
            FaProsjekts = new HashSet<FaProsjekt>();
        }

        public string PrtProsjekttype { get; set; }
        public string PrtBeskrivelse { get; set; }
        public DateTime? PrtPassivisertdato { get; set; }

        public virtual ICollection<FaProsjekt> FaProsjekts { get; set; }
    }
}
