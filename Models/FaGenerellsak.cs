using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaGenerellsak
    {
        public FaGenerellsak()
        {
            FaAktivitets = new HashSet<FaAktivitet>();
            InverseGsaErstattetav = new HashSet<FaGenerellsak>();
        }

        public decimal GsaAar { get; set; }
        public decimal GsaJournalnr { get; set; }
        public decimal? GsaErstattetavAar { get; set; }
        public decimal? GsaErstattetavJournalnr { get; set; }
        public decimal? KliLoepenr { get; set; }
        public string MynVedtakstype { get; set; }
        public decimal? SokLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhInitialer2 { get; set; }
        public string SbhEndretav { get; set; }
        public decimal? PosAar { get; set; }
        public decimal? PosLoepenr { get; set; }
        public decimal? KomKommunenr { get; set; }
        public string SatSakstype { get; set; }
        public string SbhRegistrertav { get; set; }
        public string DisDistriktskode { get; set; }
        public DateTime GsaDato { get; set; }
        public string GsaStatus { get; set; }
        public string GsaEmne { get; set; }
        public string GsaKonklusjon { get; set; }
        public DateTime? GsaKonklusjonsdato { get; set; }
        public DateTime? GsaVideresendtdato { get; set; }
        public DateTime? GsaRegistrertdato { get; set; }
        public DateTime? GsaEndretdato { get; set; }
        public decimal GsaAnbefaling { get; set; }
        public string GsaSbhKommune { get; set; }
        public decimal? GsaAntallbarn { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
        public virtual FaGenerellsak GsaErstattetav { get; set; }
        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaKommuner KomKommunenrNavigation { get; set; }
        public virtual FaVedtaksmyndighet MynVedtakstypeNavigation { get; set; }
        public virtual FaPostjournal Pos { get; set; }
        public virtual FaSakstype SatSakstypeNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialer2Navigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaSoeker SokLoepenrNavigation { get; set; }
        public virtual ICollection<FaAktivitet> FaAktivitets { get; set; }
        public virtual ICollection<FaGenerellsak> InverseGsaErstattetav { get; set; }
    }
}
