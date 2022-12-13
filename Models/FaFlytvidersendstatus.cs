using System;

namespace UttrekkFamilia.Models
{
    public partial class FaFlytvidersendstatus
    {
        public Guid FlySendRef { get; set; }
        public int FlyStatus { get; set; }
        public string FlyBeskrivelse { get; set; }
    }
}
