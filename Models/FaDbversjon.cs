using System;

namespace UttrekkFamilia.Models
{
    public partial class FaDbversjon
    {
        public decimal DbvLoepenr { get; set; }
        public decimal DbvVersjon { get; set; }
        public DateTime DbvVaardato { get; set; }
        public DateTime DbvDbdato { get; set; }
        public string DbvFritekst { get; set; }
    }
}
