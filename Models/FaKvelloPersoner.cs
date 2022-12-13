namespace UttrekkFamilia.Models
{
    public partial class FaKvelloPersoner
    {
        public decimal KveLoepenr { get; set; }
        public decimal KtkLoepenr { get; set; }

        public virtual FaKvello KveLoepenrNavigation { get; set; }
    }
}
