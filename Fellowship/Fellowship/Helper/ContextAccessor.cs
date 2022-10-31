using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fellowship.Helper
{
    /// <summary>
    /// Access identity claims for authorized user
    /// </summary>
    public class ContextAccessor : IContextAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Injections
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="userManager"></param>
        public ContextAccessor(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager)
        {
            _contextAccessor = contextAccessor;
            this.userManager = userManager;
        }

        /// <summary>
        ///     This method accesses an authorized request containing a token to extract the user email
        /// </summary>
        /// <returns>User email</returns>
        public string GetCurrentUserEmail()
        {
            var identity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            // Gets list of claims.
            var claim = identity.Claims;

            // Gets tenantID from claims. Generally it's a  string.
            var loggedInUSerEmail = claim
                .First(x => x.Type == ClaimTypes.Email).Value;

            //convert to Guid
            return loggedInUSerEmail;
        }


        /// <summary>
        ///     This method accesses an authorized request containing a token to extract the user id
        /// </summary>
        /// <returns>User ID</returns>
        public Guid GetUserId()
        {
            var identity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            // Gets list of claims.
            var claim = identity.Claims;

            // Gets userID from claims. Generally it's a  string.
            var userIdClaim = claim
                .First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            //convert to Guid
            return Guid.Parse(userIdClaim);
        }

        
    }
}
