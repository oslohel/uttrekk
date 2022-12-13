using System;

namespace UttrekkFamilia.Models
{
    public partial class FaDistriktloepenrserier
    {
        public string DisDistriktskode { get; set; }
        public string DlsIdent { get; set; }
        public string DlsNrserieid { get; set; }
        public string DlsNavn { get; set; }
        public DateTime? DlsPassivisertdato { get; set; }
        public string DlsBalanse { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
    }
}
