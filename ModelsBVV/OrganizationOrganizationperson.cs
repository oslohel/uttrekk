using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell over organisasjonspersoner. I Person_Person tabellen lagres det personlig informasjon, i denne tabellen lagrer man bl.a. kontaktinformasjon for denne personen som en ansatt i en organisasjon.
    /// </summary>
    public partial class OrganizationOrganizationperson
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int OrganizationOrganizationPersonId { get; set; }
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
        /// Id til organisasjon
        /// </summary>
        public int OrganizationId { get; set; }
        /// <summary>
        /// Id til person
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Id til rolle personen har i denne organisasjonen (rektor, lærer, lege, psykolog etc.
        /// </summary>
        public int RoleRegistryId { get; set; }
        /// <summary>
        /// E-Post for denne person tilknytt oppgitt organisasjon
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Telefonnummer for denne person tilknytt oppgitt organisasjon
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Prefiks/landskode til telefonnummer for denne person tilknytt oppgitt organisasjon
        /// </summary>
        public string PhonePrefix { get; set; }
        /// <summary>
        /// Notat for denne person tilknytt oppgitt organisasjon
        /// </summary>
        public string Notes { get; set; }
    }
}
