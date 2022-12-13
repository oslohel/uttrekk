using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaTilgangsgrupper
    {
        public FaTilgangsgrupper()
        {
            FaTilgangsgrVindus = new HashSet<FaTilgangsgrVindu>();
            BfuIdents = new HashSet<FaBegrFunksjoner>();
            SbhInitialers = new HashSet<FaSaksbehandlere>();
        }

        public string TggIdent { get; set; }
        public string TggBeskrivelse { get; set; }
        public decimal TggAktiverAvvikslogg { get; set; }

        public virtual ICollection<FaTilgangsgrVindu> FaTilgangsgrVindus { get; set; }

        public virtual ICollection<FaBegrFunksjoner> BfuIdents { get; set; }
        public virtual ICollection<FaSaksbehandlere> SbhInitialers { get; set; }
    }
}
