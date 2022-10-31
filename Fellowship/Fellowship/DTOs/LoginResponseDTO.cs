using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.DTOs
{
    public class LoginResponseDTO
    {
        public Guid UserID { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}
