using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News_WebApp_API.DTOs;
using Newtonsoft.Json;

namespace News_WebApp_MVC.Controllers
{
    public class NewsController : Controller
    {

        private readonly HttpClient _httpClient;

        public NewsController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<IActionResult> ViewNews(int id)
        {
            var response = await _httpClient.GetAsync("https://localhost:44368/api/GetNewsById/"+id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<NewsDTO>(content);

                return View(data);
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> ViewAllNews()
        {
            var response = await _httpClient.GetAsync("https://localhost:44368/api/GetAllNews");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<NewsDTO>>(content);

                return View(data);
            }
            else
            {
                return View("Error");
            }
        }



    }
}
