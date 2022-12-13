namespace UttrekkFamilia.Models
{
    public partial class FaSbhSetting
    {
        public string SbhInitialer { get; set; }
        public byte[] SbsSettinger { get; set; }
        public bool? SbhUsenativespellcheck { get; set; }

        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
    }
}
