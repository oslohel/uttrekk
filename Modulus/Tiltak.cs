using System;

namespace UttrekkFamilia.Modulus
{
    public class Tiltak
    {
        public Tiltak()
        {
        }

        public string tiltakId { get; set; }
        public string sakId { get; set; }
        public string aktivitetId { get; set; }
        public string ssbHovedkategori { get; set; }
        public string ssbUnderkategori { get; set; }
        public string ssbUnderkategoriSpesifisering { get; set; }
        public string iEllerUtenforFamilie { get; set; }
        public DateTime? planlagtFraDato { get; set; }
        public DateTime? planlagtTilDato { get; set; }
        public DateTime? iverksattDato { get; set; }
        public DateTime? bortfaltDato { get; set; }
        public string bortfaltKommentar { get; set; }
        public DateTime? avsluttetDato { get; set; }
        public string avsluttetKode { get; set; }
        public string avsluttetSpesifisering { get; set; }
        public string arsakFlyttingFraFosterhjemInstitusjon { get; set; }
        public string arsakFlyttingFraPresisering { get; set; }
        public string flyttingTil { get; set; }
        public string presiseringAvBosted { get; set; }
        public string notat { get; set; }
        public string lovhjemmel { get; set; }
        public string jfLovhjemmelNr1 { get; set; }
        public string jfLovhjemmelNr2 { get; set; }
        public string jfLovhjemmelNr3 { get; set; }
        public string tilsynskommunenummer { get; set; }
    }
}
