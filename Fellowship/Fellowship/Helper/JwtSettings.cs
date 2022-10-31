﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Helper
{
    public class JwtSettings
    {
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpirationTime { get; set; }
        public string Secret { get; set; }

    }
}
