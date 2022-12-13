using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Innbygger
    {
        public Innbygger()
        {
            telefonnummer = new List<AktørTelefonnummer>();
            adresse = new List<AktørAdresse>();
        }

        public string actorId { get; set; }
        public string fodselsnummer { get; set; }
        public string dufNummer { get; set; }
        public string dufNavn { get; set; }
        public string fornavn { get; set; }
        public string mellomnavn { get; set; }
        public string etternavn { get; set; }
        public string kjonn { get; set; }
        public string alternativtKjonn { get; set; }
        public string sivilstand { get; set; }
        public string statsborger { get; set; }
        public string morsmal { get; set; }
        public DateTime? fodselDato { get; set; }
        public int alder { get; set; }
        public bool? avdod { get; set; }
        public DateTime? dodDato { get; set; }
        public bool? ukjentPerson { get; set; }
        public bool? ufodtBarn { get; set; }
        public DateTime? terminDato { get; set; }
        public string beskrivelse { get; set; }
        public string kontonummer { get; set; }
        public bool? potensiellOppdragstaker { get; set; }
        public bool? oppdragstaker { get; set; }
        public bool? ikkeAktuellForOppdrag { get; set; }
        public List<AktørTelefonnummer> telefonnummer { get; set; }
        public List<AktørAdresse> adresse { get; set; }
        public string adressesperre { get; set; }
        public bool? ensligMindreaarig { get; set; }
        public DateTime? bosettingsdato { get; set; }
        public string foedeland { get; set; }
        public string flyktningStatus { get; set; }
        public bool? deaktiver { get; set; }
    }

    public class AktørTelefonnummer
    {
        public string telefonnummerType { get; set; }
        public string telefonnummer { get; set; }
        public bool? hovedtelefon { get; set; }
    }
}
