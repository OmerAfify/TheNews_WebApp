using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class News
    {

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string NewsDescription { get; set; }


        public string NewsImageName { get; set; }
        public string NewsImagePath { get; set; }

        [Required]

        public int AuthorId { get; set; }
        public Author Author { get; set; }


      [Required]
      public DateTime PublicationDate { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow ;


    }
}
