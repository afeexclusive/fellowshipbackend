using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Helper
{
    public class FirebaseFileStore
    {
        public string ApiKey { get; set; }
        public string AuthEmail { get; set; }
        public string AuthPassword { get; set; }
        public string Bucket { get; set; }
    }
}
