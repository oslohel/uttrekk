namespace UttrekkFamilia.Models
{
    public partial class FaTiltakGmltype
    {
        public decimal TilLoepenr { get; set; }
        public string TgtTiltakstype { get; set; }
        public string TgtBeskrivelse { get; set; }

        public virtual FaTiltak TilLoepenrNavigation { get; set; }
    }
}
