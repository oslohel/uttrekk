using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#LOW] Tabell for å angi gjennomsnittlig tid per type kategori innenfor forskjellige type hendelser (feks 2t for kategori &amp;apos;Oppsummering hjemmebesøk&amp;apos; for Journal)
    /// </summary>
    public partial class TimeregistrationTimeregistration
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int TimeRegistrationTimeRegistrationId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Type hendelse, kan være korrespondanse, journal etc.
        /// </summary>
        public int EventElementType { get; set; }
        /// <summary>
        /// Id til aktuell kategori
        /// </summary>
        public int? CategoryRegistryId { get; set; }
        /// <summary>
        /// Dato når det gjelder fra
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Tid angitt i minutt
        /// </summary>
        public int Minutes { get; set; }

        public virtual EnumEventelementtype EventElementTypeNavigation { get; set; }
    }
}
