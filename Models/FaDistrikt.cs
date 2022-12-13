using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaDistrikt
    {
        public FaDistrikt()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
            FaDistriktloepenrseriers = new HashSet<FaDistriktloepenrserier>();
            FaEngasjementslinjers = new HashSet<FaEngasjementslinjer>();
            FaForbindelsers = new HashSet<FaForbindelser>();
            FaGenerellsaks = new HashSet<FaGenerellsak>();
            FaKlients = new HashSet<FaKlient>();
            FaMeldingers = new HashSet<FaMeldinger>();
            FaPostjournals = new HashSet<FaPostjournal>();
            FaProsjekts = new HashSet<FaProsjekt>();
            FaRoders = new HashSet<FaRoder>();
            FaTellers = new HashSet<FaTeller>();
            SbhInitialers = new HashSet<FaSaksbehandlere>();
        }

        public string DisDistriktskode { get; set; }
        public string KtnKontonrUHjemmet { get; set; }
        public string KtnKontonrIHjemmet { get; set; }
        public string KtpNoekkel { get; set; }
        public string PnrPostnr { get; set; }
        public decimal? KomKommunenr { get; set; }
        public string DisDistriktsnavn { get; set; }
        public string DisAdresse1 { get; set; }
        public string DisAdresse2 { get; set; }
        public string DisTelefonnr { get; set; }
        public string DisTelefaks { get; set; }
        public string DisLederstittel { get; set; }
        public DateTime? DisPassivisertdato { get; set; }
        public string DisLedersnavn { get; set; }
        public string DisSsbKode { get; set; }
        public string DisSelskap { get; set; }
        public string DisOrganisasjonsnr { get; set; }
        public string DisBydelsnavn { get; set; }

        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual FaKontoer Kt { get; set; }
        public virtual FaKontoer KtNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual FaEksterneParametre FaEksterneParametre { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaDistriktloepenrserier> FaDistriktloepenrseriers { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjers { get; set; }
        public virtual ICollection<FaForbindelser> FaForbindelsers { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsaks { get; set; }
        public virtual ICollection<FaKlient> FaKlients { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingers { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournals { get; set; }
        public virtual ICollection<FaProsjekt> FaProsjekts { get; set; }
        public virtual ICollection<FaRoder> FaRoders { get; set; }
        public virtual ICollection<FaTeller> FaTellers { get; set; }

        public virtual ICollection<FaSaksbehandlere> SbhInitialers { get; set; }
    }
}
