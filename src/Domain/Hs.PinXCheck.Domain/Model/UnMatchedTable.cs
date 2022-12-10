namespace Hs.PinXCheck.Domain.Model
{
    public class UnMatchedTable
    {
        public string FileName { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public bool MatchedName { get; set; }

        public string MatchedDescription { get; set; }

        public bool FlagRename { get; set; }

    }
}
