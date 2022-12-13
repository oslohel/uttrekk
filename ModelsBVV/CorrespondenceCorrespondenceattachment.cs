using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#HIGH] Tabell for vedlegg (fra andre korrespondanser, journaler eller andre hendelser). Gjelder utgående korrespondanse
    /// </summary>
    public partial class CorrespondenceCorrespondenceattachment
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id til korrespondanse
        /// </summary>
        public int CorrespondenceId { get; set; }
        /// <summary>
        /// Id til vedlegg
        /// </summary>
        public string AttachmentId { get; set; }
        /// <summary>
        /// Navn på vedlegg
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id til fil
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// Passord til filen
        /// </summary>
        public string FilePassword { get; set; }
        /// <summary>
        /// Fil-data
        /// </summary>
        public byte[] FileData { get; set; }
        public byte[] FileDataBlob { get; set; }
        /// <summary>
        /// Id til hendelse, kan være korrespondanse, journal etc.
        /// </summary>
        public int? EventElementId { get; set; }
        /// <summary>
        /// Type hendelse, kan være korrespondanse, journal etc.
        /// </summary>
        public int? EventElementType { get; set; }
        /// <summary>
        /// -
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// -
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Angir om vedlegget er internt eller inndratt
        /// </summary>
        public bool? InternalOrWithheld { get; set; }
        /// <summary>
        /// Angir hvilken type internt or inndratt
        /// </summary>
        public int? InternalOrWithheldType { get; set; }
    }
}
