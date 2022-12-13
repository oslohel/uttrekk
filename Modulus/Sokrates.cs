using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Sokrates
    {
        public Sokrates()
        {
            tidligereAvdelinger = new List<TidligereAvdeling>();
        }
        public decimal kliLoepenr { get; set; }
        public string fodselsnummer { get; set; }
        public bool eierBydel { get; set; }
        public List<TidligereAvdeling> tidligereAvdelinger { get; set; }
    }
}
