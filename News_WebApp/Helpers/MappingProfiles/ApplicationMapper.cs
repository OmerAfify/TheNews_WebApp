
using AutoMapper;
using Domains.Models;
using News_WebApp_API.DTOs;
using News_WebApp_API.Helpers.ValueResolvers;

namespace News_WebApp_API.Helpers.MappingProfiles
{
    public class ApplicationMapper : Profile
    {

        public ApplicationMapper()
        {
            CreateMap<News, AddNewsDTO>().ReverseMap();

            CreateMap<NewsDTO, News>().ReverseMap()
                      .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author.Name)).ReverseMap();

            CreateMap<News, NewsDTO>()
                .ForMember(d => d.NewsImagePath, opt => opt.MapFrom<NewsPictureUrlResolver>()).ReverseMap();






            CreateMap<Author, AddAuthorDTO>().ReverseMap();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            

        }
    }
}
