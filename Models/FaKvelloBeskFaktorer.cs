namespace UttrekkFamilia.Models
{
    public partial class FaKvelloBeskFaktorer
    {
        public decimal KveLoepenr { get; set; }
        public decimal KbfType { get; set; }

        public virtual FaKvello KveLoepenrNavigation { get; set; }
    }
}
