namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell over dokument på journal
    /// </summary>
    public partial class JournalDocument
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int JournalDocumentId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til journal
        /// </summary>
        public int JournalId { get; set; }
        /// <summary>
        /// Dokument tekst, basis for pdf generering
        /// </summary>
        public string DocumentText { get; set; }
        public string Password { get; set; }
        public string FileId { get; set; }
        /// <summary>
        /// Id til mal
        /// </summary>
        public bool IsCk5Template { get; set; }
        /// <summary>
        /// Fildata når konvertert til pdf
        /// </summary>
        //public byte[] FileDataBlob { get; set; }
        /// <summary>
        /// Liste over kommentarer til dokumenttekst
        /// </summary>
        public string DocumentCommentsJson { get; set; }
    }
}
