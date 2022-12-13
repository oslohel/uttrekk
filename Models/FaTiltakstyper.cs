using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaTiltakstyper
    {
        public FaTiltakstyper()
        {
            FaFriTiltakstypes = new HashSet<FaFriTiltakstype>();
            FaPlantiltaks = new HashSet<FaPlantiltak>();
            FaTiltaks = new HashSet<FaTiltak>();
        }

        public string TttTiltakstype { get; set; }
        public string TttBeskrivelse { get; set; }
        public DateTime? TttPassivisertdato { get; set; }
        public string TttSsbkode { get; set; }
        public string TttKategori { get; set; }

        public virtual ICollection<FaFriTiltakstype> FaFriTiltakstypes { get; set; }
        public virtual ICollection<FaPlantiltak> FaPlantiltaks { get; set; }
        public virtual ICollection<FaTiltak> FaTiltaks { get; set; }
    }
}
