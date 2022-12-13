using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell over Organisasjoner (bedrifter, kommuner, etc) 
    /// </summary>
    public partial class OrganizationOrganization
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int OrganizationOrganizationId { get; set; }
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
        /// Navn på organisasjonen
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Id til Type organisasjon (grunnregister)
        /// </summary>
        public int? OrganizationType { get; set; }
        /// <summary>
        /// Organisasjonsnummer
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Organisasjonsnummer brukt i elektronisk kommunikasjon(sikker post/svarut)
        /// </summary>
        public string NumberForCorrespondence { get; set; }
        /// <summary>
        /// Adresse
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Id til postnummer for adresse
        /// </summary>
        public int? AddressPostCodeRegistryId { get; set; }
        /// <summary>
        /// Id til kommunenr
        /// </summary>
        public int? MunicipalityRegistryId { get; set; }
        /// <summary>
        /// Besøksadresse
        /// </summary>
        public string VisitAddress { get; set; }
        /// <summary>
        /// Id til postnummer for besøksadresse
        /// </summary>
        public int? VisitAddressPostCodeRegistryId { get; set; }
        /// <summary>
        /// E-Postadresse
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Prefiks/landskode for telefonnummer
        /// </summary>
        public string PhonePrefix { get; set; }
        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Prefiks/landskode for sekundært telefonnummer
        /// </summary>
        public string SecondaryPhonePrefix { get; set; }
        /// <summary>
        /// Sekundært telefonnummer
        /// </summary>
        public string SecondaryPhone { get; set; }
        /// <summary>
        /// Id til SvarUt forsendelsestype som skal benyttes ved sending til SvarUt for denne organisasjonen
        /// </summary>
        public int? ShipmentTypeRegistryId { get; set; }
        /// <summary>
        /// Angir om man skal inkludere fødselsnummer ved SvarUt forsendelser til denne organisasjonen
        /// </summary>
        public bool? SvarUtIncludeBirthNumber { get; set; }
        public string AccountNumber { get; set; }
    }
}
