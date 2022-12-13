using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Knytning mellom team, rolle og ansatt som gir en ansatt tilgang til klienter
    /// </summary>
    public partial class EmployeeEmployeeteamrole
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeEmployeeTeamRoleId { get; set; }
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
        /// Id til ansatt
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Id til team
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// Id til rolle
        /// </summary>
        public int RoleId { get; set; }
    }
}
