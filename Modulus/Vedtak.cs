using System;

namespace UttrekkFamilia.Modulus
{
    public class Vedtak
    {
        public Vedtak()
        {
        }

        public string sakId { get; set; }
        public string aktivitetId { get; set; }
        public string saksbehandlerId { get; set; }
        public string godkjentAvSaksbehandlerId { get; set; }
        public string aktivitetsUndertype { get; set; }
        public DateTime? startdato { get; set; }
        public string tittel { get; set; }
        public DateTime? vedtaksdato { get; set; }
        public string status { get; set; }
        public DateTime? godkjentStatusDato { get; set; }
        public DateTime? begjaeringOversendtNemndStatusDato { get; set; }
        public DateTime? bortfaltStatusDato { get; set; }
        public DateTime? avsluttetStatusDato { get; set; }
        public string lovhjemmel { get; set; }
        public string jfLovhjemmelNr1 { get; set; }
        public string jfLovhjemmelNr2 { get; set; }
        public string jfLovhjemmelNr3 { get; set; }
        public string avslutningsArsak { get; set; }
        public string avslutningsårsakPresisering { get; set; }
        public string arsakIkkeEttervern { get; set; }
        public string barnetsMedvirkning { get; set; }
        public string bakgrunnsopplysninger { get; set; }
        public string vedtak { get; set; }
        public string begrunnelse { get; set; }
        public bool? klagesakStatsforvalter { get; set; }
        public bool? vedtakFraBarnevernsvakt { get; set; }
        public string vedtakstype { get; set; }
        public string beslutning { get; set; }
        public string rettsinstans { get; set; }
        public string behandlingIFylkesnemda { get; set; }
        public bool? beslutningOmAdresseSperre { get; set; }
        public string antallFremtidigeÅrligeTilsyn { get; set; }
    }
}
