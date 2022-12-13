namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for med-saksbehandlere på en sak
    /// </summary>
    public partial class CaseCasecaseworker
    {
        /// <summary>
        /// Primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Saksid, referanse til primærnøkkel til saken
        /// </summary>
        public int CaseId { get; set; }
        /// <summary>
        /// Saksbehandlerid
        /// </summary>
        public int CaseworkerId { get; set; }
    }
}
