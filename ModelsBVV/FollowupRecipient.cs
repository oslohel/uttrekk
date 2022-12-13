namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for mottakere av en opprettet oppfølging
    /// </summary>
    public partial class FollowupRecipient
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til Oppfølging
        /// </summary>
        public int FollowUpId { get; set; }
        /// <summary>
        /// Angir om oppfølgingen tilhører en ansatt eller et team
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Id til mottaker, må ses i sammenheng av kolonnen Type
        /// </summary>
        public int RecipientId { get; set; }
        /// <summary>
        /// Navn på mottaker
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// Initialer på mottaker
        /// </summary>
        public string RecipientInitials { get; set; }

        public virtual EnumFollowuprecipienttype TypeNavigation { get; set; }
    }
}
