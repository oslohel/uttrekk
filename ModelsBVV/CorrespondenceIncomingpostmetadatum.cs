using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#LOW] Ekstra metadata-informasjon knytt til nylig (og til nå uregistrert) innkommende post. Typisk for data hentet fra Svarinn.
    /// </summary>
    public partial class CorrespondenceIncomingpostmetadatum
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// FK
        /// </summary>
        public int IncomingPostId { get; set; }
        /// <summary>
        /// Tittel
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Dato for dokumentet
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Navn på avsender
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Organisasjonsnummer på avsender
        /// </summary>
        public string OrganizationNumber { get; set; }
        /// <summary>
        /// Fødselsnummer på avsender
        /// </summary>
        public string BirthNumber { get; set; }
        /// <summary>
        /// Land på avsender
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// By på avsender
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Postnr på avsender
        /// </summary>
        public string PostNumber { get; set; }
        /// <summary>
        /// Adressefelt på avsender
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// Adressefelt på avsender
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// Adressefelt på avsender
        /// </summary>
        public string Address3 { get; set; }
    }
}
