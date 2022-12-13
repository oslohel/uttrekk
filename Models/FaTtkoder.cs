using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaTtkoder
    {
        public FaTtkoder()
        {
            FaBetalingskategorierTtkKodeIndividuellNavigations = new HashSet<FaBetalingskategorier>();
            FaBetalingskategorierTtkKodeRegulativNavigations = new HashSet<FaBetalingskategorier>();
        }

        public string TtkKode { get; set; }
        public string TtkBeskrivelse { get; set; }
        public DateTime? TtkPassivisertdato { get; set; }

        public virtual ICollection<FaBetalingskategorier> FaBetalingskategorierTtkKodeIndividuellNavigations { get; set; }
        public virtual ICollection<FaBetalingskategorier> FaBetalingskategorierTtkKodeRegulativNavigations { get; set; }
    }
}
