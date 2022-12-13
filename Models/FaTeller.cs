using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaTeller
    {
        public FaTeller()
        {
            FaMerkantilfils = new HashSet<FaMerkantilfil>();
        }

        public decimal TelTeller { get; set; }
        public string DisDistriktskode { get; set; }
        public string TelType { get; set; }
        public decimal? TelSistbruktenr { get; set; }
        public string TelBeskrivelse { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual ICollection<FaMerkantilfil> FaMerkantilfils { get; set; }
    }
}
