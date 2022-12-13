namespace UttrekkFamilia.Models
{
    public partial class FaTilgangsgrVindu
    {
        public string VinUtvnavn { get; set; }
        public string TggIdent { get; set; }
        public string TgvRettighet { get; set; }

        public virtual FaTilgangsgrupper TggIdentNavigation { get; set; }
        public virtual FaVindu VinUtvnavnNavigation { get; set; }
    }
}
