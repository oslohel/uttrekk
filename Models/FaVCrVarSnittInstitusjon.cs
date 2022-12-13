using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVCrVarSnittInstitusjon
    {
        public decimal KliLoepenr { get; set; }
        public DateTime? KliAvsluttetdato { get; set; }
        public string DisDistriktskode { get; set; }
        public string KliFornavn { get; set; }
        public string KliEtternavn { get; set; }
        public decimal TilLoepenr { get; set; }
        public string TttSsbkode { get; set; }
        public string TttTiltakstype { get; set; }
        public string LovHovedParagraf { get; set; }
        public string LovJmfParagraf1 { get; set; }
        public string LovJmfParagraf2 { get; set; }
        public DateTime? TilIverksattdato { get; set; }
        public DateTime? TilAvsluttetdato { get; set; }
        public string TttBeskrivelse { get; set; }
        public string KodKorttekst { get; set; }
    }
}
