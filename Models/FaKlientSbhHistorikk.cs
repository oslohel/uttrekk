using System;

namespace UttrekkFamilia.Models
{
    public partial class FaKlientSbhHistorikk
    {
        public decimal KliLoepenr { get; set; }
        public decimal KshLoepenr { get; set; }
        public string SbhInitialer1New { get; set; }
        public string KshSbh1Newname { get; set; }
        public string SbhInitialer2New { get; set; }
        public string KshSbh2Newname { get; set; }
        public string SbhInitialer1Old { get; set; }
        public string KshSbh1Oldname { get; set; }
        public string SbhInitialer2Old { get; set; }
        public string KshSbh2Oldname { get; set; }
        public string SbhEndretav { get; set; }
        public DateTime? KshEndretdato { get; set; }
    }
}
