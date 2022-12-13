namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for blålys-tilgang
    /// </summary>
    public partial class AuthorizationBluelight
    {
        /// <summary>
        /// Id til Ansatt som har fått aktivert blålys-tilgang til angitt klient i ClientId
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int AuthorizationBlueLightId { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Id til grunn for å aktivere blålys-tilgang
        /// </summary>
        public string ReasonForBlueLight { get; set; }
    }
}
