using System;

namespace UttrekkFamilia.Models
{
    public partial class FaLoennstrinnsat
    {
        public string LoeTrinn { get; set; }
        public DateTime LosFradato { get; set; }
        public DateTime? LosTildato { get; set; }
        public decimal LosAarsloenn { get; set; }
        public decimal LosKveldnattillegg { get; set; }
        public decimal? LosTimeloenn { get; set; }

        public virtual FaLoennstrinn LoeTrinnNavigation { get; set; }
    }
}
