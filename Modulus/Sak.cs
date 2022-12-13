using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Sak
    {
        public Sak()
        {
            sekunderSaksbehandlerId = new List<string>();
            sekunderAvdelingId = new List<string>();
            tidligereAvdelingListe = new List<TidligereAvdeling>();
        }

        public string sakId { get; set; }
        public string avdelingId { get; set; }
        public string saksbehandlerId { get; set; }
        public string aktorId { get; set; }
        public DateTime? startDato { get; set; }
        public DateTime? sluttDato { get; set; }
        public List<string> sekunderSaksbehandlerId { get; set; }
        public List<string> sekunderAvdelingId { get; set; }
        public string status { get; set; }
        public string konfidensialitet { get; set; }
        public string saksnavn { get; set; }
        public string sakstype { get; set; }
        public string merknad { get; set; }
        public string advarsel { get; set; }
        public string arbeidsbelastning { get; set; }
        public string risiko { get; set; }
        public bool? tolk { get; set; }
        public string tolkBeskrivelse { get; set; }
        public List<TidligereAvdeling> tidligereAvdelingListe { get; set; }
        public string typeBarnevernsvaktsak { get; set; }
        public string hovedkategori { get; set; }
        public string melder { get; set; }
    }

    public class TidligereAvdeling
    {
        public string avdelingId { get; set; }
        public DateTime fraDato { get; set; }
        public DateTime tilDato { get; set; }
    }
}
