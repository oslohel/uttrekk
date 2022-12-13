namespace UttrekkFamilia.Models
{
    public partial class FaFriTiltakstype
    {
        public string TttTiltakstype { get; set; }
        public string FttFriTiltakstype { get; set; }
        public string FttBeskrivelse { get; set; }

        public virtual FaTiltakstyper TttTiltakstypeNavigation { get; set; }
    }
}
