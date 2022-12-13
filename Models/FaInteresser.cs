using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaInteresser
    {
        public FaInteresser()
        {
            FaKlientinteressers = new HashSet<FaKlientinteresser>();
            FaMedarbeiderinteressers = new HashSet<FaMedarbeiderinteresser>();
        }

        public string IntIdent { get; set; }
        public string IntBeskrivelse { get; set; }

        public virtual ICollection<FaKlientinteresser> FaKlientinteressers { get; set; }
        public virtual ICollection<FaMedarbeiderinteresser> FaMedarbeiderinteressers { get; set; }
    }
}
