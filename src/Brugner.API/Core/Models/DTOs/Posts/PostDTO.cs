using System;
namespace Brugner.API.Core.Models.DTOs.Posts
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Summary { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string[] Tags { get; set; } = default!;
        public bool IsDraft { get; set; }
        public string? Thumbnail { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Title}";
        }
    }
}

