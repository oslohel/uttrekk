using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaRefusjonskrav
    {
        public FaRefusjonskrav()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
            FaEngasjementslinjers = new HashSet<FaEngasjementslinjer>();
            RefLoepenrs = new HashSet<FaRefusjoner>();
        }

        public decimal RerLoepenr { get; set; }
        public decimal KliLoepenr { get; set; }
        public DateTime? RerFradato { get; set; }
        public DateTime? RerTildato { get; set; }
        public decimal? PosAar { get; set; }
        public decimal? PosLoepenr { get; set; }
        public decimal RerFosArbGodBeregnet { get; set; }
        public decimal RerFosArbGivBeregnet { get; set; }
        public decimal RerFosUtgDekBeregnet { get; set; }
        public decimal RerStoBeregnet { get; set; }
        public decimal RerBesBeregnet { get; set; }
        public decimal RerMilBeregnet { get; set; }
        public decimal RerAnnBeregnet { get; set; }
        public string SbhRegistrertav { get; set; }
        public DateTime? RerRegistrertdato { get; set; }
        public string SbhEndretav { get; set; }
        public DateTime? RerEndretdato { get; set; }
        public decimal RerKomEgenandelMnd { get; set; }
        public decimal RerKomEgenandelKrav { get; set; }
        public decimal RerAndreFradrag { get; set; }
        public decimal RerStoArbGivBeregnet { get; set; }
        public decimal RerStoUtgDekBeregnet { get; set; }
        public decimal RerBesArbGivBeregnet { get; set; }
        public decimal RerBesUtgDekBeregnet { get; set; }
        public decimal RerMilArbGivBeregnet { get; set; }
        public decimal RerMilUtgDekBeregnet { get; set; }
        public decimal RerStoArbGodBeregnet { get; set; }
        public decimal RerMilArbGodBeregnet { get; set; }
        public decimal RerBesArbGodBeregnet { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaPostjournal Pos { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjers { get; set; }

        public virtual ICollection<FaRefusjoner> RefLoepenrs { get; set; }
    }
}
