using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaVindu
    {
        public FaVindu()
        {
            FaTilgangsgrVindus = new HashSet<FaTilgangsgrVindu>();
        }

        public string VinUtvnavn { get; set; }
        public string VinBeskrivelse { get; set; }
        public string VinUtveventid { get; set; }

        public virtual ICollection<FaTilgangsgrVindu> FaTilgangsgrVindus { get; set; }
    }
}
