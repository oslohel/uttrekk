using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell over henvisninger (kun brukt av Visma Flyt PPT)
    /// </summary>
    public partial class ReferralReferral
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int ReferralReferralId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til sak
        /// </summary>
        public int CaseId { get; set; }
        /// <summary>
        /// Id til klienten henvisningen er knytt til
        /// </summary>
        public int? ReferrerClientId { get; set; }
        /// <summary>
        /// Id til organisasjon når henvisningen er knytt til en organisasjon
        /// </summary>
        public int? ReferrerOrganizationId { get; set; }
        /// <summary>
        /// Id til tilhørende inngående korrespondanse
        /// </summary>
        public int? CorrespondenceId { get; set; }
        /// <summary>
        /// Id til begrunnelse for henvisning
        /// </summary>
        public int? ReasonRegistryId { get; set; }
        /// <summary>
        /// Beskrivelse
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Id til konklusjon av inntaksmøte
        /// </summary>
        public int? IntakeMeetingConclusionRegistryId { get; set; }
        /// <summary>
        /// Dato for inntaksmøte
        /// </summary>
        public DateTime? IntakeMeetingDate { get; set; }
        /// <summary>
        /// Id til organisasjon som henvisningen evt. er videresendt til
        /// </summary>
        public int? ForwardedToOrganizationTypeRegistryId { get; set; }
        /// <summary>
        /// Mottattdato
        /// </summary>
        public DateTime? ReceivedDate { get; set; }
    }
}
