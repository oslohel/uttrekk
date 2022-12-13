using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKilometergodtgjoerelse
    {
        public FaKilometergodtgjoerelse()
        {
            FaEngasjementsavtales = new HashSet<FaEngasjementsavtale>();
            FaKmsatsers = new HashSet<FaKmsatser>();
        }

        public string KmgIdent { get; set; }
        public string KmgBeskrivelse { get; set; }

        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtales { get; set; }
        public virtual ICollection<FaKmsatser> FaKmsatsers { get; set; }
    }
}
