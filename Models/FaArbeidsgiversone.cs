using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaArbeidsgiversone
    {
        public FaArbeidsgiversone()
        {
            FaArbeidsgiveravgifts = new HashSet<FaArbeidsgiveravgift>();
            FaMedarbeideres = new HashSet<FaMedarbeidere>();
        }

        public decimal ArsIdent { get; set; }
        public string ArsSonenavn { get; set; }

        public virtual ICollection<FaArbeidsgiveravgift> FaArbeidsgiveravgifts { get; set; }
        public virtual ICollection<FaMedarbeidere> FaMedarbeideres { get; set; }
    }
}
