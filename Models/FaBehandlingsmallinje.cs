namespace UttrekkFamilia.Models
{
    public partial class FaBehandlingsmallinje
    {
        public decimal BelLoepenr { get; set; }
        public decimal BemLoepenr { get; set; }
        public string AkkType { get; set; }
        public string SbhInitialer { get; set; }
        public string BelMerknad { get; set; }
        public decimal? BelFristEtterStart { get; set; }
        public decimal BelIntern { get; set; }

        public virtual FaAktivitetskode AkkTypeNavigation { get; set; }
        public virtual FaBehandlingsmal BemLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
