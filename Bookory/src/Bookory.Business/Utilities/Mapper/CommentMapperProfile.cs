using AutoMapper;
using Bookory.Business.Utilities.DTOs.CommentDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class CommentMapperProfile : Profile
{
	public CommentMapperProfile()
	{
        CreateMap<CommentPostDto, Comment>();
        CreateMap<Comment, CommentGetResponseDto>().ReverseMap();
    }
}
