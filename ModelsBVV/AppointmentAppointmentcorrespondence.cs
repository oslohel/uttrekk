namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell som knytter korrespondanser og avtaler i sammen. En avtale kan ha flere korrespondanser.
    /// </summary>
    public partial class AppointmentAppointmentcorrespondence
    {
        /// <summary>
        /// Unik Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id til avtale
        /// </summary>
        public int AppointmentId { get; set; }
        /// <summary>
        /// Id til korrespondanse
        /// </summary>
        public int CorrespondenceId { get; set; }
    }
}
