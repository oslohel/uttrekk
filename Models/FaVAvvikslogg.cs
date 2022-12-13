using System;

namespace UttrekkFamilia.Models
{
    public partial class FaVAvvikslogg
    {
        public string Guid { get; set; }
        public DateTime Loggtidspunkt { get; set; }
        public DateTime? Dato { get; set; }
        public decimal? Klokkeslett { get; set; }
        public int Arbeidstid { get; set; }
        public string Operatoerinitialer { get; set; }
        public string Operatoernavn { get; set; }
        public decimal? Id { get; set; }
        public decimal? Id2 { get; set; }
        public string Sbh1 { get; set; }
        public string Sbh2 { get; set; }
        public string Idtekst { get; set; }
        public string Oppslagstypetekst { get; set; }
        public string Oppslagstype { get; set; }
        public string Detaljer { get; set; }
        public string Referanse { get; set; }
        public DateTime? Loggtidspunkt2 { get; set; }
        public DateTime? Dato2 { get; set; }
        public decimal? Klokkeslett2 { get; set; }
    }
}
