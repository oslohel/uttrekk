using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaPlantiltak
    {
        public FaPlantiltak()
        {
            TpdLoepenrs = new HashSet<FaDelmaal>();
        }

        public decimal TptLoepenr { get; set; }
        public decimal TtpLoepenr { get; set; }
        public string TttTiltakstype { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public DateTime? TptRegistrertdato { get; set; }
        public DateTime? TptEndretdato { get; set; }
        public string TptTiltak { get; set; }
        public DateTime? TptFradato { get; set; }
        public DateTime? TptTildato { get; set; }

        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaTiltaksplan TtpLoepenrNavigation { get; set; }
        public virtual FaTiltakstyper TttTiltakstypeNavigation { get; set; }

        public virtual ICollection<FaDelmaal> TpdLoepenrs { get; set; }
    }
}
