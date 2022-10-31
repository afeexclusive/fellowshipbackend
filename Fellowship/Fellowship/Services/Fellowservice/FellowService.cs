using Fellowship.DataAccess.UnitOfWork.Interface;
using Fellowship.DTOs;
using Fellowship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Services.Fellowengine
{
    public class FellowService : IFellowService
    {
        private readonly IUnitOfWork<Fellow> unitOfWorkFellow;

        public FellowService(IUnitOfWork<Fellow> unitOfWorkFellow)
        {
            this.unitOfWorkFellow = unitOfWorkFellow;
        }

        public async Task<ResponseModel> GetAllFellowsAsync()
        {
            return new ResponseModel {Status= true, Response="Success", ReturnObj = await unitOfWorkFellow.Repository.GetAll() };
        }

        public ResponseModel GetFellows(int page = 0, int numberInPage = 20)
        {

            var query = unitOfWorkFellow.Repository.GetAllQuery();

            if (query.Any())
            {
                int count = query.Count();
                query.Skip(page * numberInPage).Take(numberInPage);

                return new ResponseModel { Status = true, Response = "Success", ReturnObj = new {TotalFellows = count, Fellows = query } };
            }

            return new ResponseModel { Response = "No fellows found", Status = false };
        }

        public async Task<ResponseModel> SaveOnboardingProgressAsync(FellowProgressDto model)
        {
            try
            {
                Fellow fellowToBeUpdated = await PatchFellow(null, null, model);
                unitOfWorkFellow.Repository.Update(fellowToBeUpdated);
                await unitOfWorkFellow.Save();
                return new ResponseModel { Response = "Success", Status = true, ReturnObj = fellowToBeUpdated };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Response = ex.Message, Status = false };
            }
        }

        public async Task<ResponseModel> UpdateFellowAsync(Fellow fellowModel)
        {
            try
            {
                var fellowToBeUpdated = await PatchFellow(fellowModel, null, null);
                unitOfWorkFellow.Repository.Update(fellowToBeUpdated);
                await unitOfWorkFellow.Save();
                return new ResponseModel { Response = "Successful", Status = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Response = ex.Message, Status = false };
            }
        }

        private async Task<Fellow> PatchFellow(Fellow fellowModel, UserDto user, FellowProgressDto fellowProgress)
        {
            Fellow fellowToUpdate;

            if (fellowModel != null)
            {
                if (fellowModel.ID == Guid.Empty && string.IsNullOrWhiteSpace(fellowModel.Email)) return null;

                Guid searchId = fellowModel.ID != Guid.Empty ? fellowModel.ID : Guid.Empty;
                string searchEmail = searchId == Guid.Empty ? fellowModel.Email : "";
                

                if (string.IsNullOrWhiteSpace(searchEmail))
                {
                    fellowToUpdate = await unitOfWorkFellow.Repository.GetByID(fellowModel.ID);
                    if (fellowToUpdate == null) return null;
                    return MapFellowProperties(fellowModel, fellowToUpdate);
                }
                else
                {
                    fellowToUpdate = unitOfWorkFellow.Repository.GetAllQuery().FirstOrDefault(x => x.Email == fellowModel.Email);
                    if (fellowToUpdate == null) return null;
                        return MapFellowProperties(fellowModel, fellowToUpdate);
                }

            }

            if (user != null)
            {
                fellowToUpdate = unitOfWorkFellow.Repository.GetAllQuery().FirstOrDefault(x => x.Email == fellowModel.Email);
                if (fellowToUpdate == null) return null;
                return MapFellowProperties(fellowModel =new Fellow { Email = user.Email}, fellowToUpdate);
            }

            if (fellowProgress != null)
            {
                fellowToUpdate = await unitOfWorkFellow.Repository.GetByID(fellowProgress.FellowID);
                if (fellowToUpdate == null) return null;
                fellowToUpdate.ApplyProgress = fellowProgress.Progress;
                return fellowToUpdate;
            }

            return null;
        }

        private Fellow MapFellowProperties(Fellow incomingModel, Fellow fellowToUpdate)
        {
            incomingModel.Address = string.IsNullOrWhiteSpace(incomingModel.Address) ? fellowToUpdate.Address : incomingModel.Address;
            incomingModel.CVPath = string.IsNullOrWhiteSpace(incomingModel.CVPath) ? fellowToUpdate.CVPath : incomingModel.CVPath;
            incomingModel.Email = string.IsNullOrWhiteSpace(incomingModel.Email) ? fellowToUpdate.Email : incomingModel.Email;
            incomingModel.ApplyProgress = fellowToUpdate.ApplyProgress;
            incomingModel.PhoneNumber = string.IsNullOrWhiteSpace(incomingModel.PhoneNumber) ? fellowToUpdate.PhoneNumber : incomingModel.PhoneNumber;
            incomingModel.OtherName = string.IsNullOrWhiteSpace(incomingModel.OtherName) ? fellowToUpdate.OtherName : incomingModel.OtherName;
            incomingModel.LastName = string.IsNullOrWhiteSpace(incomingModel.LastName) ? fellowToUpdate.LastName : incomingModel.LastName;
            incomingModel.FirstName = string.IsNullOrWhiteSpace(incomingModel.FirstName) ? fellowToUpdate.FirstName : incomingModel.FirstName;
            return incomingModel;
        }

    }
}
