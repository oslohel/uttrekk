using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKontoer
    {
        public FaKontoer()
        {
            FaBetalingerKt1s = new HashSet<FaBetalinger>();
            FaBetalingerKt2s = new HashSet<FaBetalinger>();
            FaBetalingerKt3s = new HashSet<FaBetalinger>();
            FaBetalingerKt4s = new HashSet<FaBetalinger>();
            FaBetalingerKtNavigations = new HashSet<FaBetalinger>();
            FaBetalingerKts = new HashSet<FaBetalinger>();
            FaBetalingskategoriers = new HashSet<FaBetalingskategorier>();
            FaBudsjettKt1s = new HashSet<FaBudsjett>();
            FaBudsjettKt2s = new HashSet<FaBudsjett>();
            FaBudsjettKt3s = new HashSet<FaBudsjett>();
            FaBudsjettKt4s = new HashSet<FaBudsjett>();
            FaBudsjettKtNavigations = new HashSet<FaBudsjett>();
            FaBudsjettKts = new HashSet<FaBudsjett>();
            FaDistriktKtNavigations = new HashSet<FaDistrikt>();
            FaDistriktKts = new HashSet<FaDistrikt>();
            FaEngasjementslinjerKt1s = new HashSet<FaEngasjementslinjer>();
            FaEngasjementslinjerKt2s = new HashSet<FaEngasjementslinjer>();
            FaEngasjementslinjerKt3s = new HashSet<FaEngasjementslinjer>();
            FaEngasjementslinjerKt4s = new HashSet<FaEngasjementslinjer>();
            FaEngasjementslinjerKtNavigations = new HashSet<FaEngasjementslinjer>();
            FaEngasjementslinjerKts = new HashSet<FaEngasjementslinjer>();
            FaKlientgruppers = new HashSet<FaKlientgrupper>();
            FaKontotiltakstypes = new HashSet<FaKontotiltakstype>();
            FaProsjekts = new HashSet<FaProsjekt>();
            FaStatsborgerskapkontos = new HashSet<FaStatsborgerskapkonto>();
            FaTiltakUiKontos = new HashSet<FaTiltakUiKonto>();
        }

        public string KtpNoekkel { get; set; }
        public string KtnKontonummer { get; set; }
        public string KtnBeskrivelse { get; set; }
        public string KtnKontotype { get; set; }
        public DateTime? KtnPassivisertdato { get; set; }

        public virtual FaKontoplan KtpNoekkelNavigation { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerKt1s { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerKt2s { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerKt3s { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerKt4s { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerKtNavigations { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingerKts { get; set; }
        public virtual ICollection<FaBetalingskategorier> FaBetalingskategoriers { get; set; }
        public virtual ICollection<FaBudsjett> FaBudsjettKt1s { get; set; }
        public virtual ICollection<FaBudsjett> FaBudsjettKt2s { get; set; }
        public virtual ICollection<FaBudsjett> FaBudsjettKt3s { get; set; }
        public virtual ICollection<FaBudsjett> FaBudsjettKt4s { get; set; }
        public virtual ICollection<FaBudsjett> FaBudsjettKtNavigations { get; set; }
        public virtual ICollection<FaBudsjett> FaBudsjettKts { get; set; }
        public virtual ICollection<FaDistrikt> FaDistriktKtNavigations { get; set; }
        public virtual ICollection<FaDistrikt> FaDistriktKts { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerKt1s { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerKt2s { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerKt3s { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerKt4s { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerKtNavigations { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjerKts { get; set; }
        public virtual ICollection<FaKlientgrupper> FaKlientgruppers { get; set; }
        public virtual ICollection<FaKontotiltakstype> FaKontotiltakstypes { get; set; }
        public virtual ICollection<FaProsjekt> FaProsjekts { get; set; }
        public virtual ICollection<FaStatsborgerskapkonto> FaStatsborgerskapkontos { get; set; }
        public virtual ICollection<FaTiltakUiKonto> FaTiltakUiKontos { get; set; }
    }
}
