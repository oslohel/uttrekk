using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for kartlegging. Disse overføres til Checkware som i sin tur sender ut skjema til mottakere som så lastes ned igjen
    /// </summary>
    public partial class Assessment
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
        /// Id til (person-)saken
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Opprettet dato
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Id til ansatt som eier kartlegging
        /// </summary>
        public int OwnedBy { get; set; }
        /// <summary>
        /// Avsluttet dato
        /// </summary>
        public DateTime? FinishedDate { get; set; }
        /// <summary>
        /// Tittel
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Type kartlegging. Ref AssessmentSettings.CheckWareTreatmentTypesJson
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Id fra tilsvarende behandling som er blitt opprettet i Checkware
        /// </summary>
        public int? TreatmentId { get; set; }
        /// <summary>
        /// Id&amp;apos;er til medsaksbehandlere(ansatt) på kartlegging
        /// </summary>
        public string CoCaseWorkerIdsJson { get; set; }
    }
}
