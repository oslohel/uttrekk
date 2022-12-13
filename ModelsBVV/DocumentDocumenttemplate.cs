using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Dokumentmaler
    /// </summary>
    public partial class DocumentDocumenttemplate
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int DocumentDocumentTemplateId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Aktiv fra
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Aktiv til
        /// </summary>
        public DateTime? StopDate { get; set; }
        /// <summary>
        /// Deaktivert av
        /// </summary>
        public int? StoppedBy { get; set; }
        /// <summary>
        /// Navn på malen
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Type mal
        /// </summary>
        public int TemplateType { get; set; }
        /// <summary>
        /// Angir hvilken type hendelse er malen ment for, f.eks. korrespondanse, journal, avtale etc.
        /// </summary>
        public int EventElementType { get; set; }
        /// <summary>
        /// Angir hvilken kategori for valge hendelsestype malen er ment for
        /// </summary>
        public int? CategoryRegistryId { get; set; }
        /// <summary>
        /// Maltekst
        /// </summary>
        public string TemplateText { get; set; }
        /// <summary>
        /// Id til mal for topptekst
        /// </summary>
        public int? HeaderDocumentTemplateId { get; set; }
        /// <summary>
        /// Id til mal for bunntekst
        /// </summary>
        public int? FooterDocumentTemplateId { get; set; }
        /// <summary>
        /// Sideinnstillinger for topptekst
        /// </summary>
        public int? HeaderPageSetting { get; set; }
        /// <summary>
        /// Sideinnstillinger for bunntekst
        /// </summary>
        public int? FooterPageSetting { get; set; }
        /// <summary>
        /// Angir om malen er basert på en annen mal
        /// </summary>
        public int? BasedOnDocumentTemplateId { get; set; }
        /// <summary>
        /// Angir mal for topptekst for side to og utover
        /// </summary>
        public int? SecondHeaderDocumentTemplateId { get; set; }
        /// <summary>
        /// Sideinnstillinger for topptekst for side 2 og utover
        /// </summary>
        public int? SecondHeaderPageSetting { get; set; }
        /// <summary>
        /// Angir mal for bunntekst for side to og utover
        /// </summary>
        public int? SecondFooterDocumentTemplateId { get; set; }
        /// <summary>
        /// Sideinnstillinger for bunntekst for side 2 og utover
        /// </summary>
        public int? SecondFooterPageSetting { get; set; }
        /// <summary>
        /// Innstillingre for marger
        /// </summary>
        public int Margin { get; set; }
        /// <summary>
        /// Om malen er forberedt for CK editor versjon 5
        /// </summary>
        public bool Ck5Ready { get; set; }
        /// <summary>
        /// Maltekst når man benytter CK editor versjon 5
        /// </summary>
        public string Ck5TemplateText { get; set; }
        public int Orientation { get; set; }

        public virtual EnumEventelementtype EventElementTypeNavigation { get; set; }
        public virtual EnumHeaderfooterpagesetting FooterPageSettingNavigation { get; set; }
        public virtual EnumHeaderfooterpagesetting HeaderPageSettingNavigation { get; set; }
        public virtual EnumPdfpagemargin MarginNavigation { get; set; }
        public virtual EnumSecondheaderfooterpagesetting SecondFooterPageSettingNavigation { get; set; }
        public virtual EnumSecondheaderfooterpagesetting SecondHeaderPageSettingNavigation { get; set; }
        public virtual EnumTemplatetype TemplateTypeNavigation { get; set; }
    }
}
