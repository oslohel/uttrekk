using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for nettverkspersoner til en klient, personer som ikke er del av &amp;apos;familien&amp;apos; til klienten
    /// </summary>
    public partial class PersonNetworkpersonrole
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int PersonNetworkPersonRoleId { get; set; }
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
        /// Id til klient
        /// </summary>
        public int RelatedClientId { get; set; }
        /// <summary>
        /// Id til person som er &amp;apos;nettverksperson&amp;apos; til klienten
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Id til sak dersom nettverksperson er knytt til person vedrørende en bestemt sak
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Id til rollen for nettverksperson (lærer, lege, psykolog etc.)
        /// </summary>
        public int RoleRegistryId { get; set; }
        /// <summary>
        /// Id til organisasjon dersom nettverksperson er tilknyttet en bestemt organisasjon (skole, legesenter, kommune, etc.
        /// </summary>
        public int? OrganizationId { get; set; }
        /// <summary>
        /// Id til organisasjonsperson, når en person er knytt til en organisasjon og inneholder info relatert til denne personens\u0020\u0020som en ansatt i denne organisasjonen, type telefonnr, epost, adresse etc.
        /// </summary>
        public int? OrganizationPersonId { get; set; }
        /// <summary>
        /// Angir om det er en nær relasjon
        /// </summary>
        public bool IsClosestRelation { get; set; }
        /// <summary>
        /// Om vedkommende skal få kopi av av post
        /// </summary>
        public bool IsCopyOfPost { get; set; }
        public string Notes { get; set; }
    }
}
