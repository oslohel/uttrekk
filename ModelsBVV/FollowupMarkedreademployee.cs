namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Ansatte som har lest en followup
    /// </summary>
    public partial class FollowupMarkedreademployee
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id to followup
        /// </summary>
        public int FollowUpId { get; set; }
        /// <summary>
        /// Id til ansatt
        /// </summary>
        public int EmployeeId { get; set; }
    }
}
