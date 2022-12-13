using System;

namespace UttrekkFamilia.Models
{
    public partial class FaSsbStatistikk
    {
        public int SsbId { get; set; }
        public int SsbYear { get; set; }
        public DateTime SsbOrderdate { get; set; }
        public byte[] SsbParameters { get; set; }
        public decimal SsbCanBeFinalized { get; set; }
        public decimal SsbFinal { get; set; }
        public decimal SsbIsDownloaded { get; set; }
        public byte[] SsbResultxml { get; set; }
        public byte[] SsbErrormessages { get; set; }
        public string SbhInitialer { get; set; }
        public decimal KomKommunenr { get; set; }
        public decimal? DokLoepenr { get; set; }
        public byte[] SsbValidationerrors { get; set; }
        public byte[] SsbActiveWoActivity { get; set; }

        public virtual FaDokumenter DokLoepenrNavigation { get; set; }
        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
