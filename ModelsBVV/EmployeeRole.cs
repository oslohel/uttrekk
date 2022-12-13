using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Roller for tilgangskontroll
    /// </summary>
    public partial class EmployeeRole
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeRoleId { get; set; }
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
        /// Navn på rollen
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Angir tilgangene for rollen
        /// </summary>
        public string AccessPoints { get; set; }
        /// <summary>
        /// Angir om ansatt må autentisere seg på nytt når man bytter til denne rollen
        /// </summary>
        public bool ReauthenticationRequired { get; set; }
        /// <summary>
        /// Bestemmer om man må angi en grunn når man bytter til denne rollen
        /// </summary>
        public bool ReasonRequired { get; set; }
        /// <summary>
        /// Angir om denne rollen er en rolle relatert til helseinformasjon (EPJ)
        /// </summary>
        public bool IsHealthrole { get; set; }
        /// <summary>
        /// Angir om ansatte med denne rollen har tilgang til godkjenning
        /// </summary>
        public bool ApprovalAuthority { get; set; }
        /// <summary>
        /// Oppdatert av (ansatt)
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Opprettet dato
        /// </summary>
        public DateTime? CreatedDate { get; set; }
    }
}
