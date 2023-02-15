using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Aktivitet
    {
        public Aktivitet()
        {
            parter = new List<string>();
            årsakskoderUtvidelseAvFristUndersokelse = new List<string>();
        }

        public string aktivitetId { get; set; }
        public string sakId { get; set; }
        public string aktivitetsType { get; set; }
        public string aktivitetsUnderType { get; set; }
        public List<string> parter { get; set; }
        public DateTime? hendelsesdato { get; set; }
        public string saksbehandlerId { get; set; }
        public string status { get; set; }
        public string tittel { get; set; }
        public string notat { get; set; }
        public string kilde { get; set; }
        public string prioritet { get; set; }
        public string utfortAvId { get; set; }
        public DateTime? utfortDato { get; set; }
        public bool? lovPaalagt { get; set; }
        public DateTime? fristDato { get; set; }
        public bool? fristLovpaalagt { get; set; }
        public string fristTitel { get; set; }
        public string fristBeskrivelse { get; set; }
        public List<string> årsakskoderUtvidelseAvFristUndersokelse { get; set; }
        public string presiseringAvÅrsaksUtvidelseAvFristUndersokelse { get; set; }
        public string tilbudOmEttervern { get; set; }
        public string kravFraPrivatPartVedtakId { get; set; }
        public bool? kravFraPrivatPartOverføresNemnd { get; set; }
        public DateTime? kravFraPrivatPartOverføresNemndEllerBortfaltDato { get; set; }
        public string tilsynAnsvarligKommunenummer { get; set; }
        public string tiltaksId { get; set; }
    }
}
