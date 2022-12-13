using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for korrespondanser (inngående og utgående post)
    /// </summary>
    public partial class CorrespondenceCorrespondence
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int CorrespondenceCorrespondenceId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til saken, dersom registrert på en sak (personsak eller systemsak)
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Id til klienten (er ikke utfylt om det gjelder systemsak)
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// Opprettet dato
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Id til hovedsaksbehandler (ansatt)
        /// </summary>
        public int OwnedBy { get; set; }
        /// <summary>
        /// Dato for ferdigstilling av korrespondanse
        /// </summary>
        public DateTime? FinishedDate { get; set; }
        /// <summary>
        /// Felt for tittel
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Dato for korrespondanse(sendt/mottatt)
        /// </summary>
        public DateTime CorrespondenceDate { get; set; }
        /// <summary>
        /// Registreringsnummer (sett i forbindelse med kolonne Year)
        /// </summary>
        public int RegistrationNumber { get; set; }
        /// <summary>
        /// Ved evt. tidligere migrering vises tidligere verdier i tidligere system her
        /// </summary>
        public string HistoricMigrationDataJson { get; set; }
        /// <summary>
        /// Registreringsår (sett i forbindelse med kolonne RegistrationNumber)
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Retning, feks. inngående/utgående
        /// </summary>
        public int Direction { get; set; }
        /// <summary>
        /// Id til malen som brevet er basert på
        /// </summary>
        public int? BasedOnDocumentTemplateId { get; set; }
        /// <summary>
        /// Id til kategori for korrespondansen
        /// </summary>
        public int CorrespondenceCategoryRegistryId { get; set; }
        /// <summary>
        /// Dato da korrespondansen er sendt
        /// </summary>
        public DateTime? SentDate { get; set; }
        /// <summary>
        /// Dato da korrespondansen er mottatt
        /// </summary>
        public DateTime? ReceivedDate { get; set; }
        /// <summary>
        /// Angir om korrespondansen skal vises i toppen av listen over hendelser på en klient.
        /// </summary>
        public int AlwaysOnTopOfList { get; set; }
        /// <summary>
        /// Liste over avsendere/mottakere av korrespondanse. Oppført som json-objekt.
        /// </summary>
        public string SendersRecipients { get; set; }
        /// <summary>
        /// Antall vedlegg på korrespondansen
        /// </summary>
        public int? NumberOfAttachments { get; set; }
        /// <summary>
        /// Id til arkivkø, for bruk ved arkivering
        /// </summary>
        public int? ArchiveJobQueueId { get; set; }
        /// <summary>
        /// Dato da korrespondasen er sendt til godkjenning
        /// </summary>
        public DateTime? SentForApprovalDate { get; set; }
        /// <summary>
        /// Ansatt som sendte korrespondanse til godkjenning
        /// </summary>
        public int? SentForApprovalBy { get; set; }
        /// <summary>
        /// Liste over id til klienter som korrespondansen skal kopieres til
        /// </summary>
        public string CopyToClientsJson { get; set; }
        /// <summary>
        /// Angir om denne korrespondansen er opprettet av en saksbehandler som er logget med en helserolle (&amp;apos;elektronisk pasient journal&amp;apos;)
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

        public virtual EnumCorrespondencedirection DirectionNavigation { get; set; }
        public virtual EnumInternalorwithheldtype InternalOrWithheldTypeNavigation { get; set; }
    }
}
