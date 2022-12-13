using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Henvendelse
    /// </summary>
    public partial class EnquiryEnquiry
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EnquiryEnquiryId { get; set; }
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
        /// Ferdig dato
        /// </summary>
        public DateTime? FinishedDate { get; set; }
        /// <summary>
        /// Id til arkivjobb i arkiv køen
        /// </summary>
        public int? ArchiveJobQueueId { get; set; }
        /// <summary>
        /// Tittel
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Kallenavn på klienten
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// Kontaktinformasjon
        /// </summary>
        public string ContactInfo { get; set; }
        /// <summary>
        /// Markering om viktig
        /// </summary>
        public bool IsImportant { get; set; }
        /// <summary>
        /// Rapportertdato
        /// </summary>
        public DateTime ReportedDate { get; set; }
        /// <summary>
        /// Id til Organisasjon
        /// </summary>
        public int? OrganizationId { get; set; }
        /// <summary>
        /// Id for tilhørighet
        /// </summary>
        public int? AffiliationId { get; set; }
        /// <summary>
        /// Type melder
        /// </summary>
        public int? ReporterTypeRegistryId { get; set; }
        /// <summary>
        /// Hovedkategori
        /// </summary>
        public int? MainCategoryRegistryId { get; set; }
        /// <summary>
        /// Tilleggskategori
        /// </summary>
        public string AdditionalCategoryRegistryIdsJson { get; set; }
        /// <summary>
        /// Tilhørende inngående korrespondanse
        /// </summary>
        public int? CorrespondenceId { get; set; }
        /// <summary>
        /// Er initiell hendelse på en sak
        /// </summary>
        public bool? IsInitialEnquiry { get; set; }
        /// <summary>
        /// Angir om dette er en intern eller unndratt hendendelse
        /// </summary>
        public bool InternalOrWithheld { get; set; }
        /// <summary>
        /// Angir hvilken type inndratt eller unndragelse som gjelder
        /// </summary>
        public int InternalOrWithheldType { get; set; }
    }
}
