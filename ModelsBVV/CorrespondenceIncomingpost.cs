using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#LOW] Tabell som viser nylig (og til nå uregistrert) post. Kan komme fra skannet post, post fra SvarInn, opplastet dokument, dokument videresendt fra annet fagsystem, eller feilregistrerte korrespondanser som er &amp;apos;returnert til post&amp;apos; av en ansatt
    /// </summary>
    public partial class CorrespondenceIncomingpost
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int CorrespondenceIncomingPostId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Angir hvilken type post det er, f.eks. skannet, svarinn etc.
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Dato da den blir lagt inn i listen over innkommende post
        /// </summary>
        public DateTime AddedDate { get; set; }
        /// <summary>
        /// Dato da filen i den innkommende post var laget
        /// </summary>
        public DateTime FileCreatedDate { get; set; }
        /// <summary>
        /// Filnavn
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Selve fildataene om de ikke er lagret i FileDataBlob.
        /// </summary>
        public byte[] FileData { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Opprinnelig fil-identifikator til dokumentet (ikke i bruk)
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// Opprinnelig passord til dokumentet (ikke i bruk)
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Navnet på fagsystemet som dokumentet kommer fra
        /// </summary>
        public string SpecialistSystemName { get; set; }
        /// <summary>
        /// Organisasjonsnummer som dokumentet tilhører
        /// </summary>
        public string OrganizationNumber { get; set; }
        /// <summary>
        /// Fildata for PDF dokument
        /// </summary>
        public byte[] FileDataBlob { get; set; }

        public virtual EnumIncomingpoststatus StatusNavigation { get; set; }
        public virtual EnumIncomingposttype TypeNavigation { get; set; }
    }
}
