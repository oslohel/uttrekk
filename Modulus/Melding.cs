using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Melding
    {
        public string sakId { get; set; }
        public string meldingId { get; set; }
        public MottattBekymringsmelding mottattBekymringsmelding { get; set; }
        public BehandlingAvBekymringsmelding behandlingAvBekymringsmelding { get; set; }
        public TilbakemeldingTilMelder tilbakemeldingTilMelder { get; set; }
    }

    public class MottattBekymringsmelding
    {
        public string mottattBekymringsmeldingsType { get; set; }
        public DateTime mottattDato { get; set; }
        public DateTime sendingsDato { get; set; }
        public string melderKode { get; set; }
        public string melder { get; set; }
        public string melderFritekst { get; set; }
        public string innhold { get; set; }
        public bool? umiddelbarInngripen { get; set; }
        public List<string> typeMelder { get; set; }
        public string typeMelderPresisering { get; set; }
        public List<string> saksinnhold { get; set; }
        public string saksinnholdPresiseringKode18 { get; set; }
        public string saksinnholdPresiseringKode19 { get; set; }
        public string meldingstype { get; set; }
        public string status { get; set; }
        public string utfortAvId { get; set; }
        public bool? anonymMelder { get; set; }
        public string relasjon { get; set; }
        public string antattAlder { get; set; }
        public string kontaktperson { get; set; }
        public string rolle { get; set; }
        public string behovForTolk { get; set; }
        public string andreHjelpeinstanser { get; set; }
    }

    public class BehandlingAvBekymringsmelding
    {
        public string behandlingId { get; set; }
        public DateTime? pabegyntDato { get; set; }
        public DateTime? konklusjonsdato { get; set; }
        public string vurderingAvInnholdet { get; set; }
        public bool? tilstrekkeligBelyst { get; set; }
        public string vurderingTilstrekkeligBelyst { get; set; }
        public string konklusjon { get; set; }
        public string henlagtKodeverk { get; set; }
        public string vurderingGrunnlagForUndersokelse { get; set; }
        public bool? samtykkeMor { get; set; }
        public bool? iRusbehandling { get; set; }
        public string status { get; set; }
        public string utfortAvId { get; set; }
    }

    public class TilbakemeldingTilMelder
    {
        public string tilbakemeldingId { get; set; }
        public string notat { get; set; }
        public string status { get; set; }
        public string utfortAvId { get; set; }
        public DateTime? utfortDato { get; set; }
    }
}
