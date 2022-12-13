namespace UttrekkFamilia.Modulus
{
    public class BarnetsNettverk
    {
        public BarnetsNettverk()
        {
        }

        public string sakId { get; set; }
        public string actorId { get; set; }
        public string relasjonTilSak { get; set; }
        public string rolle { get; set; }
        public bool? foresatt { get; set; }
        public string kommentar { get; set; }
    }
}
