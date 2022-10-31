using Fellowship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.DTOs
{
    public class FellowProgressDto
    {
        public Guid FellowID { get; set; }
        public ApplicationProgress Progress { get; set; }
    }
}
