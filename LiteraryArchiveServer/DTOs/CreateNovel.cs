namespace LiteraryArchiveServer.DTOs
{
    public class CreateNovel
    {
        public required int Isbn { get; set; }
        public required int GenreId { get; set; }
        public required string Title { get; set; } = null!;
        public required string Author { get; set; } = null!;
    }
}
