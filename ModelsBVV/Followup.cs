using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for lagring av oppfølging (interne huskelapper og system-genererte oppfølginger av frister)
    /// </summary>
    public partial class Followup
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
        /// Tittel
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Beskrivelse
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Dato for frist
        /// </summary>
        public DateTime? Deadline { get; set; }
        /// <summary>
        /// Angir om oppfølging er tilknyttet klient, sak, hendelse, eller ingenting
        /// </summary>
        public int ConnectionType { get; set; }
        /// <summary>
        /// Id til person
        /// </summary>
        public int? PersonId { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// Klientens navn
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Id til organisasjon
        /// </summary>
        public int? OrganizationId { get; set; }
        /// <summary>
        /// Navn på organisasjon
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Id til sak
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Saksnummer
        /// </summary>
        public string CaseNumber { get; set; }
        /// <summary>
        /// Saksstatus
        /// </summary>
        public int? CaseStatus { get; set; }
        /// <summary>
        /// Type hendelse (korrespondanse, journal, avtale etc.
        /// </summary>
        public int? EventElementType { get; set; }
        /// <summary>
        /// Id til hendelse
        /// </summary>
        public int? EventElementId { get; set; }
        /// <summary>
        /// Opprettet dato
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Opprettet av (navn på ansatt)
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Hovedsaksbehandler(ansatt)
        /// </summary>
        public int? OwnerId { get; set; }
        /// <summary>
        /// Navn på hovedsaksbehandler
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// Dato hvis og når hovedsaksbehandler er byttet til en annen ansatt
        /// </summary>
        public DateTime? OwnerChangeDate { get; set; }
        /// <summary>
        /// Status på oppfølgingen
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Dato for når status på oppfølgingen er endret
        /// </summary>
        public DateTime StatusChangeDate { get; set; }
        /// <summary>
        /// Angir om denne oppfølgingen ble generert aut. av systemet.
        /// </summary>
        public int? SystemGeneratedType { get; set; }
        /// <summary>
        /// Id til SMS som evt har opprette oppfølgingen (skjer feks om en sms feiler slik at noen kan følge opp feilen)
        /// </summary>
        public int? SmsId { get; set; }

        public virtual EnumEventelementtype EventElementTypeNavigation { get; set; }
    }
}
