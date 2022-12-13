using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for avtaler, innholder avtaler opprettet i løsningen, men kan også være eksterne avtaler som er synkronisert fra en Microsoft Exchange integrasjon, hvis kunden har benyttet denne integrasjonen
    /// </summary>
    public partial class AppointmentAppointment
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Hvilken ansatt som opprettet eller trigget at denne raden ble opprettet
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Tittel på avtale
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Beskrivelse for avtalen
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Avtalens startdato og tidspunkt
        /// </summary>
        public DateTime DateFrom { get; set; }
        /// <summary>
        /// Avtalens sluttdato og tidspunkt
        /// </summary>
        public DateTime DateTo { get; set; }
        /// <summary>
        /// Angir om dette er en avtale som varer hele dagen
        /// </summary>
        public bool? IsAllDay { get; set; }
        /// <summary>
        /// Angir når tid avtalen ble opprettet/lagret første gang
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Angir navnet på den som opprettet avtalen
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Angir hvilken ansatt som har endret på avtalen
        /// </summary>
        public int? ChangedBy { get; set; }
        /// <summary>
        /// Angir navnet på den som har endret på en avtale
        /// </summary>
        public string ChangedByName { get; set; }
        /// <summary>
        /// Angir tilnytning avtale har, den kan være knyttet til en klient/personsak, en organisasjon(systemsak) eller ha ingen tilknytning.
        /// </summary>
        public int ConnectionType { get; set; }
        /// <summary>
        /// Id for avtalekategori, verdien refererer til grunnregister for avtalekategorier
        /// </summary>
        public int? CategoryRegistryId { get; set; }
        /// <summary>
        /// Navn/Beskrivelse av Avtalekategori
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Klientid
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// Personid
        /// </summary>
        public int? PersonId { get; set; }
        /// <summary>
        /// Klientens navn
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Klientens fødselsdato
        /// </summary>
        public DateTime? ClientBirthDate { get; set; }
        /// <summary>
        /// Id til tilknytt Organisasjon
        /// </summary>
        public int? OrganizationId { get; set; }
        /// <summary>
        /// Organisasjonsnavn
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Id til tilknytt sak (personsak eller systemsak)
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Møtested
        /// </summary>
        public string MeetingPlace { get; set; }
        /// <summary>
        /// Adresse
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Id til postnummer
        /// </summary>
        public int? PostCodeRegistryId { get; set; }
        /// <summary>
        /// Poststed
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Postnummer
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// Årsak til kansellering av avtale
        /// </summary>
        public string CancellationReason { get; set; }
        /// <summary>
        /// Id til kanselleringsgrunn
        /// </summary>
        public int? CancellationReasonRegistryId { get; set; }
        /// <summary>
        /// Ansatt som kansellerte avtalen
        /// </summary>
        public int? CancelledBy { get; set; }
        /// <summary>
        /// Navn på ansatt som kansellerte avtalen
        /// </summary>
        public string CancelledByName { get; set; }
        /// <summary>
        /// Kansellert dato
        /// </summary>
        public DateTime? CancelDate { get; set; }
        /// <summary>
        /// Id til korrespondanse kategori
        /// </summary>
        public int? CorrespondenceCategoryRegistryId { get; set; }
        /// <summary>
        /// Korrespondanse kategori
        /// </summary>
        public string CorrespondenceCategoryName { get; set; }
        /// <summary>
        /// Id til mal
        /// </summary>
        public int? TemplateId { get; set; }
        /// <summary>
        /// Angir om det er en ekstern avtale (synkronisert fra f.eks. Microsoft Exchange)
        /// </summary>
        public bool IsExternalAppointment { get; set; }
        /// <summary>
        /// Id til den eksterne avtalen, id komemr fra det eksterne systemet
        /// </summary>
        public string ExternalAppointmentId { get; set; }
        /// <summary>
        /// Angir om man trenger å synkronisere denne avtalen, f.eks. fordi det har skjedd en endring
        /// </summary>
        public bool IsSyncRequired { get; set; }
        /// <summary>
        /// Id for ekstern avtale (ExternalAppointmentId), angitt i bytes
        /// </summary>
        public byte[] ExternalAppointmentIdInBytes { get; set; }
        /// <summary>
        /// Angir epost til den som har organisert avtalen (synkronisert fra f.eks. Microsoft Exchange).
        /// </summary>
        public string Organizer { get; set; }
        /// <summary>
        /// Nøkkel som angir hvilken organisasjon entiteten tilhører, del av multitenancy, brukes for å skille kundedata fra hverandre. Hver kunde har sin egen unike CustomerGuid.
        /// </summary>
        public string CustomerGuid { get; set; }
    }
}
