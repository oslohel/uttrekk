using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Knytning mellom team og klientgrupper
    /// </summary>
    public partial class EmployeeTeamclientgroup
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeTeamClientGroupId { get; set; }
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
        /// Id til klientgruppe
        /// </summary>
        public int ClientClientGroupId { get; set; }
        public int ClientGroupId { get; set; }
    }
}
