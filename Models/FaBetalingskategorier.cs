using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaBetalingskategorier
    {
        public FaBetalingskategorier()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
            FaBetalingsplaners = new HashSet<FaBetalingsplaner>();
            FaEngasjementsplans = new HashSet<FaEngasjementsplan>();
            InverseBktMotpost = new HashSet<FaBetalingskategorier>();
        }

        public string BktType { get; set; }
        public string BktKode { get; set; }
        public string KtpNoekkel { get; set; }
        public string KtnKontonummer { get; set; }
        public string BktMotpostType { get; set; }
        public string BktMotpostKode { get; set; }
        public string TtkKodeRegulativ { get; set; }
        public string TtkKodeIndividuell { get; set; }
        public string BktBeskrivelse { get; set; }
        public decimal BktOverfoeresregnskap { get; set; }
        public string BktMerknad { get; set; }
        public DateTime? BktPassivisertdato { get; set; }
        public string BktResultatbalanse { get; set; }
        public decimal BktFeriepengeberegning { get; set; }
        public decimal BktArbavgberegning { get; set; }

        public virtual FaBetalingskategorier BktMotpost { get; set; }
        public virtual FaKontoer Kt { get; set; }
        public virtual FaTtkoder TtkKodeIndividuellNavigation { get; set; }
        public virtual FaTtkoder TtkKodeRegulativNavigation { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplaners { get; set; }
        public virtual ICollection<FaEngasjementsplan> FaEngasjementsplans { get; set; }
        public virtual ICollection<FaBetalingskategorier> InverseBktMotpost { get; set; }
    }
}
