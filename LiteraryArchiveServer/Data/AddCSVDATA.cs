namespace LiteraryArchiveServer.Data
{
    public class AddCSVDATA
    {
        public required string title { get; set; }
        public required int isbn { get; set; }
        public required string genre { get; set; }
        public required string author { get; set; }
        public required string keyword { get; set; }
        public required string rating { get; set; }
    }
}
