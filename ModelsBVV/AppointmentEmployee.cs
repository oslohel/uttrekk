using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell som inneholder ansatte, synkronisert fra hovedregister for ansatte.
    /// </summary>
    public partial class AppointmentEmployee
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
        /// Ansatt aktivert fra
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Ansatt aktivert til
        /// </summary>
        public DateTime? StopDate { get; set; }
        /// <summary>
        /// Id til ansatt
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Epost til ansatt
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Navn til ansatt
        /// </summary>
        public string Name { get; set; }
    }
}
