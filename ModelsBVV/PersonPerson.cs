using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell over personer, inneholder personlig informasjon om alle personer i løsningen(klienter, nettverkspersoner, organisasjonspersoner, familiapersoner). Merk at ansatte har en egen tabell
    /// </summary>
    public partial class PersonPerson
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int PersonPersonId { get; set; }
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
        /// Kallenavn/kortnavn
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// Fødselsdato
        /// </summary>
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// Fødselsnummer
        /// </summary>
        public string BirthNumber { get; set; }
        /// <summary>
        /// Dnummer
        /// </summary>
        public string Dnumber { get; set; }
        /// <summary>
        /// DUFnummer
        /// </summary>
        public string Dufnumber { get; set; }
        /// <summary>
        /// HNummer
        /// </summary>
        public string Hnumber { get; set; }
        /// <summary>
        /// Adresse
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Angir om det er hemmelig adresse
        /// </summary>
        public bool IsSecretAddress { get; set; }
        /// <summary>
        /// Angir om det er ekstra hemmelig adresse
        /// </summary>
        public bool IsHighlyRestricted { get; set; }
        /// <summary>
        /// E-Postadresse
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Notat
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Prefiks/landskode for telefonnummer
        /// </summary>
        public string PhonePrefix { get; set; }
        /// <summary>
        /// Sekundært telefonnummer
        /// </summary>
        public string SecondaryPhone { get; set; }
        /// <summary>
        /// Prefiks/landskode til sekundært telefonnummer
        /// </summary>
        public string SecondaryPhonePrefix { get; set; }
        /// <summary>
        /// Id til postnummer
        /// </summary>
        public int? PostCodeRegistryId { get; set; }
        /// <summary>
        /// Id til kommune for adresse
        /// </summary>
        public int? MunicipalityRegistryId { get; set; }
        /// <summary>
        /// Angir om personen er en klient og dermed har en rad i CLIENT_CLIENT tabellen også
        /// </summary>
        public bool IsClient { get; set; }
        /// <summary>
        /// Dato for død
        /// </summary>
        public DateTime? DeathDate { get; set; }
        /// <summary>
        /// Besøksadresse
        /// </summary>
        public string VisitingAddress { get; set; }
        /// <summary>
        /// Id til postnummer for besøksadresse
        /// </summary>
        public int? VisitingAddressPostCodeRegistryId { get; set; }
        /// <summary>
        /// Id til kommune for besøksadresse
        /// </summary>
        public int? VisitingAddressMunicipalityRegistryId { get; set; }
        /// <summary>
        /// Angir om besøksadressen er hemmelig
        /// </summary>
        public bool IsSecretVisitingAddress { get; set; }
        /// <summary>
        /// Id til personen som personen bor hos
        /// </summary>
        public int? LivingWithId { get; set; }
        /// <summary>
        /// Id til personens språk
        /// </summary>
        public int? LanguageRegistryId { get; set; }
        /// <summary>
        /// Kjønn
        /// </summary>
        public int? Gender { get; set; }
        /// <summary>
        /// Inneholder besøksaddresse, hvis personen har hemmelig besøksadresse
        /// </summary>
        public string SecretVisitingAddress { get; set; }
        /// <summary>
        /// Inneholder id til postnummer for besøksadresse, hvis personen har hemmelig besøksadresse
        /// </summary>
        public int? SecretVisitingAddressPostCodeRegistryId { get; set; }
        /// <summary>
        /// Inneholder addresse, hvis personen har hemmelig besøksadresse
        /// </summary>
        public string SecretAddress { get; set; }
        /// <summary>
        /// Inneholder id til postnummer for adresse, hvis personen har hemmelig adresse
        /// </summary>
        public int? SecretPostCodeRegistryId { get; set; }
        /// <summary>
        /// Angir om personen ikke ønsker sms varslinger
        /// </summary>
        public bool IsAgainstSmsNotification { get; set; }

        public virtual EnumGender GenderNavigation { get; set; }
    }
}
