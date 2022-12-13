using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaFristoversittelser
    {
        public FaFristoversittelser()
        {
            FaMeldingerFro1s = new HashSet<FaMeldinger>();
            FaMeldingerFroNavigations = new HashSet<FaMeldinger>();
            FaMeldingerFros = new HashSet<FaMeldinger>();
            FaTiltakFro1s = new HashSet<FaTiltak>();
            FaTiltakFroNavigations = new HashSet<FaTiltak>();
            FaTiltakFros = new HashSet<FaTiltak>();
            FaUndersoekelserFro1s = new HashSet<FaUndersoekelser>();
            FaUndersoekelserFroNavigations = new HashSet<FaUndersoekelser>();
            FaUndersoekelserFros = new HashSet<FaUndersoekelser>();
        }

        public string FroType { get; set; }
        public string FroKode { get; set; }
        public string FroBeskrivelse { get; set; }

        public virtual ICollection<FaMeldinger> FaMeldingerFro1s { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerFroNavigations { get; set; }
        public virtual ICollection<FaMeldinger> FaMeldingerFros { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakFro1s { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakFroNavigations { get; set; }
        public virtual ICollection<FaTiltak> FaTiltakFros { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserFro1s { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserFroNavigations { get; set; }
        public virtual ICollection<FaUndersoekelser> FaUndersoekelserFros { get; set; }
    }
}
