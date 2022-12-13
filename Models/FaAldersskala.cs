using System;

namespace UttrekkFamilia.Models
{
    public partial class FaAldersskala
    {
        public string AlsIdent { get; set; }
        public string LoeTrinn { get; set; }
        public string AlsBeskrivelse { get; set; }
        public decimal AlsFraaar { get; set; }
        public decimal? AlsTilaar { get; set; }
        public DateTime? AlsPassivisertdato { get; set; }

        public virtual FaLoennstrinn LoeTrinnNavigation { get; set; }
    }
}
