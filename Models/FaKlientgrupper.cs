using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKlientgrupper
    {
        public FaKlientgrupper()
        {
            FaKlients = new HashSet<FaKlient>();
            FaPostjournals = new HashSet<FaPostjournal>();
        }

        public string KgrGruppeid { get; set; }
        public string KtpNoekkel { get; set; }
        public string KtnKontonummer { get; set; }
        public string KgrBeskrivelse { get; set; }
        public DateTime? KgrPassivisertdato { get; set; }

        public virtual FaKontoer Kt { get; set; }
        public virtual ICollection<FaKlient> FaKlients { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournals { get; set; }
    }
}
