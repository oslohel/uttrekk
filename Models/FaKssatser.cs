using System;

namespace UttrekkFamilia.Models
{
    public partial class FaKssatser
    {
        public string KsiIdent { get; set; }
        public DateTime KssFradato { get; set; }
        public DateTime? KssTildato { get; set; }
        public decimal KssArbeidsgodtgjoerelse { get; set; }
        public decimal KssUtgiftsdekning { get; set; }

        public virtual FaKsIdenter KsiIdentNavigation { get; set; }
    }
}
