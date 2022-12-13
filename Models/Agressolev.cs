using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class Agressolev
    {
        public Agressolev()
        {
            FaForbindelsers = new HashSet<FaForbindelser>();
        }

        public decimal AglLevnr { get; set; }
        public string AglLevnavn { get; set; }
        public string AglOrgnr { get; set; }
        public string AglBankAccount { get; set; }
        public string AglAdresse { get; set; }
        public string AglLandkode { get; set; }
        public string AglSted { get; set; }
        public string AglPostnr { get; set; }

        public virtual ICollection<FaForbindelser> FaForbindelsers { get; set; }
    }
}
