using System;

namespace UttrekkFamilia.Models
{
    public partial class FaArbeidsgiveravgift
    {
        public decimal ArsIdent { get; set; }
        public DateTime AraFradato { get; set; }
        public DateTime? AraTildato { get; set; }
        public decimal AraSats { get; set; }
        public decimal? AraFeriepengesats { get; set; }

        public virtual FaArbeidsgiversone ArsIdentNavigation { get; set; }
    }
}
