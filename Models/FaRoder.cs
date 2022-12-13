using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaRoder
    {
        public FaRoder()
        {
            FaKlients = new HashSet<FaKlient>();
            FaMeldingers = new HashSet<FaMeldinger>();
            FaPostjournals = new HashSet<FaPostjournal>();
        }

        public string DisDistriktskode { get; set; }
        public string RodIdent { get; set; }
        public string RodNavn { get; set; }
        public DateTime? RodPassivisertdato { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual ICollection<FaKlient> FaKlients { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingers { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournals { get; set; }
    }
}
