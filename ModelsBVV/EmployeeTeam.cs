using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Team
    /// </summary>
    public partial class EmployeeTeam
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeTeamId { get; set; }
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
        /// Navn på team
        /// </summary>
        public string Name { get; set; }
    }
}
