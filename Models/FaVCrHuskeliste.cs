using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrHuskeliste
    {
        public DateTime? Frist { get; set; }
        public string Type { get; set; }
        public string SbhInitialer { get; set; }
        public string Emne { get; set; }
        public string Oppfoelging { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string Klientnavn { get; set; }
        public decimal Loepenr { get; set; }
        public DateTime? Oppfyltdato { get; set; }
        public decimal Aar { get; set; }
        public decimal? Loepenr2 { get; set; }
        public string Ferdigdato { get; set; }
        public string SbhInitialer2 { get; set; }
    }
}
