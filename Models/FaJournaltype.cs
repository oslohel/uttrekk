using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaJournaltype
    {
        public FaJournaltype()
        {
            FaJournals = new HashSet<FaJournal>();
        }

        public string JotIdent { get; set; }
        public string JotBeskrivelse { get; set; }
        public decimal JotFosterhjemsbesoek { get; set; }
        public DateTime? JotPassivisertdato { get; set; }
        public decimal JotIntern { get; set; }
        public decimal JotTilsynsbesoek { get; set; }
        public decimal JotForbindelse { get; set; }
        public decimal JotTilarkiv { get; set; }

        public virtual ICollection<FaJournal> FaJournals { get; set; }
    }
}
