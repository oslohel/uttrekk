using System;

namespace UttrekkFamilia.Models
{
    public partial class FaAppointment
    {
        public int AppId { get; set; }
        public string AppInitialer { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string AppTitle { get; set; }
        public string AppPlace { get; set; }
        public string AppDescription { get; set; }
        public DateTime AppAppointmentStartDate { get; set; }
        public DateTime AppAppointmentEndDate { get; set; }
        public decimal? AppClientAsConnection { get; set; }
        public string AppPhonenumber { get; set; }
        public string PnrPostnr { get; set; }
        public decimal? KtkLoepenr { get; set; }

        public virtual FaKlienttilknytning K { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
    }
}
