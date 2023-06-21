using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News_WebApp_API.DTOs;
using News_WebApp_MVC.CustomAttributes;
using Newtonsoft.Json;

namespace News_WebApp_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
   [TokenAuthorization]
    public class AdminAuthorsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminAuthorsController (HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
     
        public async Task<IActionResult> ViewAuthors()
        {
            var response = await _httpClient.GetAsync("https://localhost:44368/api/GetAllAuthors");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<AuthorDTO>>(content);

                return View(data);
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult AddAuthor()
        {
           return View();
        }
        public async Task<IActionResult> UpdateAuthor(int id)
        {
            if (id!= 0)
            {
                var response = await _httpClient.GetAsync("https://localhost:44368/api/GetAuthorById/"+id);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<AuthorDTO>(content);

                    return View("AddAuthor",data);
                }
                else
                {
                    return View("Error");
                }

            }
            else
                  return View("AddAuthor");
            

          
        }

        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id != 0)
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


                var response = await _httpClient.DeleteAsync("https://localhost:44368/api/DeleteAuthors?id=" + id);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("ViewAuthors");
                else
                    return View("Error");
            }
            else
            {
                return RedirectToAction("ViewAuthors");
            }

        }

        public async Task<IActionResult> SaveAuthor(int id, AddAuthorDTO author)
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


            // Serialize the model data as JSON
            string json = JsonConvert.SerializeObject(author);

            // Create a StringContent object from the JSON data
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = new HttpResponseMessage();


            if (id == 0)
            {
                //add
                 response = await _httpClient.PostAsync("https://localhost:44368/api/AddNewAuthors", content);

            }
            else
            {
                //edit
                 response = await _httpClient.PutAsync("https://localhost:44368/api/UpdateAuthors/"+id, content);
            }


            // Check the response status code
            if (response.IsSuccessStatusCode)
                    // Redirect to the details page for the new data item
                    return RedirectToAction("ViewAuthors");
                else
                    // Display an error message
                    return RedirectToAction("/AdminHome/Index");   
        }



    }
}
