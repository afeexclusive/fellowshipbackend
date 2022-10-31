using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Models
{
    public class Fellow
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string OtherName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CVPath { get; set; }
        public string Email { get; set; }
        public ApplicationProgress ApplyProgress { get; set; }
    }

    public enum ApplicationProgress
    {
        SignUp, FirstPage, SecondPage
    }
}
