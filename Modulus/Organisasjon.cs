using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Organisasjon
    {
        public Organisasjon()
        {
            kategori = new List<string>();
            telefonnummer = new List<AktørTelefonnummer>();
            adresse = new List<AktørAdresse>();
        }

        public string actorId { get; set; }
        public string navn { get; set; }
        public string epost { get; set; }
        public string hjemmeside { get; set; }
        public string beskrivelse { get; set; }
        public string organisasjonsnummer { get; set; }
        public string kontonummer { get; set; }
        public string eksternId { get; set; }
        public List<string> kategori { get; set; }
        public string leverandørAvTiltak { get; set; }

        public List<AktørTelefonnummer> telefonnummer { get; set; }
        public List<AktørAdresse> adresse { get; set; }
        public bool? deaktiver { get; set; }
    }

    public class AktørAdresse
    {
        public AktørAdresse()
        {
        }

        public string adresseId { get; set; }
        public string adresseType { get; set; }
        public string linje1 { get; set; }
        public string linje2 { get; set; }
        public string linje3 { get; set; }
        public string linje4 { get; set; }
        public string linje5 { get; set; }
        public string postnummer { get; set; }
        public string poststed { get; set; }
        public string landkode { get; set; }
        public string kommentar { get; set; }
        public bool? hovedadresse { get; set; }
    }
}
