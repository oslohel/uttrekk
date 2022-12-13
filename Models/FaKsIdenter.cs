using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKsIdenter
    {
        public FaKsIdenter()
        {
            FaKssatsers = new HashSet<FaKssatser>();
        }

        public string KsiIdent { get; set; }
        public string KsiBeskrivelse { get; set; }
        public string KsiType { get; set; }
        public string KsiFraaar { get; set; }
        public string KsiTilaar { get; set; }
        public DateTime? KsiPassivisertdato { get; set; }

        public virtual ICollection<FaKssatser> FaKssatsers { get; set; }
    }
}
