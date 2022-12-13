using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class Message
    {
        public Message()
        {
            Dbmessages = new HashSet<Dbmessage>();
        }

        public string Msgid { get; set; }
        public string Msgtitle { get; set; }
        public string Msgtext { get; set; }
        public string Msgicon { get; set; }
        public string Msgbutton { get; set; }
        public decimal? Msgdefaultbutton { get; set; }
        public decimal? Msgseverity { get; set; }
        public string Msgprint { get; set; }
        public string Msguserinput { get; set; }
        public decimal? Msgwidth { get; set; }
        public decimal? Msgheight { get; set; }

        public virtual ICollection<Dbmessage> Dbmessages { get; set; }
    }
}
