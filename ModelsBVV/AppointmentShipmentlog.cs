namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for forsendelser gjort fra en avtale
    /// </summary>
    public partial class AppointmentShipmentlog
    {
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id for avtalen
        /// </summary>
        public int AppointmentId { get; set; }
        /// <summary>
        /// Id for korrespondanse
        /// </summary>
        public int CorrespondenceId { get; set; }
        /// <summary>
        /// Id for mal benyttet i korrespondanse
        /// </summary>
        public int? TemplateId { get; set; }
        /// <summary>
        /// Id til kategori for korrespondanse
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
