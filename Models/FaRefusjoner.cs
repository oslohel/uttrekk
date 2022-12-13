using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaRefusjoner
    {
        public FaRefusjoner()
        {
            FaBetalingsplaners = new HashSet<FaBetalingsplaner>();
            FaEngasjementsplans = new HashSet<FaEngasjementsplan>();
            RerLoepenrs = new HashSet<FaRefusjonskrav>();
        }

        public decimal RefLoepenr { get; set; }
        public decimal KliLoepenr { get; set; }
        public DateTime? RefFradato { get; set; }
        public DateTime? RefTildato { get; set; }
        public decimal RefStatus { get; set; }
        public decimal? PosAarSoeknad { get; set; }
        public decimal? PosLoepenrSoeknad { get; set; }
        public decimal? PosAarTilsagn { get; set; }
        public decimal? PosLoepenrTilsagn { get; set; }
        public string RefFosArbGodBeskrivelse { get; set; }
        public decimal RefFosArbGodBeregnet { get; set; }
        public decimal RefFosArbGodJustert { get; set; }
        public string RefFosArbGivBeskrivelse { get; set; }
        public decimal RefFosArbGivBeregnet { get; set; }
        public decimal RefFosArbGivJustert { get; set; }
        public string RefFosUtgDekBeskrivelse { get; set; }
        public decimal RefFosUtgDekBeregnet { get; set; }
        public decimal RefFosUtgDekJustert { get; set; }
        public string RefFosEksUtgBeskrivelse { get; set; }
        public decimal RefFosEksUtgBeregnet { get; set; }
        public decimal RefFosEksUtgJustert { get; set; }
        public string RefStoArbGodBeskrivelse { get; set; }
        public decimal RefStoArbGodBeregnet { get; set; }
        public decimal RefStoArbGodJustert { get; set; }
        public string RefStoArbGivBeskrivelse { get; set; }
        public decimal RefStoArbGivBeregnet { get; set; }
        public decimal RefStoArbGivJustert { get; set; }
        public string RefStoUtgDekBeskrivelse { get; set; }
        public decimal RefStoUtgDekBeregnet { get; set; }
        public decimal RefStoUtgDekJustert { get; set; }
        public string RefBesArbGodBeskrivelse { get; set; }
        public decimal RefBesArbGodBeregnet { get; set; }
        public decimal RefBesArbGodJustert { get; set; }
        public string RefBesArbGivBeskrivelse { get; set; }
        public decimal RefBesArbGivBeregnet { get; set; }
        public decimal RefBesArbGivJustert { get; set; }
        public string RefBesUtgDekBeskrivelse { get; set; }
        public decimal RefBesUtgDekBeregnet { get; set; }
        public decimal RefBesUtgDekJustert { get; set; }
        public string RefAn1UtgDekBeskrivelse { get; set; }
        public decimal RefAn1UtgDekBeregnet { get; set; }
        public decimal RefAn1UtgDekJustert { get; set; }
        public string RefAn2UtgDekBeskrivelse { get; set; }
        public decimal RefAn2UtgDekBeregnet { get; set; }
        public decimal RefAn2UtgDekJustert { get; set; }
        public decimal RefEgenandelBelop { get; set; }
        public string RefLeverandoernr { get; set; }
        public DateTime? RefRegistrertdato { get; set; }
        public DateTime? RefEndretdato { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaPostjournal Pos { get; set; }
        public virtual FaPostjournal PosNavigation { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaBetalingsplaner> FaBetalingsplaners { get; set; }
        public virtual ICollection<FaEngasjementsplan> FaEngasjementsplans { get; set; }

        public virtual ICollection<FaRefusjonskrav> RerLoepenrs { get; set; }
    }
}
