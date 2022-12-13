using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrTilsynsfoerer
    {
        public decimal KliLoepenr { get; set; }
        public string KtkRolle { get; set; }
        public DateTime? KtkFradato { get; set; }
        public DateTime? KtkTildato { get; set; }
        public decimal ForLoepenr { get; set; }
        public string ForEtternavn { get; set; }
        public string ForFornavn { get; set; }
    }
}
