namespace UttrekkFamilia.Models
{
    public partial class FaLogglinjer
    {
        public decimal LogLoepenr { get; set; }
        public decimal LglLoepenr { get; set; }
        public decimal? LglNoekkel { get; set; }
        public decimal? LglEndret { get; set; }
        public string LglFeltnavn { get; set; }
        public string LglBeskrivelse { get; set; }
        public string LglGmlverdi { get; set; }
        public string LglNyverdi { get; set; }

        public virtual FaLogg LogLoepenrNavigation { get; set; }
    }
}
