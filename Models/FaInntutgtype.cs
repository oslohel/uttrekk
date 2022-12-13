using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaInntutgtype
    {
        public FaInntutgtype()
        {
            FaInntutgs = new HashSet<FaInntutg>();
        }

        public string IutType { get; set; }
        public string IutKode { get; set; }
        public string IutBeskrivelse { get; set; }
        public decimal IutStandard { get; set; }
        public DateTime? IutPassivisertdato { get; set; }

        public virtual ICollection<FaInntutg> FaInntutgs { get; set; }
    }
}
