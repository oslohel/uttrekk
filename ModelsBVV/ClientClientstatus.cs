using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for klientstatuser over tid for en klient
    /// </summary>
    public partial class ClientClientstatus
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int ClientClientStatusId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Id til klient
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Klientstatus
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Dato for klientstatus
        /// </summary>
        public DateTime StatusDate { get; set; }
        /// <summary>
        /// Sluttstatus for klienten
        /// </summary>
        public int? EndStatus { get; set; }
        /// <summary>
        /// Dato for sluttstatus
        /// </summary>
        public DateTime? EndStatusDate { get; set; }

        public virtual EnumClientstatus EndStatusNavigation { get; set; }
        public virtual EnumClientstatus StatusNavigation { get; set; }
    }
}
