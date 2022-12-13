namespace UttrekkFamilia.Models
{
    public partial class FaInntutg
    {
        public decimal InuLoepenr { get; set; }
        public decimal VurLoepenr { get; set; }
        public string IutType { get; set; }
        public string IutKode { get; set; }
        public string InuHvem { get; set; }
        public decimal InuBeloep { get; set; }
        public string InuMerknad { get; set; }

        public virtual FaInntutgtype Iut { get; set; }
        public virtual FaVurdegenbet VurLoepenrNavigation { get; set; }
    }
}
