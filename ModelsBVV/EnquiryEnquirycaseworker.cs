namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] TODO
    /// </summary>
    public partial class EnquiryEnquirycaseworker
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// FK
        /// </summary>
        public int EnquiryId { get; set; }
        /// <summary>
        /// Saksbehandler
        /// </summary>
        public int CaseworkerId { get; set; }
    }
}
