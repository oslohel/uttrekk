using System;

namespace UttrekkFamilia.Models
{
    public partial class FaStatTabell23G
    {
        public DateTime G23PeriodeStartdato { get; set; }
        public DateTime G23PeriodeSluttdato { get; set; }
        public decimal KliLoepenr { get; set; }
        public string G23Distrikter { get; set; }
        public string G23Punkt { get; set; }
        public decimal G23Teller { get; set; }
        public string G23Klientgrupper { get; set; }
        public string G23Saksbehandlere { get; set; }
        public string G23Distriktsnavn { get; set; }
    }
}
