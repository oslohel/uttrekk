using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaForbindelser
    {
        public FaForbindelser()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
            FaForbindelsesadressers = new HashSet<FaForbindelsesadresser>();
            FaJournals = new HashSet<FaJournal>();
            FaKlienttilknytnings = new HashSet<FaKlienttilknytning>();
            FaKontaktpersoners = new HashSet<FaKontaktpersoner>();
            FaProsjektdeltEks = new HashSet<FaProsjektdeltEk>();
            FaRekvisisjoners = new HashSet<FaRekvisisjoner>();
            FotIdents = new HashSet<FaForbindelsestyper>();
        }

        public decimal ForLoepenr { get; set; }
        public string PnrPostnr { get; set; }
        public decimal? ForAgressolevnr { get; set; }
        public decimal? NasKodenr { get; set; }
        public string ForEtternavn { get; set; }
        public string SbhEndretav { get; set; }
        public decimal? KomKommunenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public string DisDistriktskode { get; set; }
        public string ForFornavn { get; set; }
        public string ForPostadresse { get; set; }
        public string ForBesoeksadresse { get; set; }
        public string ForTelefonprivat { get; set; }
        public string ForKontonummer { get; set; }
        public string ForTelefaks { get; set; }
        public string ForTelefonarbeid { get; set; }
        public string ForTelefonmobil { get; set; }
        public string ForArbeidssted { get; set; }
        public string ForLeverandoernr { get; set; }
        public string ForFoedselsnummer { get; set; }
        public DateTime? ForEndretdato { get; set; }
        public DateTime? ForRegistrertdato { get; set; }
        public string ForBetalingsmaate { get; set; }
        public DateTime? ForPassivisertdato { get; set; }
        public string ForEmail { get; set; }
        public string ForUrl { get; set; }
        public string ForGmlreferanse { get; set; }
        public string ForOrganisasjonsnr { get; set; }
        public decimal? ForDnummer { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual Agressolev ForAgressolevnrNavigation { get; set; }
        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual FaNasjoner NasKodenrNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaMedarbeidere FaMedarbeidere { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaForbindelsesadresser> FaForbindelsesadressers { get; set; }
        public virtual ICollection<FaJournal> FaJournals { get; set; }
        public virtual ICollection<FaKlienttilknytning> FaKlienttilknytnings { get; set; }
        public virtual ICollection<FaKontaktpersoner> FaKontaktpersoners { get; set; }
        public virtual ICollection<FaProsjektdeltEk> FaProsjektdeltEks { get; set; }
        public virtual ICollection<FaRekvisisjoner> FaRekvisisjoners { get; set; }
        public virtual ICollection<FaForbindelsestyper> FotIdents { get; set; }
    }
}
