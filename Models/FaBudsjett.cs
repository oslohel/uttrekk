namespace UttrekkFamilia.Models
{
    public partial class FaBudsjett
    {
        public decimal BudBudsjettaar { get; set; }
        public decimal BudLoepenr { get; set; }
        public string KtpNoekkel1 { get; set; }
        public string KtnKontonummer1 { get; set; }
        public string KtpNoekkel2 { get; set; }
        public string KtnKontonummer2 { get; set; }
        public string KtpNoekkel3 { get; set; }
        public string KtnKontonummer3 { get; set; }
        public string KtpNoekkel4 { get; set; }
        public string KtnKontonummer4 { get; set; }
        public string KtpNoekkel5 { get; set; }
        public string KtnKontonummer5 { get; set; }
        public string KtpNoekkel6 { get; set; }
        public string KtnKontonummer6 { get; set; }
        public decimal? BudBudsjett01 { get; set; }
        public decimal? BudBudsjett02 { get; set; }
        public decimal? BudBudsjett03 { get; set; }
        public decimal? BudBudsjett04 { get; set; }
        public decimal? BudBudsjett05 { get; set; }
        public decimal? BudBudsjett06 { get; set; }
        public decimal? BudBudsjett07 { get; set; }
        public decimal? BudBudsjett08 { get; set; }
        public decimal? BudBudsjett09 { get; set; }
        public decimal? BudBudsjett10 { get; set; }
        public decimal? BudBudsjett11 { get; set; }
        public decimal? BudBudsjett12 { get; set; }
        public decimal? BudBudsjett13 { get; set; }
        public decimal BudBudsjettramme { get; set; }

        public virtual FaKontoer Kt { get; set; }
        public virtual FaKontoer Kt1 { get; set; }
        public virtual FaKontoer Kt2 { get; set; }
        public virtual FaKontoer Kt3 { get; set; }
        public virtual FaKontoer Kt4 { get; set; }
        public virtual FaKontoer KtNavigation { get; set; }
    }
}
