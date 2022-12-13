namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#LOW] Vedlegg for knytt til nylig (og til nå uregistrert) innkommende post, f.eks. post fra svarinn kan ha vedlegg
    /// </summary>
    public partial class CorrespondenceIncomingpostattachment
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// FK
        /// </summary>
        public int IncomingPostId { get; set; }
        /// <summary>
        /// Vedlegg id
        /// </summary>
        public string AttachmentId { get; set; }
        /// <summary>
        /// Navn
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Opprinnelig fil-identifikator til dokumentet (ikke i bruk)
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// Opprinnelig passord til dokumentet (ikke i bruk)
        /// </summary>
        public string FilePassword { get; set; }
        /// <summary>
        /// Fildata for PDF dokument. Om denne er null er fil lagret i FileDataBlob
        /// </summary>
        public byte[] FileData { get; set; }
        /// <summary>
        /// Fildata for PDF dokument. Om denne er null er fil lagret i FileData
        /// </summary>
        public byte[] FileDataBlob { get; set; }
    }
}
