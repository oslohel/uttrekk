using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKontoplan
    {
        public FaKontoplan()
        {
            FaKontoers = new HashSet<FaKontoer>();
        }

        public string KtpNoekkel { get; set; }
        public decimal KtpDimensjon { get; set; }
        public decimal? KtpDimensjonLengde { get; set; }
        public string KtpFortekst { get; set; }
        public string KtpBehandlingsregel { get; set; }
        public string KtpStyringsregel { get; set; }

        public virtual ICollection<FaKontoer> FaKontoers { get; set; }
    }
}
