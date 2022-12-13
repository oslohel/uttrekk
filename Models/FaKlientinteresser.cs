namespace UttrekkFamilia.Models
{
    public partial class FaKlientinteresser
    {
        public decimal KliLoepenr { get; set; }
        public string IntIdent { get; set; }
        public string KinKommentar { get; set; }

        public virtual FaInteresser IntIdentNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
    }
}
