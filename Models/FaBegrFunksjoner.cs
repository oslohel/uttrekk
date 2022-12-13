using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaBegrFunksjoner
    {
        public FaBegrFunksjoner()
        {
            TggIdents = new HashSet<FaTilgangsgrupper>();
        }

        public string BfuIdent { get; set; }
        public string BfuBeskrivelse { get; set; }

        public virtual ICollection<FaTilgangsgrupper> TggIdents { get; set; }
    }
}
