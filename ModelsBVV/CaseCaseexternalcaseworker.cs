namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for eksterne-saksbehandlere (Samarbeidspartnere) på en sak, dette er da personer som ikke er registrert som saksbehandler (ansatt) i systemet, men registrert som en person/forbindelse i systemet (for systemsak)
    /// </summary>
    public partial class CaseCaseexternalcaseworker
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
        /// Person id, referanse til persontabellen
        /// </summary>
        public int PersonId { get; set; }
    }
}
