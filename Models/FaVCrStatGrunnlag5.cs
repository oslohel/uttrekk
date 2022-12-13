using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrStatGrunnlag5
    {
        public decimal KliLoepenr { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public DateTime? FraDato { get; set; }
        public string Punkt { get; set; }
        public DateTime? KtkFradato { get; set; }
        public DateTime? KtkTildato { get; set; }
    }
}
