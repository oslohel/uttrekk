namespace UttrekkFamilia.Models
{
    public partial class FaProsjektdeltInt
    {
        public decimal ProLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string PrdMerknad { get; set; }

        public virtual FaProsjekt ProLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
