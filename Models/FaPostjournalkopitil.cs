namespace UttrekkFamilia.Models
{
    public partial class FaPostjournalkopitil
    {
        public decimal PosAar { get; set; }
        public decimal PosLoepenr { get; set; }
        public decimal PokLoepenr { get; set; }
        public string PnrPostnr { get; set; }
        public string PokFornavn { get; set; }
        public string PokEtternavn { get; set; }
        public string PokPostadresse { get; set; }
        public string PokBesoeksadresse { get; set; }
        public decimal PokAnonymisert { get; set; }
        public decimal PokMedadressat { get; set; }
        public decimal PokDigitalpost { get; set; }
        public decimal? PokForbLoepenr { get; set; }
        public string PokDigitalpostRef { get; set; }
        public decimal PokTmpDigitalstatus { get; set; }
        public decimal? KliLoepenrKopiFamilie { get; set; }

        public virtual FaKlient KliLoepenrKopiFamilieNavigation { get; set; }
        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual FaPostjournal Pos { get; set; }
    }
}
