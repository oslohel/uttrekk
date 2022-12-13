using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tilhørighet for klientgruppe
    /// </summary>
    public partial class ClientClientgrouporg
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int ClientClientGroupOrgId { get; set; }
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
        /// Id til klientgruppen
        /// </summary>
        public int ClientGroupId { get; set; }
        /// <summary>
        /// Id til tilhørighet(nivå i organisasjonsstruktur)
        /// </summary>
        public int CustomerOrganizationId { get; set; }
    }
}
