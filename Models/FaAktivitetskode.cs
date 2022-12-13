using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaAktivitetskode
    {
        public FaAktivitetskode()
        {
            FaAktivitets = new HashSet<FaAktivitet>();
            FaBehandlingsmallinjes = new HashSet<FaBehandlingsmallinje>();
        }

        public string AkkType { get; set; }
        public string AkkBeskrivelse { get; set; }
        public DateTime? AkkPassivisertdato { get; set; }
        public string AkkBhmaate { get; set; }

        public virtual ICollection<FaAktivitet> FaAktivitets { get; set; }
        public virtual ICollection<FaBehandlingsmallinje> FaBehandlingsmallinjes { get; set; }
    }
}
