namespace Hs.PinXCheck.Domain.Model
{
    public class IpdbDatabase : PinballXTable
    {
        public int Id { get; set; }

        public byte Players { get; set; }

        public string Abbreviation { get; set; }

        public int Units { get; set; }
    }
}
