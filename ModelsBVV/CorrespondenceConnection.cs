namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for å håndtere innbyggerdialog via KS (knytter inngående innbyggerdialog-meldinger til en orginal/utgående innbyggerdialog i form av en korrespondanse
    /// </summary>
    public partial class CorrespondenceConnection
    {
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int CorrespondenceConnectionId { get; set; }
        /// <summary>
        /// Id til korrespondanse
        /// </summary>
        public int? CorrespondenceParentId { get; set; }
        /// <summary>
        /// Id til korrespondanse
        /// </summary>
        public int CorrespondenceId { get; set; }
        /// <summary>
        /// Shipmentid fra KS for den inngående forsendelsen vi har hentet ned
        /// </summary>
        public string ShipmentReferenceId { get; set; }
    }
}
