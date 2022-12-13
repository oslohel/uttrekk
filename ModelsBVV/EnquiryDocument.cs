namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Henvendelsedokument
    /// </summary>
    public partial class EnquiryDocument
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EnquiryDocumentId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// FK
        /// </summary>
        public int EnquiryId { get; set; }
        /// <summary>
        /// HTML versjon av dokument
        /// </summary>
        public string DocumentText { get; set; }
        public byte[] FileDataBlob { get; set; }
    }
}
