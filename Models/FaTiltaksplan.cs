using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaTiltaksplan
    {
        public FaTiltaksplan()
        {
            FaDelmaals = new HashSet<FaDelmaal>();
            FaPlantiltaks = new HashSet<FaPlantiltak>();
            FaTiltaks = new HashSet<FaTiltak>();
            FaTiltaksplanevalueringers = new HashSet<FaTiltaksplanevalueringer>();
        }

        public decimal TtpLoepenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public decimal KliLoepenr { get; set; }
        public decimal? DokLoepenr { get; set; }
        public string PtyPlankode { get; set; }
        public string SbhInitialer { get; set; }
        public DateTime TtpRegistrertdato { get; set; }
        public string TtpHovedmaal { get; set; }
        public DateTime? TtpFradato { get; set; }
        public DateTime? TtpTildato { get; set; }
        public decimal? TtpDokumentnr { get; set; }
        public DateTime? TtpEndretdato { get; set; }
        public DateTime? TtpFerdigdato { get; set; }
        public DateTime? TtpAvsluttdato { get; set; }
        public decimal? TtpEttdokument { get; set; }
        public decimal? ArkTiltakSystemid { get; set; }
        public DateTime? ArkDato { get; set; }
        public decimal ArkStopp { get; set; }
        public decimal ArkSjekkIVsa { get; set; }
        public string TtpNote { get; set; }
        public string TtpBegrSlettet { get; set; }
        public decimal TtpSlettet { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaPlantype PtyPlankodeNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaDelmaal> FaDelmaals { get; set; }
        public virtual ICollection<FaPlantiltak> FaPlantiltaks { get; set; }
        public virtual ICollection<FaTiltak> FaTiltaks { get; set; }
        public virtual ICollection<FaTiltaksplanevalueringer> FaTiltaksplanevalueringers { get; set; }
    }
}
