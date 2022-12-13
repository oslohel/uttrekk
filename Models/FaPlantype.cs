using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaPlantype
    {
        public FaPlantype()
        {
            FaTiltaksplans = new HashSet<FaTiltaksplan>();
        }

        public string PtyPlankode { get; set; }
        public string PtyPlantype { get; set; }
        public string PtyLovhjemmel { get; set; }

        public virtual ICollection<FaTiltaksplan> FaTiltaksplans { get; set; }
    }
}
