using System;

namespace UttrekkFamilia.Models
{
    public partial class FaForbindelsesadresser
    {
        public decimal ForLoepenr { get; set; }
        public decimal FoaLoepenr { get; set; }
        public string SbhEndretav { get; set; }
        public string PnrPostnr { get; set; }
        public decimal? KomKommunenr { get; set; }
        public string FoaPostadresse { get; set; }
        public string FoaBesoeksadresse { get; set; }
        public DateTime? FoaPassivisertdato { get; set; }

        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
    }
}
