using System;

namespace UttrekkFamilia.Models
{
    public partial class FaAktivitet
    {
        public decimal AkvLoepenr { get; set; }
        public decimal GsaAar { get; set; }
        public decimal GsaJournalnr { get; set; }
        public string AkkType { get; set; }
        public string SbhInitialer { get; set; }
        public decimal? PosAar { get; set; }
        public decimal? PosLoepenr { get; set; }
        public string AkvMerknad { get; set; }
        public DateTime? AkvFrist { get; set; }
        public DateTime? AkvUtfoert { get; set; }

        public virtual FaAktivitetskode AkkTypeNavigation { get; set; }
        public virtual FaGenerellsak Gsa { get; set; }
        public virtual FaPostjournal Pos { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
