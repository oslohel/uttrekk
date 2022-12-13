namespace UttrekkFamilia.Models
{
    public partial class FaSaksbehKlient
    {
        public int Id { get; set; }
        public string SbhInitialer { get; set; }
        public decimal KliLoepenr { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
