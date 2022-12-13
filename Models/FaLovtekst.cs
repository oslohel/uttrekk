using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaLovtekst
    {
        public FaLovtekst()
        {
            FaSaksjournalLovHovedParagrafNavigations = new HashSet<FaSaksjournal>();
            FaSaksjournalLovJmfParagraf1Navigations = new HashSet<FaSaksjournal>();
            FaSaksjournalLovJmfParagraf2Navigations = new HashSet<FaSaksjournal>();
            FaTiltakLovHovedParagrafNavigations = new HashSet<FaTiltak>();
            FaTiltakLovJmfParagraf1Navigations = new HashSet<FaTiltak>();
            FaTiltakLovJmfParagraf2Navigations = new HashSet<FaTiltak>();
        }

        public string LovParagraf { get; set; }
        public string LovOverskrift { get; set; }
        public string LovLovtekst { get; set; }
        public DateTime? LovPassivisertdato { get; set; }
        public decimal LovHovedparagraf { get; set; }

        public virtual ICollection<FaSaksjournal> FaSaksjournalLovHovedParagrafNavigations { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalLovJmfParagraf1Navigations { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalLovJmfParagraf2Navigations { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakLovHovedParagrafNavigations { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakLovJmfParagraf1Navigations { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakLovJmfParagraf2Navigations { get; set; }
    }
}
