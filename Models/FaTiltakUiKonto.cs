namespace UttrekkFamilia.Models
{
    public partial class FaTiltakUiKonto
    {
        public string UikIdent { get; set; }
        public string KtpNoekkel { get; set; }
        public string KtnKontonummer { get; set; }

        public virtual FaKontoer Kt { get; set; }
    }
}
