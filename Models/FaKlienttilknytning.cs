using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKlienttilknytning
    {
        public FaKlienttilknytning()
        {
            FaAppointments = new HashSet<FaAppointment>();
        }

        public decimal KliLoepenr { get; set; }
        public decimal KtkLoepenr { get; set; }
        public decimal ForLoepenr { get; set; }
        public string KtkRolle { get; set; }
        public DateTime? KtkDoeddato { get; set; }
        public decimal KtkForeldreansvar { get; set; }
        public decimal KtkDagligomsorg { get; set; }
        public decimal KtkForesatt { get; set; }
        public decimal KtkHovedperson { get; set; }
        public string KtkMerknad { get; set; }
        public string KtkNypartner { get; set; }
        public DateTime? KtkFradato { get; set; }
        public DateTime? KtkTildato { get; set; }
        public string KtkPartsrettighet { get; set; }
        public decimal KtkDeltbosted { get; set; }
        public string KtkRolleKategori { get; set; }
        public string KtkPartsrettighetKategori { get; set; }

        public virtual FaForbindelser ForLoepenrNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual ICollection<FaAppointment> FaAppointments { get; set; }
    }
}
