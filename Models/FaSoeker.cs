using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaSoeker
    {
        public FaSoeker()
        {
            FaGenerellsaks = new HashSet<FaGenerellsak>();
        }

        public decimal SokLoepenr { get; set; }
        public string PnrPostnr { get; set; }
        public string SokFornavn1 { get; set; }
        public string SokEtternavn1 { get; set; }
        public DateTime? SokFoedselsdato1 { get; set; }
        public decimal? SokPersonnr1 { get; set; }
        public string SokFornavn2 { get; set; }
        public string SokEtternavn2 { get; set; }
        public DateTime? SokFoedselsdato2 { get; set; }
        public decimal? SokPersonnr2 { get; set; }
        public string SokAdresse { get; set; }
        public string SokTelefonprivat { get; set; }
        public string SokTelefonarbeid1 { get; set; }
        public string SokTelefonarbeid2 { get; set; }
        public string SokTelefonmobil1 { get; set; }
        public string SokTelefonmobil2 { get; set; }
        public string SokMerknad { get; set; }

        public virtual FaPostadresser PnrPostnrNavigation { get; set; }
        public virtual ICollection<FaGenerellsak> FaGenerellsaks { get; set; }
    }
}
