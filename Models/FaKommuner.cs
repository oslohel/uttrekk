using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKommuner
    {
        public FaKommuner()
        {
            FaDistrikts = new HashSet<FaDistrikt>();
            FaForbindelsers = new HashSet<FaForbindelser>();
            FaForbindelsesadressers = new HashSet<FaForbindelsesadresser>();
            FaGenerellsaks = new HashSet<FaGenerellsak>();
            FaKlientKomKommunenrNavigations = new HashSet<FaKlient>();
            FaKlientKomPlassertNavigations = new HashSet<FaKlient>();
            FaKlientadressers = new HashSet<FaKlientadresser>();
            FaKlientplasserings = new HashSet<FaKlientplassering>();
            FaPostadressers = new HashSet<FaPostadresser>();
            FaSsbStatistikks = new HashSet<FaSsbStatistikk>();
            FaStatSsb1s = new HashSet<FaStatSsb1>();
        }

        public decimal KomKommunenr { get; set; }
        public string KomKommunenavn { get; set; }
        public DateTime? KomPassivisertdato { get; set; }

        public virtual FaEier FaEier { get; set; }
        public virtual ICollection<FaDistrikt> FaDistrikts { get; set; }
        public virtual ICollection<FaForbindelser> FaForbindelsers { get; set; }
        public virtual ICollection<FaForbindelsesadresser> FaForbindelsesadressers { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsaks { get; set; }
        public virtual ICollection<FaKlient> FaKlientKomKommunenrNavigations { get; set; }
        public virtual ICollection<FaKlient> FaKlientKomPlassertNavigations { get; set; }
        public virtual ICollection<FaKlientadresser> FaKlientadressers { get; set; }
        public virtual ICollection<FaKlientplassering> FaKlientplasserings { get; set; }
        public virtual ICollection<FaPostadresser> FaPostadressers { get; set; }
        public virtual ICollection<FaSsbStatistikk> FaSsbStatistikks { get; set; }
        public virtual ICollection<FaStatSsb1> FaStatSsb1s { get; set; }
    }
}
