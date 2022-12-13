using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#SYSTEM] For å knytte team til en klient. Hjelpetabell for å øke yelse ved autorisasjon
    /// </summary>
    public partial class EmployeeTeamclient
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeTeamClientId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til team
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Dato for tilknytningen mellom team og klient
        /// </summary>
        public DateTime Date { get; set; }
    }
}
