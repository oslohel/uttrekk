using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for opprettede kartlegginger (kartleggingsplan/skjema) per respondent, som skal til Checkware. 
    /// </summary>
    public partial class Assessmentevaluation
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
        /// Id til respondent på kartlegging
        /// </summary>
        public int RespondentId { get; set; }
        /// <summary>
        /// Id til kartlegging
        /// </summary>
        public int AssessmentId { get; set; }
        /// <summary>
        /// Status på sendingen til checkware.
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Type evaluering
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Ekstern id til evaluering (benyttes av integrasjon med Checkware, dette er en id fra dem)
        /// </summary>
        public string ExternalEvaluationId { get; set; }
        /// <summary>
        /// Id til mal for evaluering
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// Startdato for evaluering
        /// </summary>
        public DateTime? StartDate { get; set; }

        public virtual EnumAssessmentevaluationstatus StatusNavigation { get; set; }
        public virtual EnumAssessmentevaluationtype TypeNavigation { get; set; }
    }
}
