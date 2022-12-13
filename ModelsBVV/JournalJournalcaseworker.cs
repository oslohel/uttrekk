namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Liste over medsaksbehandlere på journalen
    /// </summary>
    public partial class JournalJournalcaseworker
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id til journal
        /// </summary>
        public int JournalId { get; set; }
        /// <summary>
        /// Id til ansatt
        /// </summary>
        public int CaseworkerId { get; set; }
    }
}
