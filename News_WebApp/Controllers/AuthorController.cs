using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domains.Interfaces.IUnitOfWork;
using Domains.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_WebApp_API.DTOs;

namespace Authors_WebApp_API.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AuthorsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var Authors = await _unitOfWork.Author.GetAllAsync();
                return Ok(_mapper.Map<List<AuthorDTO>>(Authors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }



        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthorById(int id)
        {

            if (id <= 0)
                return BadRequest("Id can't be 0 or less.");

            try
            {
                var Authors = await _unitOfWork.Author.GetByIdAsync(id);

                if (Authors == null)
                    return BadRequest("Author is Not Found.");

                return Ok(_mapper.Map<AuthorDTO>(Authors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }






        [HttpPost]
        public async Task<IActionResult> AddNewAuthors([FromBody] AddAuthorDTO Authors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                try
            {
                _unitOfWork.Author.InsertAsync(_mapper.Map<Author>(Authors));
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthors(int id, [FromBody] AddAuthorDTO Authors)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("Id can't be 0 or less.");

            try
            {
                var AuthorsToUpdate = await _unitOfWork.Author.GetByIdAsync(id);

                if (AuthorsToUpdate == null)
                    return BadRequest("Invalid Author ID is sent.");


                _mapper.Map(Authors, AuthorsToUpdate);

                _unitOfWork.Author.Update(AuthorsToUpdate);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + "An error has occured.");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAuthors(int id)
        {

            if (id <= 0)
                return BadRequest("Id can't be 0 or less.");
       

            try
            {
                var AuthorsToDelete = await _unitOfWork.Author.GetByIdAsync(id);

                if (AuthorsToDelete == null)
                    return BadRequest("Invalid Author ID is sent.");

                _unitOfWork.Author.Delete(AuthorsToDelete);
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
