namespace UttrekkFamilia.Models
{
    public partial class FaKontaktpersoner
    {
        public decimal ForLoepenr { get; set; }
        public decimal KpeLoepenr { get; set; }
        public string KpeEtternavn { get; set; }
        public string KpeFornavn { get; set; }
        public string KpeTelefonarbeid { get; set; }
        public string KpeStilling { get; set; }
        public string KpeTelefonprivat { get; set; }
        public string KpeTelefonmobil { get; set; }
        public string KpeTelefaks { get; set; }

        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
    }
}
