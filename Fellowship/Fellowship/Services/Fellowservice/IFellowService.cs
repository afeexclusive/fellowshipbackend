using Fellowship.DTOs;
using Fellowship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Services.Fellowengine
{
    public interface IFellowService
    {
        Task<ResponseModel> GetAllFellowsAsync();
        ResponseModel GetFellows(int page = 0, int numberInPage = 20);
        Task<ResponseModel> SaveOnboardingProgressAsync(FellowProgressDto model);
        Task<ResponseModel> UpdateFellowAsync(Fellow fellowModel);
    }
}
