namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#SYSTEM] Tabell som inneholder kunde-innstillinger for avtaler
    /// </summary>
    public partial class AppointmentSetting
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
        /// Json som inneholder innstillinger for avtaler.
        /// </summary>
        public string Settings { get; set; }
    }
}
