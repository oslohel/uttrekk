using System;

namespace UttrekkFamilia.Models
{
    public partial class FaAvvikslogg
    {
        public string AvlGuid { get; set; }
        public DateTime AvlLoggtidspunkt { get; set; }
        public string AvlSbhOperatoerinitialer { get; set; }
        public string AvlSbhOperatoernavn { get; set; }
        public string AvlOppslagstype { get; set; }
        public string AvlLoggdetaljer { get; set; }
        public decimal? AvlId { get; set; }
        public decimal? AvlId2 { get; set; }
        public string AvlSbh1 { get; set; }
        public string AvlSbh2 { get; set; }
        public string AvlReferanse { get; set; }
        public DateTime? AvlLoggtidspunkt2 { get; set; }
    }
}
