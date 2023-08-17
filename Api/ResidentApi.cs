using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.ResidentModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Models.HouseholdModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class ResidentApi
    {
        public static void RegisterResidentApi(this WebApplication app)
        {
            app.MapGet("/resident/getresidents", GetResidents);
            app.MapPost("/resident/registration", ResidentRegistration);
            app.MapPost("/resident/invite", InviteResident);

        }


        [ProducesResponseType(typeof(PaginatedResponseModel<List<ResidentModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetResidents(string? estateId, string? householdId, string? residentId, string? residentCode, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                pageNumber = pageNumber == 0 ? 1 : pageNumber;
                List<ResidentModelView> data = await ResidentRepo.GetResidents(estateId, householdId, residentId, residentCode, pageSize, pageNumber);
                int total = await ResidentRepo.GetResidentsCount(estateId, householdId, residentId, residentCode);

                PaginatedResponseModel<List<ResidentModelView>> ret = new PaginatedResponseModel<List<ResidentModelView>>()
                {
                    Total = total,
                    Data = data
                };

                return Results.Ok(ret);

            }
            catch (Exception ex)
            {
                Log.Error($"[Street][GetStreets] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(List<ResidentModelView>), StatusCodes.Status200OK)]
        public static async Task<IResult> ResidentRegistration(ResidentModelCreate model)
        {
            try
            {
                string ret = await ResidentRepo.ResidentRegistration(model);
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                else if (ret == Constants.StatusResponses.SUCCESS) { 
                    List<ResidentModelView> resident = await ResidentRepo.GetResidentByProfileId(model.ProfileId!);
                    return Results.Ok(resident); 
                }
                else return Results.BadRequest("Failed. Please try again");

            }
            catch (Exception ex)
            {
                Log.Error($"[Resident][ResidentRegistration] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(List<ResidentModelView>), StatusCodes.Status200OK)]
        public static async Task<IResult> InviteResident(ResidentModelCreate model)
        {
            try
            {
                string ret = await ResidentRepo.InviteResident(model);
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                if (ret == Constants.StatusResponses.INVALID) return Results.BadRequest("Incorrect Username");
                if (ret == Constants.StatusResponses.INVALID_FORMAT) return Results.BadRequest("Email is not in a valid format");
                else if (ret == Constants.StatusResponses.SUCCESS) return Results.Ok("Success");
                else return Results.BadRequest("Failed. Please try again");

            }
            catch (Exception ex)
            {
                Log.Error($"[Resident][InviteResident] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
