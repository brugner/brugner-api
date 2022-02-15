using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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

        [FromForm]
        public IFormFile? Thumbnail { get; set; } = default!;

        public override string ToString()
        {
            return Title;
        }
    }
}

