using System;

namespace UttrekkFamilia.Models
{
    public partial class FaMerkantilfil
    {
        public string FilIdent { get; set; }
        public decimal FilLoepenr { get; set; }
        public decimal TelTeller { get; set; }
        public DateTime? FilProdusertdato { get; set; }
        public string FilRemFoersteProdnr { get; set; }
        public string FilRemSisteProdnr { get; set; }
        public string FilRemFoersteSekvnr { get; set; }
        public string FilRemSisteSekvnr { get; set; }
        public string FilSystem { get; set; }
        public string FilNavn { get; set; }

        public virtual FaTeller TelTellerNavigation { get; set; }
    }
}
