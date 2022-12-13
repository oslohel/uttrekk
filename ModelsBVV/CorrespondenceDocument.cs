namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for dokument knytt til en korrespondanse (hoveddokument for utgående korrespondanse, og hoveddokument og evt. vedlegg på inngående korrespondanser)
    /// </summary>
    public partial class CorrespondenceDocument
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int CorrespondenceDocumentId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til korrespondanse
        /// </summary>
        public int CorrespondenceId { get; set; }
        /// <summary>
        /// Tekstfelt for korrespondanse, benyttes under skriving av korrespondanse før den er ferdigstilt og pdf er generert
        /// </summary>
        public string DocumentText { get; set; }
        /// <summary>
        /// Angir om det er benyttet mal for versjon 5 av CK editor
        /// </summary>
        public bool IsCk5Template { get; set; }
        /// <summary>
        /// Id til fil
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// Navn på fil
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Passord til filen
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Fildata
        /// </summary>
        public byte[] FileDataBlob { get; set; }
        /// <summary>
        /// Liste over kommentarer på en korrespondanse, benyttes av saksbehandlere for å samarbeide ved utarbeiding av en korrespondanse.
        /// </summary>
        public string DocumentCommentsJson { get; set; }
    }
}
