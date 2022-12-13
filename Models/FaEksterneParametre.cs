using System;

namespace UttrekkFamilia.Models
{
    public partial class FaEksterneParametre
    {
        public string DisDistriktskode { get; set; }
        public string EpaRemSti { get; set; }
        public string EpaRemRet { get; set; }
        public string EpaRemDatovalg { get; set; }
        public string EpaRemOppdragskonto { get; set; }
        public decimal? EpaRemKundenr { get; set; }
        public DateTime? EpaRemSistedato { get; set; }
        public string EpaRemDivisjon { get; set; }
        public decimal EpaRemAutofil { get; set; }
        public decimal EpaRemKassabilag { get; set; }
        public string EpaRemBbsAvtaleid { get; set; }
        public string EpaRemBbsKundeid { get; set; }
        public string EpaRgnFiltype { get; set; }
        public string EpaRgnForsystem { get; set; }
        public string EpaRgnFirma { get; set; }
        public string EpaRgnAggregeringsnivaa { get; set; }
        public string EpaRgnBilagsart { get; set; }
        public string EpaRgnAggregerMotpost { get; set; }
        public decimal EpaRgnUtvidetFilnavn { get; set; }
        public string EpaRgnMvakode { get; set; }
        public string EpaRgnSti { get; set; }
        public string EpaLnnKundeid { get; set; }
        public decimal EpaLnnAutofil { get; set; }
        public string EpaLnnKtoArbgiveravg { get; set; }
        public string EpaLnnKtoFeriepenger { get; set; }
        public string EpaLnnKtoArbgiverferie { get; set; }
        public string EpaLnnBalanseArbgiveravg { get; set; }
        public string EpaLnnBalanseFeriepenger { get; set; }
        public string EpaLnnBalanseArbgiverferie { get; set; }
        public string EpaLnnSti { get; set; }
        public string EpaSsbBydelsnr { get; set; }

        public virtual FaDistrikt DisDistriktskodeNavigation { get; set; }
    }
}
