using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKlient
    {
        public FaKlient()
        {
            FaAppointments = new HashSet<FaAppointment>();
            FaBetalingers = new HashSet<FaBetalinger>();
            FaEngasjementsavtales = new HashSet<FaEngasjementsavtale>();
            FaGenerellsaks = new HashSet<FaGenerellsak>();
            FaHuskelapps = new HashSet<FaHuskelapp>();
            FaJournals = new HashSet<FaJournal>();
            FaKlientadressers = new HashSet<FaKlientadresser>();
            FaKlientinteressers = new HashSet<FaKlientinteresser>();
            FaKlientplasserings = new HashSet<FaKlientplassering>();
            FaKlienttilknytnings = new HashSet<FaKlienttilknytning>();
            FaKvellos = new HashSet<FaKvello>();
            FaMarkedclientelements = new HashSet<FaMarkedclientelement>();
            FaMeldingerSlettets = new HashSet<FaMeldingerSlettet>();
            FaMeldingers = new HashSet<FaMeldinger>();
            FaPostjournalkopitils = new HashSet<FaPostjournalkopitil>();
            FaPostjournals = new HashSet<FaPostjournal>();
            FaRefusjoners = new HashSet<FaRefusjoner>();
            FaRefusjonskravs = new HashSet<FaRefusjonskrav>();
            FaSaksbehKlients = new HashSet<FaSaksbehKlient>();
            FaSaksjournals = new HashSet<FaSaksjournal>();
            FaStatSsb1s = new HashSet<FaStatSsb1>();
            FaTiltaks = new HashSet<FaTiltak>();
            FaTiltakslinjers = new HashSet<FaTiltakslinjer>();
            FaTiltaksplans = new HashSet<FaTiltaksplan>();
            FaUndersoekelserSlettets = new HashSet<FaUndersoekelserSlettet>();
            InverseKliFamilienrNavigation = new HashSet<FaKlient>();
            InverseKliHusstandnrNavigation = new HashSet<FaKlient>();
        }

        public decimal KliLoepenr { get; set; }
        public string KgrGruppeid { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhInitialer2 { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public string DisDistriktskode { get; set; }
        public string PnrPostnr { get; set; }
        public decimal? KomKommunenr { get; set; }
        public decimal? NasKodenr { get; set; }
        public decimal? KliFamilienr { get; set; }
        public decimal? KomPlassert { get; set; }
        public string RodIdent { get; set; }
        public decimal? KliHusstandnr { get; set; }
        public string KliFornavn { get; set; }
        public string KliEtternavn { get; set; }
        public DateTime? KliFoedselsdato { get; set; }
        public decimal? KliPersonnr { get; set; }
        public decimal KliAktiv { get; set; }
        public DateTime? KliAvsluttetdato { get; set; }
        public DateTime? KliBehandlingsdato { get; set; }
        public string KliFlyktningestatus { get; set; }
        public decimal? KliFremmedkontrollnr { get; set; }
        public string KliKjoenn { get; set; }
        public string KliAdresse { get; set; }
        public string KliGatenavn { get; set; }
        public string KliGatenr { get; set; }
        public string KliKontonr { get; set; }
        public string KliMerknader { get; set; }
        public string KliFamiliestatus { get; set; }
        public decimal KliInnvandrerbarn { get; set; }
        public string KliStatsborgerskap { get; set; }
        public string KliTelefonarbeid { get; set; }
        public string KliTelefonprivat { get; set; }
        public string KliTelefonmobil { get; set; }
        public DateTime? KliRegistrertdato { get; set; }
        public DateTime? KliEndretdato { get; set; }
        public decimal KliFraannenkommune { get; set; }
        public string KliLeverandoernr { get; set; }
        public decimal? KliSistbruktedoknr { get; set; }
        public string KliTidligeretiltak { get; set; }
        public string KliFlyttetfrabydel { get; set; }
        public string KliFlyttettilbydel { get; set; }
        public DateTime? KliFlyttetfradato { get; set; }
        public DateTime? KliFlyttettildato { get; set; }
        public decimal KliFlyttetktrlskjema { get; set; }
        public string KliGmlreferanse { get; set; }
        public DateTime? KliUsavsluttetdato { get; set; }
        public string KliFylkesmannStatus { get; set; }
        public DateTime? KliFymStatusgendato { get; set; }
        public DateTime? KliFymStatusprdato { get; set; }
        public string KliEmail { get; set; }
        public string KliBufetatnr { get; set; }
        public decimal? ArkMappeSystemid { get; set; }
        public DateTime? ArkDato { get; set; }
        public decimal ArkNavnEndret { get; set; }
        public decimal ArkFnrEndret { get; set; }
        public decimal ArkStopp { get; set; }
        public string KliFylkesmannKategori { get; set; }
        public string KliStatsborgerskapKategori { get; set; }
        public string KliOpenReasonNotSbh { get; set; }
        public DateTime? KliEtterverndato { get; set; }
        public decimal KliEttervern { get; set; }
        public DateTime? KliTiltaksbarndato { get; set; }
        public decimal KliTiltaksbarn { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual FaRoder FaRoder { get; set; }
        public virtual FaKlientgrupper KgrGruppe { get; set; }
        public virtual FaKlient KliFamilienrNavigation { get; set; }
        public virtual FaKlient KliHusstandnrNavigation { get; set; }
        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual FaKommuner KomPlassertNavigation { get; set; }
        public virtual FaNasjoner NasKodenrNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialer2Navigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaAppointment> FaAppointments { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtales { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsaks { get; set; }
        public virtual ICollection<FaHuskelapp> FaHuskelapps { get; set; }
        public virtual ICollection<FaJournal> FaJournals { get; set; }
        public virtual ICollection<FaKlientadresser> FaKlientadressers { get; set; }
        public virtual ICollection<FaKlientinteresser> FaKlientinteressers { get; set; }
        public virtual ICollection<FaKlientplassering> FaKlientplasserings { get; set; }
        public virtual ICollection<FaKlienttilknytning> FaKlienttilknytnings { get; set; }
        public virtual ICollection<FaKvello> FaKvellos { get; set; }
        public virtual ICollection<FaMarkedclientelement> FaMarkedclientelements { get; set; }
        public virtual ICollection<FaMeldingerSlettet> FaMeldingerSlettets { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingers { get; set; }
        public virtual ICollection<FaPostjournalkopitil> FaPostjournalkopitils { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournals { get; set; }
        public virtual ICollection<FaRefusjoner> FaRefusjoners { get; set; }
        public virtual ICollection<FaRefusjonskrav> FaRefusjonskravs { get; set; }
        public virtual ICollection<FaSaksbehKlient> FaSaksbehKlients { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournals { get; set; }
        public virtual ICollection<FaStatSsb1> FaStatSsb1s { get; set; }
        public virtual ICollection<FaTiltak> FaTiltaks { get; set; }
        public virtual ICollection<FaTiltakslinjer> FaTiltakslinjers { get; set; }
        public virtual ICollection<FaTiltaksplan> FaTiltaksplans { get; set; }
        public virtual ICollection<FaUndersoekelserSlettet> FaUndersoekelserSlettets { get; set; }
        public virtual ICollection<FaKlient> InverseKliFamilienrNavigation { get; set; }
        public virtual ICollection<FaKlient> InverseKliHusstandnrNavigation { get; set; }
    }
}
