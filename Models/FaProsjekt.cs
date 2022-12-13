using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaProsjekt
    {
        public FaProsjekt()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
            FaBetalingsplaners = new HashSet<FaBetalingsplaner>();
            FaJournals = new HashSet<FaJournal>();
            FaProsjektaktivitets = new HashSet<FaProsjektaktivitet>();
            FaProsjektdeltEks = new HashSet<FaProsjektdeltEk>();
            FaProsjektdeltInts = new HashSet<FaProsjektdeltInt>();
            FaProsjektevaluerings = new HashSet<FaProsjektevaluering>();
        }

        public decimal ProLoepenr { get; set; }
        public string PrtProsjekttype { get; set; }
        public string DisDistriktskode { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public string KtpNoekkel { get; set; }
        public string KtnKontonummer { get; set; }
        public string ProNavn { get; set; }
        public string ProBeskrivelse { get; set; }
        public DateTime ProStartdato { get; set; }
        public DateTime? ProSluttdato { get; set; }
        public string ProInnhold { get; set; }
        public string ProMaalsetning { get; set; }
        public DateTime? ProRegistrertdato { get; set; }
        public DateTime? ProEndretdato { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual FaKontoer Kt { get; set; }
        public virtual FaProsjekttype PrtProsjekttypeNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplaners { get; set; }
        public virtual ICollection<FaJournal> FaJournals { get; set; }
        public virtual ICollection<FaProsjektaktivitet> FaProsjektaktivitets { get; set; }
        public virtual ICollection<FaProsjektdeltEk> FaProsjektdeltEks { get; set; }
        public virtual ICollection<FaProsjektdeltInt> FaProsjektdeltInts { get; set; }
        public virtual ICollection<FaProsjektevaluering> FaProsjektevaluerings { get; set; }
    }
}
