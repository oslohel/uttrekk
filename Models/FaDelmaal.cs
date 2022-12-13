using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaDelmaal
    {
        public FaDelmaal()
        {
            TptLoepenrs = new HashSet<FaPlantiltak>();
        }

        public decimal TpdLoepenr { get; set; }
        public decimal TtpLoepenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public DateTime? TpdRegistrertdato { get; set; }
        public DateTime? TpdEndretdato { get; set; }
        public decimal? TpdPrioritet { get; set; }
        public string TpdDelmaal { get; set; }

        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaTiltaksplan TtpLoepenrNavigation { get; set; }

        public virtual ICollection<FaPlantiltak> TptLoepenrs { get; set; }
    }
}
