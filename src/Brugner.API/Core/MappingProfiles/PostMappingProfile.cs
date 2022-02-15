using System;
using AutoMapper;
using Brugner.API.Core.Models.DTOs.Posts;
using Brugner.API.Core.Models.Entities;

namespace Brugner.API.Core.MappingProfiles
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)));

            CreateMap<PostForCreationDTO, Post>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => string.Join(',', src.Tags)));

            CreateMap<PostForUpdateDTO, Post>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => string.Join(',', src.Tags)))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
