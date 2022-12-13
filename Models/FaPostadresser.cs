using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaPostadresser
    {
        public FaPostadresser()
        {
            FaAppointments = new HashSet<FaAppointment>();
            FaDistrikts = new HashSet<FaDistrikt>();
            FaEiers = new HashSet<FaEier>();
            FaForbindelsers = new HashSet<FaForbindelser>();
            FaForbindelsesadressers = new HashSet<FaForbindelsesadresser>();
            FaKlientadressers = new HashSet<FaKlientadresser>();
            FaKlients = new HashSet<FaKlient>();
            FaMeldingers = new HashSet<FaMeldinger>();
            FaPostjournalkopitils = new HashSet<FaPostjournalkopitil>();
            FaPostjournals = new HashSet<FaPostjournal>();
            FaSaksbehandleres = new HashSet<FaSaksbehandlere>();
            FaSoekers = new HashSet<FaSoeker>();
        }

        public string PnrPostnr { get; set; }
        public decimal? KomKommunenr { get; set; }
        public string PnrPoststed { get; set; }

        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual ICollection<FaAppointment> FaAppointments { get; set; }
        public virtual ICollection<FaDistrikt> FaDistrikts { get; set; }
        public virtual ICollection<FaEier> FaEiers { get; set; }
        public virtual ICollection<FaForbindelser> FaForbindelsers { get; set; }
        public virtual ICollection<FaForbindelsesadresser> FaForbindelsesadressers { get; set; }
        public virtual ICollection<FaKlientadresser> FaKlientadressers { get; set; }
        public virtual ICollection<FaKlient> FaKlients { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingers { get; set; }
        public virtual ICollection<FaPostjournalkopitil> FaPostjournalkopitils { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournals { get; set; }
        public virtual ICollection<FaSaksbehandlere> FaSaksbehandleres { get; set; }
        public virtual ICollection<FaSoeker> FaSoekers { get; set; }
    }
}
