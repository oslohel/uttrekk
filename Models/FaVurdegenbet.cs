using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaVurdegenbet
    {
        public FaVurdegenbet()
        {
            FaBetalingsplaners = new HashSet<FaBetalingsplaner>();
            FaInntutgs = new HashSet<FaInntutg>();
        }

        public decimal VurLoepenr { get; set; }
        public decimal TilLoepenr { get; set; }
        public decimal? PosAar { get; set; }
        public decimal? PosLoepenr { get; set; }
        public string SbhInitialer { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public string VurHvem { get; set; }
        public DateTime VurDato { get; set; }
        public decimal? VurAntVoksne { get; set; }
        public decimal? VurAntBarn { get; set; }
        public decimal? VurNorm { get; set; }
        public decimal? VurInntekt { get; set; }
        public decimal? VurFormue { get; set; }
        public string VurVurdering { get; set; }
        public string VurKonklusjon { get; set; }
        public DateTime? VurRegistrertdato { get; set; }
        public DateTime? VurEndretdato { get; set; }

        public virtual FaPostjournal Pos { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhInitialerNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual FaTiltak TilLoepenrNavigation { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplaners { get; set; }
        public virtual ICollection<FaInntutg> FaInntutgs { get; set; }
    }
}
