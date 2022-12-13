namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell over Foreldre/Personer som har sendt henvisningen
    /// </summary>
    public partial class ReferralReferralparent
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id til Henvisning
        /// </summary>
        public int ReferralId { get; set; }
        /// <summary>
        /// Id til Person
        /// </summary>
        public int ParentId { get; set; }
    }
}
