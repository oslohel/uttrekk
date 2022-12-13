namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Medsaksbehandlere (ansatt) på en korrespondanse
    /// </summary>
    public partial class CorrespondenceCorrespondencecaseworker
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id til korrespondanse
        /// </summary>
        public int CorrespondenceId { get; set; }
        /// <summary>
        /// Id til saksbehandler(ansatt)
        /// </summary>
        public int CaseworkerId { get; set; }
    }
}
