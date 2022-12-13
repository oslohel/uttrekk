namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Id&amp;apos;er for tilleggskategorier på en sak
    /// </summary>
    public partial class CaseCaseadditionalcategory
    {
        /// <summary>
        /// Primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Referanse til primærnøkkel til saken
        /// </summary>
        public int CaseId { get; set; }
        /// <summary>
        /// Referanse til kategori
        /// </summary>
        public int CategoryRegistryId { get; set; }
    }
}
