using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaEngasjementsplan
    {
        public FaEngasjementsplan()
        {
            FaEngasjementslinjers = new HashSet<FaEngasjementslinjer>();
        }

        public decimal EnpLoepenr { get; set; }
        public decimal EngLoepenr { get; set; }
        public string BktType { get; set; }
        public string BktKode { get; set; }
        public string SbhKlargjortavInitialer { get; set; }
        public string SbhStoppetavInitialer { get; set; }
        public string EnpPlantype { get; set; }
        public string EnpOppgaver { get; set; }
        public string EnpBeskrivelse { get; set; }
        public DateTime? EnpStoppetdato { get; set; }
        public DateTime? EnpKlargjortdato { get; set; }
        public DateTime? EnpAvgjortdato { get; set; }
        public string EnpStatus { get; set; }
        public DateTime EnpVarighetfra { get; set; }
        public string EnpFormaal { get; set; }
        public DateTime? EnpVarighettil { get; set; }
        public string EnpAarsaker { get; set; }
        public decimal? EnpSumGodkjent { get; set; }
        public string SbhAvgjortavInitialer { get; set; }
        public decimal? RefLoepenr { get; set; }

        public virtual FaBetalingskategorier Bkt { get; set; }
        public virtual FaEngasjementsavtale EngLoepenrNavigation { get; set; }
        public virtual FaRefusjoner RefLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhAvgjortavInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhKlargjortavInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhStoppetavInitialerNavigation { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjers { get; set; }
    }
}
