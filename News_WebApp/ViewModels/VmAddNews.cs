using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using News_WebApp_API.DTOs;

namespace News_WebApp_API.ViewModels
{
    public class VmAddNews
    {
        public AddNewsDTO AddNewsDTO { get; set; }
        public IFormFile NewsImageFile{ get; set; }
    }
}
