namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#LOW] Tabell for kartleggingsinnstillinger
    /// </summary>
    public partial class Assessmentsetting
    {
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Felt for å slå på/av kartlegging, kunder som ikke bruker kartlegging kan ha det avslått
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Url prefiks, benyttes av integrasjonen mot Checkware. Man har en fast url + en prefiks til denne.
        /// </summary>
        public string Urlprefix { get; set; }
        /// <summary>
        /// Brukernavn til integrasjon mot Checkware
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Passord(hashed) til integrasjon mot Checkware
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Behandlingstyper hentet fra Checkware, lagret lokalt for å øke responstid/brukervennlighet
        /// </summary>
        public string CheckWareTreatmentTypesJson { get; set; }
        /// <summary>
        /// Planer hentet fra Checkware, lagret lokalt for å øke responstid/brukervennlighet
        /// </summary>
        public string CheckWarePlansJson { get; set; }
        /// <summary>
        /// Liste over individuelle kartlegginger fra checkware som kan benyttes i kartlegging
        /// </summary>
        public string CheckWareIndividualAssessmentsJson { get; set; }
    }
}
