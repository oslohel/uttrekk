using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Undersøkelse
    {
        public Undersøkelse()
        {
            aktivitetIdListe = new List<string>();
        }
        public string undersokelseId { get; set; }
        public string meldingId { get; set; }
        public string sakId { get; set; }
        public string status { get; set; }
        public DateTime opprettetDato { get; set; }
        public DateTime? startDato { get; set; }
        public DateTime? konklusjonsDato { get; set; }
        public DateTime fristDato { get; set; }
        public bool? utvidetFrist { get; set; }
        public string undersokelseGodStart { get; set; }
        public string stoppunkt1 { get; set; }
        public List<string> aktivitetIdListe { get; set; }
        public string forberedelseUndersøkelseAktiviteter { get; set; }
        public string bakgrunnUndersokelsen { get; set; }
        public string hypoteserUndersokelsesperioden { get; set; }
        public string stoppunkt2 { get; set; }
        public string etterUndersokelsesAktiviteten { get; set; }
        public string konklusjon { get; set; }
        public string konklusjonPresisering { get; set; }
        public string vedtakAktivitetId { get; set; }
        public List<string> grunnlagForTiltak { get; set; }
        public string grunnlagForTiltakPresiseringKode18 { get; set; }
        public string grunnlagForTiltakPresiseringKode19 { get; set; }
        public string stoppunkt3 { get; set; }
        public string analysereGodAvslutning { get; set; }
        public string vurdere { get; set; }
        public string beslutte { get; set; }
        public string undersøkelsesrapport { get; set; }
    }
}
