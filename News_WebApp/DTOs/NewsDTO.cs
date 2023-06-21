using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News_WebApp_API.DTOs
{

    public class AddNewsDTO 
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string NewsDescription { get; set; }

       [Required]
        public int AuthorId { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }
    }


    public class NewsDTO : AddNewsDTO
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string NewsImagePath { get; set; }
        public string NewsImageName { get; set; }


        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
