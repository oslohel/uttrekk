namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell over en persons nasjonaliteter
    /// </summary>
    public partial class PersonPersoncitizenshipnationality
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id til person
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Id til nasjonalitet
        /// </summary>
        public int CitizenshipNationalityRegistryId { get; set; }
    }
}
