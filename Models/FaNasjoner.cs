using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaNasjoner
    {
        public FaNasjoner()
        {
            FaForbindelsers = new HashSet<FaForbindelser>();
            FaKlients = new HashSet<FaKlient>();
            InverseNasStatsKodenrNavigation = new HashSet<FaNasjoner>();
        }

        public decimal NasKodenr { get; set; }
        public decimal? NasStatsKodenr { get; set; }
        public string NasNasjonsnavn { get; set; }
        public string NasLandkode { get; set; }

        public virtual FaNasjoner NasStatsKodenrNavigation { get; set; }
        public virtual ICollection<FaForbindelser> FaForbindelsers { get; set; }
        public virtual ICollection<FaKlient> FaKlients { get; set; }
        public virtual ICollection<FaNasjoner> InverseNasStatsKodenrNavigation { get; set; }
    }
}
