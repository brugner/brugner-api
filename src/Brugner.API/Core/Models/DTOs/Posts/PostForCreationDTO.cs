using System;
using System.ComponentModel.DataAnnotations;

namespace Brugner.API.Core.Models.DTOs.Posts
{
    public class PostForCreationDTO
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = default!;

        [Required, MaxLength(200)]
        public string Summary { get; set; } = default!;

        [Required]
        public string Content { get; set; } = default!;

        [Required]
        public string[] Tags { get; set; } = default!;

        [Required]
        public bool IsDraft { get; set; }

        public IFormFile? Thumbnail { get; set; } = default!;

        public override string ToString()
        {
            return Title;
        }
    }
}

