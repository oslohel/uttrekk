namespace UttrekkFamilia.Models
{
    public partial class FaMedarbeiderinteresser
    {
        public decimal ForLoepenr { get; set; }
        public string IntIdent { get; set; }
        public string MinKommentar { get; set; }

        public virtual FaMedarbeidere ForLoepenrNavigation { get; set; }
        public virtual FaInteresser IntIdentNavigation { get; set; }
    }
}
