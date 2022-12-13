using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaBetalingsplaner
    {
        public FaBetalingsplaner()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
        }

        public decimal UtpLoepenr { get; set; }
        public decimal? SakAar { get; set; }
        public decimal? SakJournalnr { get; set; }
        public decimal? TilLoepenr { get; set; }
        public string SbhStoppetavInitialer { get; set; }
        public string SbhKlargjortavInitialer { get; set; }
        public string SbhAvgjortavInitialer { get; set; }
        public decimal? VurLoepenr { get; set; }
        public decimal? ProLoepenr { get; set; }
        public string BktType { get; set; }
        public string BktKode { get; set; }
        public DateTime UtpVarighetfra { get; set; }
        public DateTime? UtpVarighettil { get; set; }
        public string UtpBeskrivelse { get; set; }
        public string UtpAarsaker { get; set; }
        public DateTime? UtpStoppetdato { get; set; }
        public DateTime? UtpKlargjortdato { get; set; }
        public DateTime? UtpAvgjortdato { get; set; }
        public string UtpStatus { get; set; }
        public string UtpFormaal { get; set; }
        public decimal? UtpSumGodkjent { get; set; }
        public string UtpGmlreferanse { get; set; }
        public decimal? RefLoepenr { get; set; }

        public virtual FaBetalingskategorier Bkt { get; set; }
        public virtual FaProsjekt ProLoepenrNavigation { get; set; }
        public virtual FaRefusjoner RefLoepenrNavigation { get; set; }
        public virtual FaSaksjournal Sak { get; set; }
        public virtual FaSaksbehandlere SbhAvgjortavInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhKlargjortavInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhStoppetavInitialerNavigation { get; set; }
        public virtual FaTiltak TilLoepenrNavigation { get; set; }
        public virtual FaVurdegenbet VurLoepenrNavigation { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
    }
}
