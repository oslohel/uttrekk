using System;

namespace UttrekkFamilia.Modulus
{
    public class Henvendelse
    {
        public string sakId { get; set; }
        public string aktivitetId { get; set; }
        public string henvendelseMottaksmåte { get; set; }
        public string henvendelseMelderType { get; set; }
        public string henvendelseMelderPartId { get; set; }
        public string henvendelseKategori { get; set; }
        public string henvendelseKommunenummer { get; set; }
        public string henvendelseKommuneBydel { get; set; }
        public bool? henvendelseKreverUmiddelbarInngripen { get; set; }
        public string henvendelseInnhold { get; set; }
        public string status { get; set; }
        public string utfortAvId { get; set; }
        public DateTime? henvendelsesDato { get; set; }
        public string aktivitetsUndertype { get; set; }
    }
}
