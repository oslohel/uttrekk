using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for saker, kan være personsaker(knyttet til klient) og systemsaker(knyttet til organisasjon)
    /// </summary>
    public partial class CaseCase
    {
        /// <summary>
        /// Primærid
        /// </summary>
        public int CaseCaseId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Sakstittel
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Saksnummer på format År-Nummer
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Data fra evt migrering fra annet fagsystem
        /// </summary>
        public string HistoricMigrationDataJson { get; set; }
        /// <summary>
        /// Klientid
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// Organisasjonsid
        /// </summary>
        public int? OrganizationId { get; set; }
        /// <summary>
        /// Sakstype
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Opprettetdato
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Angir hovedsaksbehandler(ansatt)
        /// </summary>
        public int? OwnedBy { get; set; }
        /// <summary>
        /// Angir hvilket team som &amp;apos;eier&amp;apos; saken. Bare informasjon, og ikke noe med autorisasjon å gjøre.
        /// </summary>
        public int? OwnedByTeamId { get; set; }
        /// <summary>
        /// Dato som angir når tid en hendelse var sist endret på i denne saken
        /// </summary>
        public DateTime? EventElementsLastChanged { get; set; }
        /// <summary>
        /// Henvisning er fra klienten
        /// </summary>
        public bool? ReferralClient { get; set; }
        /// <summary>
        /// Id for hovedkategori på saken
        /// </summary>
        public int? MainCategoryRegistryId { get; set; }
        /// <summary>
        /// Id for type oppdrag (for systemsaker)
        /// </summary>
        public int? AssignmentRegistryId { get; set; }
        /// <summary>
        /// Id til kontaktperson (ansatt)
        /// </summary>
        public int? ContactPersonId { get; set; }
        /// <summary>
        /// Dato for når Kontaktperson (ansatt) satt
        /// </summary>
        public DateTime? PrereferralCallDate { get; set; }
        /// <summary>
        /// Mottattdato
        /// </summary>
        public DateTime? ReceivedDate { get; set; }
        /// <summary>
        /// Id for oppdragsgiver (for systemsaker)
        /// </summary>
        public int? AssignmentOrdererId { get; set; }
        /// <summary>
        /// Grunn for å å lukke saken
        /// </summary>
        public int? ClosingReasonRegistryId { get; set; }
        /// <summary>
        /// Angir om standard frist bruker skal benyttes
        /// </summary>
        public bool UseDefaultDeadline { get; set; }
        /// <summary>
        /// Id for konklusjonen på oppdrag, referanse til grunnregister (for systemsaker)
        /// </summary>
        public int? ClosingReasonAssignmentRegistryId { get; set; }
        /// <summary>
        /// Id til sakstype for personsaker
        /// </summary>
        public int? PersoncaseTypeRegistryId { get; set; }
        /// <summary>
        /// Id til sakstype for systemsaker
        /// </summary>
        public int? SystemcaseTypeRegistryId { get; set; }
        /// <summary>
        /// Id til arkivjobb
        /// </summary>
        public int? ArchiveJobQueueId { get; set; }
        /// <summary>
        /// Oppdragsbeskrivelse
        /// </summary>
        public string AssignmentDescription { get; set; }
        /// <summary>
        /// Id til korrespondanse
        /// </summary>
        public int? CorrespondenceId { get; set; }
        /// <summary>
        /// Om saken er offentlig (true) eller unntatt offentligheten. (for systemsaker)
        /// </summary>
        public bool Public { get; set; }
        /// <summary>
        /// Saksstatus
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Tidsstempel for når saksstatus ble satt
        /// </summary>
        public DateTime StatusTimeStamp { get; set; }
        /// <summary>
        /// Dato for når saken fikk status Ny
        /// </summary>
        public DateTime? StatusNewDate { get; set; }
        /// <summary>
        /// Dato for når saken fikk status Under behandling
        /// </summary>
        public DateTime? StatusInProgressDate { get; set; }
        /// <summary>
        /// Dato for når saken fikk status Lukket
        /// </summary>
        public DateTime? StatusClosedDate { get; set; }
        /// <summary>
        /// Id for tilhørighet (for systemsaker)
        /// </summary>
        public int? AffiliationId { get; set; }
        /// <summary>
        /// Saksbeskrivelse
        /// </summary>
        public string Description { get; set; }

        public virtual EnumCasestatus StatusNavigation { get; set; }
        public virtual EnumCasetype TypeNavigation { get; set; }
    }
}
