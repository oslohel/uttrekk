using System;

namespace UttrekkFamilia.Models
{
    public partial class FaKlientadresser
    {
        public decimal KliLoepenr { get; set; }
        public decimal PahLoepenr { get; set; }
        public decimal? KomKommunenr { get; set; }
        public string SbhEndretav { get; set; }
        public string PnrPostnr { get; set; }
        public string PahAdresse { get; set; }
        public DateTime? PahPassivisertdato { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
    }
}
