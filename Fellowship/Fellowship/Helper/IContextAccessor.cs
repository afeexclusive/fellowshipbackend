using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Helper
{
   
    /// <summary>
    /// Gives access to context user
    /// </summary>
    public interface IContextAccessor
    {
        /// <summary>
        /// Gets Elo user Id
        /// </summary>
        /// <returns></returns>
        Guid GetUserId();

        /// <summary>
        /// Get the current user email
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserEmail();
        
    }
    
}
