using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKkoder
    {
        public FaKkoder()
        {
            FaPostjournalKkoKodeFagNavigations = new HashSet<FaPostjournal>();
            FaPostjournalKkoKodeFellesNavigations = new HashSet<FaPostjournal>();
            FaPostjournalKkoKodeTilleggNavigations = new HashSet<FaPostjournal>();
        }

        public string KkoKode { get; set; }
        public string KkoKategori { get; set; }
        public string KkoBeskrivelse { get; set; }

        public virtual ICollection<FaPostjournal> FaPostjournalKkoKodeFagNavigations { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalKkoKodeFellesNavigations { get; set; }
        public virtual ICollection<FaPostjournal> FaPostjournalKkoKodeTilleggNavigations { get; set; }
    }
}
