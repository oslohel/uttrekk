using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#LOW] Tabell for å lagre tid per faktisk instans av et element (ref TimeRegistration).
    /// </summary>
    public partial class TimeregistrationEventelement
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int TimeRegistrationEventElementId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til hendelse, kan være korrespondanse, journal etc.
        /// </summary>
        public int EventElementId { get; set; }
        /// <summary>
        /// Type hendelse, kan være korrespondanse, journal etc.
        /// </summary>
        public int EventElementType { get; set; }
        /// <summary>
        /// Id til Kategori
        /// </summary>
        public int? CategoryRegistryId { get; set; }
        /// <summary>
        /// Id til sak
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// Id til organisasjon
        /// </summary>
        public int? OrganizationId { get; set; }
        /// <summary>
        /// Ferdigstilt dato
        /// </summary>
        public DateTime FinishedDate { get; set; }
        /// <summary>
        /// Tid angitt i minutt (hentes fra TimeRegistration.Minutes ved lagring)
        /// </summary>
        public int Minutes { get; set; }

        public virtual EnumEventelementtype EventElementTypeNavigation { get; set; }
    }
}
