using System;
using Brugner.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brugner.API.Infrastructure.EntitiesConfiguration
{
    public class PostEntityConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Summary).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Slug).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.Property(x => x.Tags).HasMaxLength(100).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired(false);
        }
    }
}

