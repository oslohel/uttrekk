namespace UttrekkFamilia.Models
{
    public partial class Dbmessage
    {
        public string Dbid { get; set; }
        public decimal Dberrorid { get; set; }
        public string Refmsgid { get; set; }
        public decimal? Errorid { get; set; }

        public virtual Message Refmsg { get; set; }
    }
}
