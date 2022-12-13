using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Bruker
    {
        public Bruker()
        {
            enhetskodeModulusBarnListe = new List<string>();
        }

        public string brukerId { get; set; }
        public string okonomiEksternId { get; set; }
        public string brukerNokkelModulusBarn { get; set; }
        public string fulltNavn { get; set; }
        public string email { get; set; }
        public List<string> enhetskodeModulusBarnListe { get; set; }
    }
}
