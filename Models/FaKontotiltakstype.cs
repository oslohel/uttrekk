using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKontotiltakstype
    {
        public FaKontotiltakstype()
        {
            FaTiltaks = new HashSet<FaTiltak>();
        }

        public string KttTiltakskode { get; set; }
        public string KtpNoekkel { get; set; }
        public string KtnKontonr { get; set; }
        public string KttBeskrivelse { get; set; }
        public DateTime? KttPassivisertdato { get; set; }

        public virtual FaKontoer Kt { get; set; }
        public virtual ICollection<FaTiltak> FaTiltaks { get; set; }
    }
}
