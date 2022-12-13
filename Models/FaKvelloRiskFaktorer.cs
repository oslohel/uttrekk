namespace UttrekkFamilia.Models
{
    public partial class FaKvelloRiskFaktorer
    {
        public decimal KveLoepenr { get; set; }
        public decimal KrfType { get; set; }

        public virtual FaKvello KveLoepenrNavigation { get; set; }
    }
}
