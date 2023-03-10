using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Brugner.API.Core.Models.DTOs.Posts
{
    public class PostForUpdateDTO
    {
        [FromRoute]
        [Required]
        public int Id { get; set; }

        [FromForm]
        [Required, MaxLength(100)]
        public string Title { get; set; } = default!;

        [FromForm]
        [Required, MaxLength(200)]
        public string Summary { get; set; } = default!;

        [FromForm]
        [Required]
        public string Content { get; set; } = default!;

        [FromForm]
        [Required]
        public string[] Tags { get; set; } = default!;

        [FromForm]
        [Required]
        public bool IsDraft { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}

