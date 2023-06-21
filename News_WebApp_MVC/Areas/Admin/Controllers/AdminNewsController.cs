using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News_WebApp_API.DTOs;
using News_WebApp_API.Errors;
using News_WebApp_API.ViewModels;
using News_WebApp_MVC.CustomAttributes;
using Newtonsoft.Json;

namespace News_WebApp_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [TokenAuthorization]

    public class AdminNewsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminNewsController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<IActionResult> ViewNews()
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

        public async Task<IActionResult> AddNews()
        {
            var authorsList = await getAuthorsNamesListAsync();

            if (authorsList != null)
                ViewBag.Authors = authorsList;

            return View();
        }

        public async Task<IActionResult> UpdateNews(int id)
        {
            if (id != 0)
            {
                var response = await _httpClient.GetAsync("https://localhost:44368/api/GetNewsById/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<NewsDTO>(content);

                    var authorsList = await getAuthorsNamesListAsync();

                    if (authorsList != null)
                        ViewBag.Authors = authorsList;

                    return View("AddNews", data);
                }
                else
                {
                    return View("Error");
                }

            }
            else
                return View("AddNews");



        }

        public async Task<IActionResult> DeleteNews(int id)
        {

            //Auth
            if (Request.Cookies.ContainsKey("AdminToken"))
            {
                var token = Request.Cookies["AdminToken"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                return Redirect("/Admin/AdminHome/Unauthorized");
            }

            if (id != 0)
            {
                var response = await _httpClient.DeleteAsync("https://localhost:44368/api/DeleteNews?id=" + id);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("ViewNews");
                else
                    return View("Error");
            }
            else
            {
                return RedirectToAction("ViewNews");
            }

        }

        public async Task<IActionResult> SaveNews(int id, NewsDTO news, IFormFile Files)
        {
            //Auth
            if (Request.Cookies.ContainsKey("AdminToken"))
            {
                var token = Request.Cookies["AdminToken"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                return Redirect("/Admin/AdminHome/Unauthorized");
            }

            var vmNews = new VmAddNews();

            vmNews.AddNewsDTO = news;
            vmNews.NewsImageFile= Files;

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(vmNews.AddNewsDTO.Title), "AddNewsDTO.Title");
            content.Add(new StringContent(vmNews.AddNewsDTO.NewsDescription), "AddNewsDTO.NewsDescription");
            content.Add(new StringContent(vmNews.AddNewsDTO.PublicationDate.ToString()), "AddNewsDTO.PublicationDate");
            content.Add(new StringContent(vmNews.AddNewsDTO.AuthorId.ToString()), "AddNewsDTO.AuthorId");

            if (Files != null)  
            content.Add(new StreamContent(vmNews.NewsImageFile.OpenReadStream()), "NewsImageFile", vmNews.NewsImageFile.FileName);
            

            HttpResponseMessage response = new HttpResponseMessage();

            if (id == 0)
                response = await _httpClient.PostAsync("https://localhost:44368/api/AddaNews", content);
            else
                response = await _httpClient.PutAsync("https://localhost:44368/api/UpdateNews/" + id, content);


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewNews");
            }
            else
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<ErrorValidationResponse>(responseJson);

                ViewBag.Errors = errors.Errors;

                var authorsList = await getAuthorsNamesListAsync();

                if (authorsList != null)
                    ViewBag.Authors = authorsList;

                return View("AddNews",news);
            }
           
        }



            private async Task<List<AuthorDTO>> getAuthorsNamesListAsync()
            {
                var response = await _httpClient.GetAsync("https://localhost:44368/api/GetAllAuthors");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<AuthorDTO>>(content);
                    return data;
                }
                else
                    return null;


            }


        } 
    
   }
