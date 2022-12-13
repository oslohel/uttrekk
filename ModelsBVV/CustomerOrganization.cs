using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Organisasjonsstruktur som også benyttes for å angi tilhørighet for ulike data i systemet.
    /// </summary>
    public partial class CustomerOrganization
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int CustomerOrganizationId { get; set; }
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
        /// Id til organisasjon som ligger over i organisasjonsstrukturen
        /// </summary>
        public int? ParentOrganizationId { get; set; }
        /// <summary>
        /// Navn på organisasjon/organisasjonsnivå
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Type organisasjon
        /// </summary>
        public int OrgType { get; set; }
        /// <summary>
        /// Id som benyttes for integrasjon mot ID-porten (benyttes for autentisering)
        /// </summary>
        public string IdportalIdentity { get; set; }
        /// <summary>
        /// Id som benyttes for Visma Connect integrasjon (benyttes for autentisering)
        /// </summary>
        public string VismaConnectIdentity { get; set; }
        /// <summary>
        /// Angir om Visma Connect integrasjon er aktivert
        /// </summary>
        public bool? VismaConnectEnabled { get; set; }
        /// <summary>
        /// Angir om &amp;apos;single sign on&amp;apos; er aktivert
        /// </summary>
        public bool? SingleSignOnEnabled { get; set; }
        /// <summary>
        /// Angir om kartlegging er aktivert for denne kunden
        /// </summary>
        public bool? ActivateAssessment { get; set; }
        /// <summary>
        /// Kommunenummer
        /// </summary>
        public string MunicipalityNumber { get; set; }
        /// <summary>
        /// Adresse
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Id til postnr
        /// </summary>
        public int? AddressPostCodeRegistryId { get; set; }
        /// <summary>
        /// Besøksadresse
        /// </summary>
        public string VisitAddress { get; set; }
        /// <summary>
        /// Id til postnummer for besøksadresse
        /// </summary>
        public int? VisitAddressPostCodeRegistryId { get; set; }
        /// <summary>
        /// Prefisk for telefonnummer, f.eks. +47
        /// </summary>
        public string PhonePrefix { get; set; }
        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// E-post
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Organisasjonsnummer
        /// </summary>
        public string OrganizationNumber { get; set; }
        /// <summary>
        /// Kode for organisasjonen
        /// </summary>
        public string OrganizationCode { get; set; }
        /// <summary>
        /// Kontaktperson (fritekst)
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// Kontonummer
        /// </summary>
        public string BankAccountNumber { get; set; }
        /// <summary>
        /// CRM nummer er et internt id nummer som Visma bruker, alle kunder er registert i et CRM verktøy.
        /// </summary>
        public string CrmNumber { get; set; }
        /// <summary>
        /// Om &amp;apos;active directory&amp;apos; er aktivert, benyttet i samband med autentisering
        /// </summary>
        public bool? ActiveDirectoryEnabled { get; set; }
        /// <summary>
        /// Active directory domener
        /// </summary>
        public string ActiveDirectoryDomains { get; set; }
        public bool? SamspillIntegrationEnabled { get; set; }
        public bool? IdPortenEnabled { get; set; }

        public virtual EnumOrgtype OrgTypeNavigation { get; set; }
    }
}
