using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Ansattes stillinger, stillingsprosent etc.
    /// </summary>
    public partial class EmployeeEmployeeposition
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int EmployeeEmployeePositionId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Aktiv fra
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Aktiv til
        /// </summary>
        public DateTime? StopDate { get; set; }
        /// <summary>
        /// Deaktivert av
        /// </summary>
        public int? StoppedBy { get; set; }
        /// <summary>
        /// Id til ansatt
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Stillingens tittel
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Ansattnummer
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// Stillingsprosent
        /// </summary>
        public decimal EmploymentPercentage { get; set; }
        /// <summary>
        /// Id til organisasjon stillingen er knytt til
        /// </summary>
        public int OrganizationId { get; set; }
    }
}
