using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaUtbetalingsvilkaar
    {
        public FaUtbetalingsvilkaar()
        {
            FaBetalingers = new HashSet<FaBetalinger>();
            FaEngasjementslinjers = new HashSet<FaEngasjementslinjer>();
        }

        public string VikType { get; set; }
        public string VikBeskrivelse { get; set; }

        public virtual ICollection<FaBetalinger> FaBetalingers { get; set; }
        public virtual ICollection<FaEngasjementslinjer> FaEngasjementslinjers { get; set; }
    }
}
