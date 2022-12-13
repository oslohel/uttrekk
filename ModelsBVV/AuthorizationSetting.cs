namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#LOW] Tabell for autorisasjonstilgang per kunde
    /// </summary>
    public partial class AuthorizationSetting
    {
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int AuthorizationSettingsId { get; set; }
        /// <summary>
        /// Om det er lov å gi tiltang til flere enheter enn en (fra organisasjonsstrukturen) ved innlogging
        /// </summary>
        public bool IsMultipleOrgUnitsAllowed { get; set; }
    }
}
