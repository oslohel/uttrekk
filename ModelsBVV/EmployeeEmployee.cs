using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for ansatte
    /// </summary>
    public partial class EmployeeEmployee
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeEmployeeId { get; set; }
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
        /// Fornavn
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Etternavn
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Mellomnavn
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Fødselsnummer
        /// </summary>
        public string BirthNumber { get; set; }
        /// <summary>
        /// DNummer
        /// </summary>
        public string Dnumber { get; set; }
        /// <summary>
        /// Adresse
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Id til postnummer
        /// </summary>
        public int? PostCodeRegistryId { get; set; }
        /// <summary>
        /// Prefiks til mobilnummer
        /// </summary>
        public string CellPhonePrefix { get; set; }
        /// <summary>
        /// Mobilnummer
        /// </summary>
        public string CellPhone { get; set; }
        /// <summary>
        /// Prefiks til telefonnummer
        /// </summary>
        public string PhonePrefix { get; set; }
        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// E-Post adresse
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Initialer
        /// </summary>
        public string Initials { get; set; }
    }
}
