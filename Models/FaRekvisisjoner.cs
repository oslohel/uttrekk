using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaRekvisisjoner
    {
        public FaRekvisisjoner()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
        }

        public decimal RekAar { get; set; }
        public decimal RekLoepenr { get; set; }
        public decimal? DokLoepenr { get; set; }
        public decimal SakAar { get; set; }
        public decimal SakJournalnr { get; set; }
        public string SbhInitialer { get; set; }
        public decimal? ForLoepenr { get; set; }
        public string RekGjelder { get; set; }
        public decimal RekMaksbeloep { get; set; }
        public string RekMaksbeloeptekst { get; set; }
        public DateTime? RekDato { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
        public virtual FaSaksjournal Sak { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
    }
}
