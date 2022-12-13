using System;

namespace UttrekkFamilia.Models
{
    public partial class FaStatTabell242Ttperioder
    {
        public DateTime P242PeriodeStartdato { get; set; }
        public DateTime P242PeriodeSluttdato { get; set; }
        public decimal KliLoepenr { get; set; }
        public string P242Distrikter { get; set; }
        public decimal P242Loepenr { get; set; }
        public string TttTiltakstype { get; set; }
        public decimal? TilUtenforhjemmet { get; set; }
        public string TilKategori { get; set; }
        public string LovHovedparagraf { get; set; }
        public DateTime? P242Fradato { get; set; }
        public DateTime? P242Tildato { get; set; }
        public decimal? P242AntallDoegn { get; set; }
        public string TttSsbkode { get; set; }
        public string P242Klientgrupper { get; set; }
        public string P242Saksbehandlere { get; set; }
        public string P242Distriktsnavn { get; set; }
    }
}
