using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.DTOs
{
    public class ResponseModel
    {
        public string Response { get; set; }
        public bool Status { get; set; }
        public object ReturnObj { get; set; }
    }
}
