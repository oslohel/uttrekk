using System;

namespace UttrekkFamilia.Models
{
    public partial class FaKmsatser
    {
        public string KmgIdent { get; set; }
        public DateTime KmsFradato { get; set; }
        public DateTime? KmsTildato { get; set; }
        public decimal KmsSats { get; set; }

        public virtual FaKilometergodtgjoerelse KmgIdentNavigation { get; set; }
    }
}
