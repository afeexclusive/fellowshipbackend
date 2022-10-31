using Fellowship.DTOs;
using Fellowship.Models;
using Fellowship.Services.Fellowengine;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Controllers
{
    /// <summary>
    /// Fellow controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FellowController : ControllerBase
    {
        private readonly IFellowService fellowService;

        public FellowController(IFellowService fellowService)
        {
            this.fellowService = fellowService;
        }

        /// <summary>
        /// Gets all fellows
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RetrieveAllFellowsAsync() => Ok(await fellowService.GetAllFellowsAsync());

        /// <summary>
        /// Gets paginated Fellows
        /// </summary>
        /// <param name="page"></param>
        /// <param name="numInPage"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult RetrieveFellows(int page, int numInPage) => Ok(fellowService.GetFellows(page, numInPage));

        /// <summary>
        /// Update onboarding progress
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("saveregprogress")]
        public async Task<IActionResult> SaveOnboardingStage(FellowProgressDto model) => Ok( await fellowService.SaveOnboardingProgressAsync(model));

        /// <summary>
        /// Update onboarding progress
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditFellow (Fellow model) => Ok(await fellowService.UpdateFellowAsync(model));

    }
}
