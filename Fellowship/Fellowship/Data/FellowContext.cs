using Fellowship.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Data
{
    public class FellowContext: IdentityDbContext<IdentityUser>
    {
        public FellowContext(DbContextOptions<FellowContext> option) : base(option)
        {

        }

        public DbSet<Fellow> Fellows { get; set; }
    }
}
