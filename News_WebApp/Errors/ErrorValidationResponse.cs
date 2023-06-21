using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News_WebApp_API.Errors
{
    public class ErrorValidationResponse : ErrorApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ErrorValidationResponse():base(400)
        {

        }


    }
}
