using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaForbindelsestyper
    {
        public FaForbindelsestyper()
        {
            ForLoepenrs = new HashSet<FaForbindelser>();
        }

        public string FotIdent { get; set; }
        public string FotBeskrivelse { get; set; }
        public decimal FotPersonrelatert { get; set; }
        public DateTime? FotPassivisertdato { get; set; }

        public virtual ICollection<FaForbindelser> ForLoepenrs { get; set; }
    }
}
