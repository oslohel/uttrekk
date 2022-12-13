namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Liste over klientgrupper som klienten tilhører
    /// </summary>
    public partial class ClientClientclientgroup
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int ClientClientClientGroupId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til klient (klientid)
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Id til klientgruppen
        /// </summary>
        public int ClientGroupId { get; set; }
    }
}
