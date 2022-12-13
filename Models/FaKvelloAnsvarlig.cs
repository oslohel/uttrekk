namespace UttrekkFamilia.Models
{
    public partial class FaKvelloAnsvarlig
    {
        public decimal KvaLoepenr { get; set; }
        public decimal KveLoepenr { get; set; }
        public string KvaNavn { get; set; }
        public string KvaMandat { get; set; }
        public string KvaTittel { get; set; }
        public string KvaInstitusjon { get; set; }

        public virtual FaKvello KveLoepenrNavigation { get; set; }
    }
}
