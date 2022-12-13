namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for kartleggingsrapporter, dette er rapporter som hentes fra Checkware (integrasjon mot eksternt system)
    /// </summary>
    public partial class Assessmentreport
    {
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Navn på rapport
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id til respondent
        /// </summary>
        public int RespondentId { get; set; }
        /// <summary>
        /// Id til kartlegging
        /// </summary>
        public int AssessmentId { get; set; }
        /// <summary>
        /// Id til evaluering
        /// </summary>
        public int EvaluationId { get; set; }
        /// <summary>
        /// Opprinnelig fil-identifikator til dokumentet (ikke i bruk)
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// Opprinnelig passord til dokumentet (ikke i bruk)
        /// </summary>
        public string FilePassword { get; set; }
        /// <summary>
        /// Fildata for PDF dokument til kartleggingsrapporten
        /// </summary>
        public byte[] FileDataBlob { get; set; }
        /// <summary>
        /// Ekstern id til rapport (benyttes av integrasjon med Checkware, dette er en id fra dem som vi må ta vare på)
        /// </summary>
        public int ExternalReportId { get; set; }
    }
}
