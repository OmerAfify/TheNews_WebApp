using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Domains.Interfaces.IUnitOfWork;
using Domains.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News_WebApp_API.DTOs;
using News_WebApp_API.Errors;
using News_WebApp_API.Helpers;
using News_WebApp_API.ViewModels;

namespace News_WebApp_API.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class NewsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNews()
        {
            try
            {
                var News = await _unitOfWork.News.GetAllAsync(new List<string>() { "Author" });
                return Ok(_mapper.Map<List<NewsDTO>>(News));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }



        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewsById(int id)
        {

            if (id <= 0)
                return BadRequest("Id can't be 0 or less.");

            try
            {
                var New = await _unitOfWork.News.FindAsync(n=>n.Id==id, new List<string>() { "Author" });

                if (New == null)
                    return BadRequest("News Not Found.");

                return Ok(_mapper.Map<NewsDTO>(New));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddaNews([FromForm] VmAddNews NewsData)
        {
       
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                //Date Check
       
                if (NewsData.AddNewsDTO.PublicationDate < DateTime.Today || NewsData.AddNewsDTO.PublicationDate > DateTime.Today.AddDays(7))
                    return BadRequest(new ErrorValidationResponse() { Errors = new List<string>() { "Publication date must be between today and a week from today."} });
                

                //Author ID Check

                var author = await _unitOfWork.Author.GetByIdAsync(NewsData.AddNewsDTO.AuthorId);

                if (author == null) 
                    return BadRequest(new ErrorValidationResponse() { Errors = new List<string>() { "Please enter a valid author id." } });


                //Create News Model
                var News = _mapper.Map<News>(NewsData.AddNewsDTO);

                //insert image
                if (NewsData.NewsImageFile != null)
                {    
                    var imagesName = ImageUploader.UploadImage(NewsData.NewsImageFile, @"wwwRoot\Images\NewsImages\").Result;
                    News.NewsImageName = imagesName;
                    News.NewsImagePath = @"Images\NewsImages\";
                }

                _unitOfWork.News.InsertAsync(News);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] VmAddNews NewsData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest(new ErrorValidationResponse() { Errors = new List<string>() { "Id can't be 0 or less." } });

            try
            {
                var NewsToUpdate = await _unitOfWork.News.GetByIdAsync(id);


                //Check news id exists
                if (NewsToUpdate == null)
                    return BadRequest(new ErrorValidationResponse() { Errors = new List<string>() { "Please enter a valid author id." } });

                //Check author id exists
                var author = await _unitOfWork.Author.GetByIdAsync(NewsData.AddNewsDTO.AuthorId);
             
                if (author == null)
                    return BadRequest(new ErrorValidationResponse() { Errors = new List<string>() { "Please enter a valid author id." } });

                //Check date
                if (NewsData.AddNewsDTO.PublicationDate < DateTime.Today || NewsData.AddNewsDTO.PublicationDate > DateTime.Today.AddDays(7))
                    return BadRequest(new ErrorValidationResponse() { Errors = new List<string>() { "Publication date must be between today and a week from today." } });

                //Mapping 
                _mapper.Map(NewsData.AddNewsDTO, NewsToUpdate);

                //image upload
                if (NewsData.NewsImageFile != null)
                {
                    //Delete old one if exists
                    if(!string.IsNullOrEmpty(NewsToUpdate.NewsImagePath) && !string.IsNullOrEmpty(NewsToUpdate.NewsImageName))
                           ImageUploader.DeleteImage(@"wwwRoot\"+NewsToUpdate.NewsImagePath, NewsToUpdate.NewsImageName);

                    //Upload new img
                    var newImagesName = ImageUploader.UploadImage(NewsData.NewsImageFile, @"wwwRoot\Images\NewsImages\").Result;
                    NewsToUpdate.NewsImageName = newImagesName;
                    NewsToUpdate.NewsImagePath = @"Images\NewsImages\";
                }


                _unitOfWork.News.Update(NewsToUpdate);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteNews(int id)
        {

            if (id <= 0)
                return BadRequest("Id can't be 0 or less.");
       
            try
            {
                var NewToDelete = await _unitOfWork.News.GetByIdAsync(id);

                if (NewToDelete == null)
                    return BadRequest("Invalid News ID is sent.");

                //Delete Image
                //Delete old one 
                ImageUploader.DeleteImage(@"wwwRoot\"+NewToDelete.NewsImagePath, NewToDelete.NewsImageName);



                _unitOfWork.News.Delete(NewToDelete);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }



    }
}


