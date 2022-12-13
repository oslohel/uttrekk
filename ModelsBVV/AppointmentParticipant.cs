using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell over personer som er deltakere på en avtale
    /// </summary>
    public partial class AppointmentParticipant
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til avtale
        /// </summary>
        public int AppointmentId { get; set; }
        /// <summary>
        /// Opprettet dato
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Angir om deltaker er ekstern eller intern
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Id til deltaker(person)
        /// </summary>
        public int? ParticipantId { get; set; }
        /// <summary>
        /// Navn på deltaker
        /// </summary>
        public string ParticipantName { get; set; }
        /// <summary>
        /// E-Post til deltaker
        /// </summary>
        public string ParticipantEmail { get; set; }
        /// <summary>
        /// Fødselsdato til deltaker
        /// </summary>
        public DateTime? ParticipantBirthDate { get; set; }
        /// <summary>
        /// Angir om deltaker får en korrespondanse(invitasjon)
        /// </summary>
        public bool IsGettingCorrespondence { get; set; }
        /// <summary>
        /// Angir om deltaker får sms varsel
        /// </summary>
        public bool IsGettingSms { get; set; }
        /// <summary>
        /// Angir om deltaker ikke ønsker sms varsel
        /// </summary>
        public bool? IsAgainstSmsNotification { get; set; }
        /// <summary>
        /// Angir om dette er hoveddeltaker/arrangør av avtalen
        /// </summary>
        public bool IsMainParticipant { get; set; }
        /// <summary>
        /// Type tilknytning (til klient, organisasjon eller manuelt registrert)
        /// </summary>
        public int? Kind { get; set; }
        /// <summary>
        /// Fødselsnummer
        /// </summary>
        public string BirthNumber { get; set; }
        /// <summary>
        /// Organisasjonsnummer
        /// </summary>
        public string OrganizationNumber { get; set; }
        /// <summary>
        /// Angir om det er en hemmelig adresse
        /// </summary>
        public bool? IsSecretAddress { get; set; }
        /// <summary>
        /// Adresse
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Postnummer
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// Poststed
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Land
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Prefiks/landskode til telefonnummer
        /// </summary>
        public string PhonePrefix { get; set; }
        /// <summary>
        /// Navn til person
        /// </summary>
        public string OrganizationPersonName { get; set; }
        public bool? IsHighlyRestricted { get; set; }

        public virtual EnumSenderrecipientkind KindNavigation { get; set; }
        public virtual EnumAppointmentparticipanttype TypeNavigation { get; set; }
    }
}
