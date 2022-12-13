namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for å tildele ekstraordinær tilgang til en klient, eller fjerne tilgang til en klient for en saksbehandler.
    /// </summary>
    public partial class ClientClientemployeedeviation
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int ClientClientEmployeeDeviationId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Id til ansatt
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Kommentar til endringen i tilgang som er lagt inn. F.eks. at tilgang er fjernet fordi saksbehandler er i familie med klient.
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Typen tilgang som er registrert(om man har fått tilgang, eller fjernet tilgang)
        /// </summary>
        public int Access { get; set; }
        public string AffiliationIdsJson { get; set; }

        public virtual EnumClientemployeedeviationaccesspoint AccessNavigation { get; set; }
    }
}
