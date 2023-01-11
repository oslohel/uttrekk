using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for klienter, en klient må alltid være registrert som en person også, derfor referanse til persontabell
    /// </summary>
    public partial class ClientClient
    {
        /// <summary>
        /// Primærid
        /// </summary>
        public int ClientClientId { get; set; }
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
        /// Referanse til person i persontabell
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Id for tilhørighet
        /// </summary>
        public int AffiliationId { get; set; }
        /// <summary>
        /// Id til organisasjon som klienten er tilknyttet, kan f.eks. være id til en skole
        /// </summary>
        public int? RelatedOrganizationId { get; set; }
        /// <summary>
        /// Navn på organisasjon som klienten er tilknyttet
        /// </summary>
        public string RelatedOrganizationDescription { get; set; }
        /// <summary>
        /// Intern unik id for klienten. Telles fra 1 og oppover per kunde
        /// </summary>
        public int InternalId { get; set; }
        /// <summary>
        /// Id til klientens sivilstatus
        /// </summary>
        public int? CivilStatusRegistryId { get; set; }
        /// <summary>
        /// Id til klientens status
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// Id til arkiv kø (for &amp;apos;dossiermappe&amp;apos;), benyttes ved arkivering
        /// </summary>
        public int? ArchiveJobQueueId { get; set; }
        /// <summary>
        /// Id til arkiv kø (&amp;apos;generell saksmappe&amp;apos;), benyttes ved arkivering
        /// </summary>
        public int? ClientCaseFileArchiveJobQueueId { get; set; }
        /// <summary>
        /// Felt for fritekst kommentar på klient
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Felt for å angi evt. allergier klienten har
        /// </summary>
        public string Allergies { get; set; }
        /// <summary>
        /// Angir om klienten har samtykket til at saksbehandler også kan se tidligere lukkede saker
        /// </summary>
        public bool? HasClosedCasesConsent { get; set; }
        /// <summary>
        /// Angir datoen for når samtykke er gitt/fjernet
        /// </summary>
        public DateTime? ConsentChangeDate { get; set; }
    }
}
