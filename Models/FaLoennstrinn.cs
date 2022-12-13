using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaLoennstrinn
    {
        public FaLoennstrinn()
        {
            FaAldersskalas = new HashSet<FaAldersskala>();
            FaEngasjementsavtaleEngFosterLoeTrinnNavigations = new HashSet<FaEngasjementsavtale>();
            FaEngasjementsavtaleLoeTrinnNavigations = new HashSet<FaEngasjementsavtale>();
            FaLoennstrinnsats = new HashSet<FaLoennstrinnsat>();
        }

        public string LoeTrinn { get; set; }
        public DateTime? LoePassivisertdato { get; set; }

        public virtual ICollection<FaAldersskala> FaAldersskalas { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleEngFosterLoeTrinnNavigations { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleLoeTrinnNavigations { get; set; }
        public virtual ICollection<FaLoennstrinnsat> FaLoennstrinnsats { get; set; }
    }
}
