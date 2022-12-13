namespace UttrekkFamilia.Models
{
    public partial class FaMarkedclientelement
    {
        public int MceId { get; set; }
        public string MceFlowElementId { get; set; }
        public int MceFlowElementType { get; set; }
        public string SbhInitialer { get; set; }
        public decimal KliLoepenr { get; set; }
        public int? MceFlowElementAar { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
