using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell over klientgrupper
    /// </summary>
    public partial class ClientClientgroup
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int ClientClientGroupId { get; set; }
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
        /// Navn på klientgruppe
        /// </summary>
        public string Name { get; set; }
    }
}
