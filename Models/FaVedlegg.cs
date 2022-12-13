using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVedlegg
    {
        public decimal VedLoepenr { get; set; }
        public decimal? DokLoepenr { get; set; }
        public decimal? PosAar { get; set; }
        public decimal? PosLoepenr { get; set; }
        public string VedSubject { get; set; }
        public DateTime? VedVedleggDato { get; set; }
        public Guid VedSvarinnRef { get; set; }
        public string VedMetaData { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaPostjournal Pos { get; set; }
    }
}
