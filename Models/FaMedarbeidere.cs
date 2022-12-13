using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaMedarbeidere
    {
        public FaMedarbeidere()
        {
            FaEngasjementsavtales = new HashSet<FaEngasjementsavtale>();
            FaMedarbeiderinteressers = new HashSet<FaMedarbeiderinteresser>();
        }

        public decimal ForLoepenr { get; set; }
        public decimal? ArsIdent { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public string MedLoennsnr { get; set; }
        public string MedMerknader { get; set; }
        public DateTime MedBegyntdato { get; set; }
        public DateTime? MedSluttdato { get; set; }
        public DateTime? MedRegistrertdato { get; set; }
        public DateTime? MedEndretdato { get; set; }
        public decimal MedTilsynsfOpplaering { get; set; }
        public decimal MedStatus { get; set; }
        public DateTime? MedPolitiattest { get; set; }
        public DateTime? MedTaushetserklaering { get; set; }
        public string MedStillingid1 { get; set; }
        public string MedStillingid2 { get; set; }
        public string MedStillingid3 { get; set; }
        public string MedStillingid4 { get; set; }
        public string MedStillingid5 { get; set; }
        public string MedStilling1 { get; set; }
        public string MedStilling2 { get; set; }
        public string MedStilling3 { get; set; }
        public string MedStilling4 { get; set; }
        public string MedStilling5 { get; set; }

        public virtual FaArbeidsgiversone ArsIdentNavigation { get; set; }
        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaEngasjementsavtale> FaEngasjementsavtales { get; set; }
        public virtual ICollection<FaMedarbeiderinteresser> FaMedarbeiderinteressers { get; set; }
    }
}
