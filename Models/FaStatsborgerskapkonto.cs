namespace UttrekkFamilia.Models
{
    public partial class FaStatsborgerskapkonto
    {
        public string SbkIdent { get; set; }
        public string KtpNoekkel { get; set; }
        public string KtnKontonummer { get; set; }

        public virtual FaKontoer Kt { get; set; }
    }
}
