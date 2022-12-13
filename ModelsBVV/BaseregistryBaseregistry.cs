using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell som inneholder mange grunnregister. Kolonner i andre tabeller som ender på RegistryId henviser til denne tabell sin primarykey
    /// </summary>
    public partial class BaseregistryBaseregistry
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int BaseRegistryBaseRegistryId { get; set; }
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
        /// Registerkode, angir hvilken type grunnregister dette er (tre-bokstav forkortelser).
        /// </summary>
        public string RegistryCode { get; set; }
        /// <summary>
        /// Verdien en grunnregister kode har. feks en beskrivelse &amp;apos;Volda&amp;apos; for poststed-grunnregister
        /// </summary>
        public string RegistryValue { get; set; }
        /// <summary>
        /// Grunnregister kode. feks postnr 6100 for et poststed
        /// </summary>
        public string RegistryValueCode { get; set; }
        /// <summary>
        /// Tilleggskode for grunnregister. feks om en kategori for korrespondanse er utgående eller inngående
        /// </summary>
        public string RegistrySubCode { get; set; }
    }
}
