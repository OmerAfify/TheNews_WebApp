using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News_WebApp_API.DTOs
{

    public class AddAuthorDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3,ErrorMessage ="Name must be between 3 and 20")]
        public string Name { get; set; }
    }

    public class AuthorDTO : AddAuthorDTO
    {
        public int Id { get; set; }
    }
}
