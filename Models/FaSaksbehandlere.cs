using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaSaksbehandlere
    {
        public FaSaksbehandlere()
        {
            FaAktivitets = new HashSet<FaAktivitet>();
            FaBehandlingsmallinjes = new HashSet<FaBehandlingsmallinje>();
            FaBetalingerSbhAnvistavNavigations = new HashSet<FaBetalinger>();
            FaBetalingerSbhEndretavNavigations = new HashSet<FaBetalinger>();
            FaBetalingerSbhRegistrertavNavigations = new HashSet<FaBetalinger>();
            FaBetalingsplanerSbhAvgjortavInitialerNavigations = new HashSet<FaBetalingsplaner>();
            FaBetalingsplanerSbhKlargjortavInitialerNavigations = new HashSet<FaBetalingsplaner>();
            FaBetalingsplanerSbhStoppetavInitialerNavigations = new HashSet<FaBetalingsplaner>();
            FaDelmaalSbhEndretavNavigations = new HashSet<FaDelmaal>();
            FaDelmaalSbhRegistrertavNavigations = new HashSet<FaDelmaal>();
            FaDokumenterSbhEndretavNavigations = new HashSet<FaDokumenter>();
            FaDokumenterSbhUtsjekketavInitialerNavigations = new HashSet<FaDokumenter>();
            FaEngasjementsavtaleSbhAvgjortavInitialerNavigations = new HashSet<FaEngasjementsavtale>();
            FaEngasjementsavtaleSbhEndretavNavigations = new HashSet<FaEngasjementsavtale>();
            FaEngasjementsavtaleSbhInitialerNavigations = new HashSet<FaEngasjementsavtale>();
            FaEngasjementsavtaleSbhKlargjortavInitialerNavigations = new HashSet<FaEngasjementsavtale>();
            FaEngasjementsavtaleSbhRegistrertavNavigations = new HashSet<FaEngasjementsavtale>();
            FaEngasjementsavtaleSbhStoppetavInitialerNavigations = new HashSet<FaEngasjementsavtale>();
            FaEngasjementslinjerSbhAnvistavNavigations = new HashSet<FaEngasjementslinjer>();
            FaEngasjementslinjerSbhEndretavNavigations = new HashSet<FaEngasjementslinjer>();
            FaEngasjementslinjerSbhRegistrertavNavigations = new HashSet<FaEngasjementslinjer>();
            FaEngasjementsplanSbhAvgjortavInitialerNavigations = new HashSet<FaEngasjementsplan>();
            FaEngasjementsplanSbhKlargjortavInitialerNavigations = new HashSet<FaEngasjementsplan>();
            FaEngasjementsplanSbhStoppetavInitialerNavigations = new HashSet<FaEngasjementsplan>();
            FaForbindelserSbhEndretavNavigations = new HashSet<FaForbindelser>();
            FaForbindelserSbhRegistrertavNavigations = new HashSet<FaForbindelser>();
            FaForbindelsesadressers = new HashSet<FaForbindelsesadresser>();
            FaGenerellsakSbhEndretavNavigations = new HashSet<FaGenerellsak>();
            FaGenerellsakSbhInitialer2Navigations = new HashSet<FaGenerellsak>();
            FaGenerellsakSbhInitialerNavigations = new HashSet<FaGenerellsak>();
            FaGenerellsakSbhRegistrertavNavigations = new HashSet<FaGenerellsak>();
            FaHuskelappSbhEndretavNavigations = new HashSet<FaHuskelapp>();
            FaHuskelappSbhInitialerNavigations = new HashSet<FaHuskelapp>();
            FaHuskelappSbhRegistrertavNavigations = new HashSet<FaHuskelapp>();
            FaJournalSbhEndretavNavigations = new HashSet<FaJournal>();
            FaJournalSbhInitialerNavigations = new HashSet<FaJournal>();
            FaJournalSbhRegistrertavNavigations = new HashSet<FaJournal>();
            FaKlientSbhEndretavNavigations = new HashSet<FaKlient>();
            FaKlientSbhInitialer2Navigations = new HashSet<FaKlient>();
            FaKlientSbhInitialerNavigations = new HashSet<FaKlient>();
            FaKlientSbhRegistrertavNavigations = new HashSet<FaKlient>();
            FaKvelloSbhEndretavNavigations = new HashSet<FaKvello>();
            FaKvelloSbhRegistrertavNavigations = new HashSet<FaKvello>();
            FaMarkedclientelements = new HashSet<FaMarkedclientelement>();
            FaMedarbeidereSbhEndretavNavigations = new HashSet<FaMedarbeidere>();
            FaMedarbeidereSbhRegistrertavNavigations = new HashSet<FaMedarbeidere>();
            FaMeldingerSbhEndretavNavigations = new HashSet<FaMeldinger>();
            FaMeldingerSbhInitialerNavigations = new HashSet<FaMeldinger>();
            FaMeldingerSbhMottattavNavigations = new HashSet<FaMeldinger>();
            FaMeldingerSbhRegistrertavNavigations = new HashSet<FaMeldinger>();
            FaMeldingerSlettets = new HashSet<FaMeldingerSlettet>();
            FaPlantiltakSbhEndretavNavigations = new HashSet<FaPlantiltak>();
            FaPlantiltakSbhRegistrertavNavigations = new HashSet<FaPlantiltak>();
            FaPostjournalSbhEndretavNavigations = new HashSet<FaPostjournal>();
            FaPostjournalSbhGodkjennInitialerNavigations = new HashSet<FaPostjournal>();
            FaPostjournalSbhInitialerNavigations = new HashSet<FaPostjournal>();
            FaPostjournalSbhRegistrertavNavigations = new HashSet<FaPostjournal>();
            FaProsjektSbhEndretavNavigations = new HashSet<FaProsjekt>();
            FaProsjektSbhInitialerNavigations = new HashSet<FaProsjekt>();
            FaProsjektSbhRegistrertavNavigations = new HashSet<FaProsjekt>();
            FaProsjektaktivitets = new HashSet<FaProsjektaktivitet>();
            FaProsjektdeltInts = new HashSet<FaProsjektdeltInt>();
            FaProsjektevaluerings = new HashSet<FaProsjektevaluering>();
            FaRefusjonerSbhEndretavNavigations = new HashSet<FaRefusjoner>();
            FaRefusjonerSbhRegistrertavNavigations = new HashSet<FaRefusjoner>();
            FaRefusjonskravSbhEndretavNavigations = new HashSet<FaRefusjonskrav>();
            FaRefusjonskravSbhRegistrertavNavigations = new HashSet<FaRefusjonskrav>();
            FaRekvisisjoners = new HashSet<FaRekvisisjoner>();
            FaSaksbehKlients = new HashSet<FaSaksbehKlient>();
            FaSaksjournalSbhAvgjortavInitialerNavigations = new HashSet<FaSaksjournal>();
            FaSaksjournalSbhBortfaltInitialerNavigations = new HashSet<FaSaksjournal>();
            FaSaksjournalSbhEndretavNavigations = new HashSet<FaSaksjournal>();
            FaSaksjournalSbhInitialerNavigations = new HashSet<FaSaksjournal>();
            FaSaksjournalSbhOpphevetInitialerNavigations = new HashSet<FaSaksjournal>();
            FaSaksjournalSbhRegistrertavNavigations = new HashSet<FaSaksjournal>();
            FaSsbStatistikks = new HashSet<FaSsbStatistikk>();
            FaTiltakSbhEndretavNavigations = new HashSet<FaTiltak>();
            FaTiltakSbhInitialer2Navigations = new HashSet<FaTiltak>();
            FaTiltakSbhInitialerNavigations = new HashSet<FaTiltak>();
            FaTiltakSbhRegistrertavNavigations = new HashSet<FaTiltak>();
            FaTiltaksevaluerings = new HashSet<FaTiltaksevaluering>();
            FaTiltakslinjers = new HashSet<FaTiltakslinjer>();
            FaTiltaksplanSbhEndretavNavigations = new HashSet<FaTiltaksplan>();
            FaTiltaksplanSbhInitialerNavigations = new HashSet<FaTiltaksplan>();
            FaTiltaksplanSbhRegistrertavNavigations = new HashSet<FaTiltaksplan>();
            FaTiltaksplanevalueringerSbhEndretavNavigations = new HashSet<FaTiltaksplanevalueringer>();
            FaTiltaksplanevalueringerSbhInitialerNavigations = new HashSet<FaTiltaksplanevalueringer>();
            FaTiltaksplanevalueringerSbhRegistrertavNavigations = new HashSet<FaTiltaksplanevalueringer>();
            FaUndersoekelserSbhEndretavNavigations = new HashSet<FaUndersoekelser>();
            FaUndersoekelserSbhGodkjennSluttrapportNavigations = new HashSet<FaUndersoekelser>();
            FaUndersoekelserSbhInitialer2Navigations = new HashSet<FaUndersoekelser>();
            FaUndersoekelserSbhInitialerNavigations = new HashSet<FaUndersoekelser>();
            FaUndersoekelserSbhRegistrertavNavigations = new HashSet<FaUndersoekelser>();
            FaUndersoekelserSlettets = new HashSet<FaUndersoekelserSlettet>();
            FaUndersoekelseslinjerSbhInitialer2Navigations = new HashSet<FaUndersoekelseslinjer>();
            FaUndersoekelseslinjerSbhInitialerNavigations = new HashSet<FaUndersoekelseslinjer>();
            FaVurdegenbetSbhEndretavNavigations = new HashSet<FaVurdegenbet>();
            FaVurdegenbetSbhInitialerNavigations = new HashSet<FaVurdegenbet>();
            FaVurdegenbetSbhRegistrertavNavigations = new HashSet<FaVurdegenbet>();
            DisDistriktskodes = new HashSet<FaDistrikt>();
            TggIdents = new HashSet<FaTilgangsgrupper>();
        }

        public string SbhInitialer { get; set; }
        public string PnrPostnr { get; set; }
        public string SbhLoennsnr { get; set; }
        public string SbhFoedselsnr { get; set; }
        public string SbhFornavn { get; set; }
        public string SbhEtternavn { get; set; }
        public string SbhAdresse1 { get; set; }
        public string SbhAdresse2 { get; set; }
        public string SbhTelefonprivat { get; set; }
        public string SbhTelefonarbeid { get; set; }
        public string SbhTelefonmobil { get; set; }
        public string SbhTelefaks { get; set; }
        public string SbhMerknad { get; set; }
        public DateTime? SbhEndretpassord { get; set; }
        public DateTime? SbhPassivisertdato { get; set; }
        public string SbhMailadresse { get; set; }
        public DateTime? SbhStartdato { get; set; }
        public string SbhTittel { get; set; }
        public bool SbhReducedmobileaccess { get; set; }
        public bool SbhHasmobileaccess { get; set; }
        public string SbhToken { get; set; }
        public DateTime? SbhDatelastused { get; set; }
        public DateTime? SbhDatetimeout { get; set; }
        public int? SbhLastclientidaccessed { get; set; }
        public DateTime? SbhLastclientidaccessedtimestamp { get; set; }
        public int SbhLastclientidvalidationattempts { get; set; }
        public string SbhPincode { get; set; }
        public int SbhPincodesattempts { get; set; }
        public DateTime? SbhPincodetimestamp { get; set; }
        public string SbhLocale { get; set; }
        public string SbhPassordhash { get; set; }
        public string SbhPassordsalt { get; set; }
        public int? SbhPassorditer { get; set; }

        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual FaSbhSetting FaSbhSetting { get; set; }
        public virtual ICollection<FaAktivitet> FaAktivitets { get; set; }
        public virtual ICollection<FaBehandlingsmallinje> FaBehandlingsmallinjes { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerSbhAnvistavNavigations { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplanerSbhAvgjortavInitialerNavigations { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplanerSbhKlargjortavInitialerNavigations { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplanerSbhStoppetavInitialerNavigations { get; set; }
        public virtual ICollection<FaDelmaal> FaDelmaalSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaDelmaal> FaDelmaalSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaDokumenter> FaDokumenterSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaDokumenter> FaDokumenterSbhUtsjekketavInitialerNavigations { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleSbhAvgjortavInitialerNavigations { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleSbhKlargjortavInitialerNavigations { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtaleSbhStoppetavInitialerNavigations { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerSbhAnvistavNavigations { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaEngasjementsplan> FaEngasjementsplanSbhAvgjortavInitialerNavigations { get; set; }
        public virtual ICollection<FaEngasjementsplan> FaEngasjementsplanSbhKlargjortavInitialerNavigations { get; set; }
        public virtual ICollection<FaEngasjementsplan> FaEngasjementsplanSbhStoppetavInitialerNavigations { get; set; }
        public virtual ICollection<FaForbindelser> FaForbindelserSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaForbindelser> FaForbindelserSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaForbindelsesadresser> FaForbindelsesadressers { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsakSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsakSbhInitialer2Navigations { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsakSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsakSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaHuskelapp> FaHuskelappSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaHuskelapp> FaHuskelappSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaHuskelapp> FaHuskelappSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaJournal> FaJournalSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaJournal> FaJournalSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaJournal> FaJournalSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaKlient> FaKlientSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaKlient> FaKlientSbhInitialer2Navigations { get; set; }
        public virtual ICollection<FaKlient> FaKlientSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaKlient> FaKlientSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaKvello> FaKvelloSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaKvello> FaKvelloSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaMarkedclientelement> FaMarkedclientelements { get; set; }
        public virtual ICollection<FaMedarbeidere> FaMedarbeidereSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaMedarbeidere> FaMedarbeidereSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerSbhMottattavNavigations { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaMeldingerSlettet> FaMeldingerSlettets { get; set; }
        public virtual ICollection<FaPlantiltak> FaPlantiltakSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaPlantiltak> FaPlantiltakSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalSbhGodkjennInitialerNavigations { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaProsjekt> FaProsjektSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaProsjekt> FaProsjektSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaProsjekt> FaProsjektSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaProsjektaktivitet> FaProsjektaktivitets { get; set; }
        public virtual ICollection<FaProsjektdeltInt> FaProsjektdeltInts { get; set; }
        public virtual ICollection<FaProsjektevaluering> FaProsjektevaluerings { get; set; }
        public virtual ICollection<FaRefusjoner> FaRefusjonerSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaRefusjoner> FaRefusjonerSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaRefusjonskrav> FaRefusjonskravSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaRefusjonskrav> FaRefusjonskravSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaRekvisisjoner> FaRekvisisjoners { get; set; }
        public virtual ICollection<FaSaksbehKlient> FaSaksbehKlients { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalSbhAvgjortavInitialerNavigations { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalSbhBortfaltInitialerNavigations { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalSbhOpphevetInitialerNavigations { get; set; }
        public virtual ICollection<FaSaksjournal> FaSaksjournalSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaSsbStatistikk> FaSsbStatistikks { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakSbhInitialer2Navigations { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaTiltaksevaluering> FaTiltaksevaluerings { get; set; }
        public virtual ICollection<FaTiltakslinjer> FaTiltakslinjers { get; set; }
        public virtual ICollection<FaTiltaksplan> FaTiltaksplanSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaTiltaksplan> FaTiltaksplanSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaTiltaksplan> FaTiltaksplanSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaTiltaksplanevalueringer> FaTiltaksplanevalueringerSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaTiltaksplanevalueringer> FaTiltaksplanevalueringerSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaTiltaksplanevalueringer> FaTiltaksplanevalueringerSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserSbhGodkjennSluttrapportNavigations { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserSbhInitialer2Navigations { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserSbhRegistrertavNavigations { get; set; }
        public virtual ICollection<FaUndersoekelserSlettet> FaUndersoekelserSlettets { get; set; }
        public virtual ICollection<FaUndersoekelseslinjer> FaUndersoekelseslinjerSbhInitialer2Navigations { get; set; }
        public virtual ICollection<FaUndersoekelseslinjer> FaUndersoekelseslinjerSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaVurdegenbet> FaVurdegenbetSbhEndretavNavigations { get; set; }
        public virtual ICollection<FaVurdegenbet> FaVurdegenbetSbhInitialerNavigations { get; set; }
        public virtual ICollection<FaVurdegenbet> FaVurdegenbetSbhRegistrertavNavigations { get; set; }

        public virtual ICollection<FaDistrikt> DisDistriktskodes { get; set; }
        public virtual ICollection<FaTilgangsgrupper> TggIdents { get; set; }
    }
}
