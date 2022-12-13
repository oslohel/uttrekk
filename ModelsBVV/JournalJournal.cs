using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell over journaler, knyttet til enten en klient, personsak eller systemsak.
    /// </summary>
    public partial class JournalJournal
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int JournalJournalId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til saken
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// Opprettetdato
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Hovedsaksbehandler(ansatt) på journalen
        /// </summary>
        public int OwnedBy { get; set; }
        /// <summary>
        /// Ferdig dato
        /// </summary>
        public DateTime? FinishedDate { get; set; }
        /// <summary>
        /// Tittel
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Registreringsnummer
        /// </summary>
        public int RegistrationNumber { get; set; }
        /// <summary>
        /// Ved evt. tidligere migrering vises tidligere verdier i tidligere system her
        /// </summary>
        public string HistoricMigrationDataJson { get; set; }
        /// <summary>
        /// Journaldato
        /// </summary>
        public DateTime JournalDate { get; set; }
        /// <summary>
        /// Id til journalkategori
        /// </summary>
        public int JournalCategoryRegistryId { get; set; }
        /// <summary>
        /// Id til dokumentmal
        /// </summary>
        public int? BasedOnDocumentTemplateId { get; set; }
        /// <summary>
        /// Id til arkivjobb i arkiv køen
        /// </summary>
        public int? ArchiveJobQueueId { get; set; }
        /// <summary>
        /// Id over klienter(søsken) som journalen er kopiert til
        /// </summary>
        public string CopyToClientsJson { get; set; }
        /// <summary>
        /// Angir om denne journalen er opprettet av en ansatt som er logget med en helserolle
        /// </summary>
        public bool IsEpj { get; set; }
        /// <summary>
        /// Angir om dette er en intern eller unndratt korrespondanse
        /// </summary>
        public bool InternalOrWithheld { get; set; }
        /// <summary>
        /// Angir hvilken type inndratt eller unndragelse som gjelder
        /// </summary>
        public int InternalOrWithheldType { get; set; }
        /// <summary>
        /// Angir om denne journalen inneholder helserelaterteopplysninger
        /// </summary>
        public bool IsHealthRelated { get; set; }

        public virtual EnumInternalorwithheldtype InternalOrWithheldTypeNavigation { get; set; }
    }
}
