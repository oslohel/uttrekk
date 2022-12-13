using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for saksstatuser, en sak kan ha ulike statuser over tid og alle statuser en sak har hatt er registrert her. Siste statuser er også lagret i Case_Case
    /// </summary>
    public partial class CaseCasestatus
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int CaseCaseStatusId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Saksid, referanse til primærnøkkel til saken
        /// </summary>
        public int CaseId { get; set; }
        /// <summary>
        /// Type status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Status start dato
        /// </summary>
        public DateTime StatusDate { get; set; }
        /// <summary>
        /// Type sluttstatus
        /// </summary>
        public int? EndStatus { get; set; }
        /// <summary>
        /// Dato for slutt status
        /// </summary>
        public DateTime? EndStatusDate { get; set; }

        public virtual EnumCasestatus EndStatusNavigation { get; set; }
        public virtual EnumCasestatus StatusNavigation { get; set; }
    }
}
