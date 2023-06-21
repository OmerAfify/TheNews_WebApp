using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News_WebApp_API.DTOs;
using News_WebApp_API.Errors;
using News_WebApp_MVC.CustomAttributes;
using Newtonsoft.Json;

namespace News_WebApp_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminAccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LoginAdmin(LoginDTO loginDTO)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44368/api/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var contentData = JsonConvert.DeserializeObject<UserDTO>(responseContent);


                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(15)
                };

             Response.Cookies.Append("AdminToken", contentData.Token, cookieOptions);
             Response.Cookies.Append("AdminUserName", contentData.Name, cookieOptions);

             return Redirect("/Admin/AdminHome");


            }
            else
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<ErrorValidationResponse>(responseJson);
                ViewBag.Errors = errors.Errors;
                return View("Login", loginDTO);

            }

           
        }

        public IActionResult Logout()
        {

            if (Request.Cookies.ContainsKey("AdminToken"))
            {
                Response.Cookies.Delete("AdminToken");
                Response.Cookies.Delete("AdminUserName");
            }

            return View("Login");
        }


    }
}
