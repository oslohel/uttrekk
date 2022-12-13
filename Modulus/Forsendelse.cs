using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Forsendelse
    {
        public Forsendelse()
        {
            mottakerIdListe = new List<string>();
            dokumentIdListe = new List<string>();
        }

        public string aktivitetId { get; set; }
        public string tittel { get; set; }
        public List<string> mottakerIdListe { get; set; }
        public string avsenderId { get; set; }
        public DateTime? sendtDato { get; set; }
        public List<string> dokumentIdListe { get; set; }
        public string hovedDokumentId { get; set; }
    }
}
