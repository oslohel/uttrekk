namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for respondenter på en kartlegging (de som skal besvare spørsmålene i en kartlegging)
    /// </summary>
    public partial class Assessmentrespondent
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
        /// Id til kartlegging
        /// </summary>
        public int AssessmentId { get; set; }
        /// <summary>
        /// Id til respondent
        /// </summary>
        public int RespondentId { get; set; }
        /// <summary>
        /// Navn på respondent
        /// </summary>
        public string RespondentName { get; set; }
        /// <summary>
        /// Fødselsnummer til respondent
        /// </summary>
        public string BirthNumber { get; set; }
        /// <summary>
        /// Epost til respondent
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Type respondent
        /// </summary>
        public int RespondentKind { get; set; }
        /// <summary>
        /// Id til respondent rolle (far, mor, lærer, elev, bror etc.). Ref AssessmentSettings.CheckWareTreatmentTypesJson
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Telefonnummer til respondent
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Prefiks/landskode for telefonnummer til respondent, f.eks. +47
        /// </summary>
        public string PhonePrefix { get; set; }

        public virtual EnumSenderrecipientkind RespondentKindNavigation { get; set; }
    }
}
