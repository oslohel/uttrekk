using System;

namespace UttrekkFamilia.Models
{
    public partial class FaTekstmaler
    {
        public string TmaMaltype { get; set; }
        public string TmaNavn { get; set; }
        public decimal? DokLoepenr { get; set; }
        public string TmaBeskrivelse { get; set; }
        public DateTime? TmaPassivisertdato { get; set; }
        public string TmaGruppe { get; set; }
        public decimal TmaCkeditor { get; set; }
        public Guid? TmaId { get; set; }
        public Guid? TmaBasedontemplateid { get; set; }
        public Guid? TmaHeadertemplateid { get; set; }
        public int? TmaHeaderpagesetting { get; set; }
        public Guid? TmaFootertemplateid { get; set; }
        public int? TmaFooterpagesetting { get; set; }
        public Guid? TmaSecondheadertemplateid { get; set; }
        public int? TmaSecondheaderpagesetting { get; set; }
        public Guid? TmaSecondfooteremplateid { get; set; }
        public int? TmaSecondfooterpagesetting { get; set; }
        public int? TmaBasetype { get; set; }
        public string TmaPassivisertav { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
    }
}
