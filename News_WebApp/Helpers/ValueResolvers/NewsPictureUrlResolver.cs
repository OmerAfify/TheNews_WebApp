using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domains.Models;
using Microsoft.Extensions.Configuration;
using News_WebApp_API.DTOs;

namespace News_WebApp_API.Helpers.ValueResolvers
{
    public class NewsPictureUrlResolver : IValueResolver<News, NewsDTO, string>
    {
        private readonly IConfiguration _configuration;

        public NewsPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(News source, NewsDTO destination, string destMember, ResolutionContext context)
        {
          
                if (!string.IsNullOrEmpty(source.NewsImagePath))
                {
                    return _configuration["ApiUrl"] + source.NewsImagePath + source.NewsImageName;
                }
                else
                {
                    return null;
                }

        }
    }
}
