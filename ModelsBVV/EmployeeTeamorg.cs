using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Knytning mellom team og organisasjonsstruktur(tilhørighet)
    /// </summary>
    public partial class EmployeeTeamorg
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeTeamOrgId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Aktiv fra
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Aktiv til
        /// </summary>
        public DateTime? StopDate { get; set; }
        /// <summary>
        /// Deaktivert av
        /// </summary>
        public int? StoppedBy { get; set; }
        /// <summary>
        /// Id til team
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// Id til organisasjonsstruktur(tilhørighet)
        /// </summary>
        public int CustomerOrganizationId { get; set; }
    }
}
