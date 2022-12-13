using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for en persons rolle i en familie
    /// </summary>
    public partial class PersonPersonrole
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int PersonPersonRoleId { get; set; }
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
        /// Id til person
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Id til personen som denne personen har en rolle til (PersonId er &amp;apos;onkel&amp;apos; til RelatedPersonId)
        /// </summary>
        public int RelatedPersonId { get; set; }
        /// <summary>
        /// Har personen foreldrerett
        /// </summary>
        public bool? HasParentalRights { get; set; }
        /// <summary>
        /// Er personen nærmeste relasjon
        /// </summary>
        public bool? IsClosestRelation { get; set; }
        /// <summary>
        /// Om vedkommende skal få kopi av av post
        /// </summary>
        public bool? IsCopyOfPost { get; set; }
        /// <summary>
        /// Id til familierollen
        /// </summary>
        public int FamilyroleRegistryId { get; set; }
        public string Notes { get; set; }
    }
}
