using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class Author
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20")]
        public string Name { get; set; }
    }
}
