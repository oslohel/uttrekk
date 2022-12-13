namespace UttrekkFamilia.Models
{
    public partial class FaProsjektdeltEk
    {
        public decimal ProLoepenr { get; set; }
        public decimal ForLoepenr { get; set; }
        public string PrfMerknad { get; set; }

        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
        public virtual FaProsjekt ProLoepenrNavigation { get; set; }
    }
}
