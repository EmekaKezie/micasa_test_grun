using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheEstate.Models.ResponseModels;
using TheEstate.Models.HouseholdModels;
using TheEstate.Repo;
using Microsoft.AspNetCore.Authorization;
using TheEstate.Data;
using TheEstate.Models.AuthModels;
using Newtonsoft.Json;

namespace TheEstate.Api
{
    public static class HouseholdApi
    {
        public static void RegisterHouseholdApi(this WebApplication app)
        {
            app.MapGet("/household/get", GetHouseholds);
            app.MapGet("/household/get/{id}", GetHouseholdById);
            app.MapPost("/household/create", CreateHousehold);
            app.MapPut("/household/update/{id}", UpdateHousehold);
            app.MapDelete("/household/delete/{id}/{profileId}", DeleteHousehold);
        }


        [ProducesResponseType(typeof(PaginatedResponseModel<List<HouseholdModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetHouseholds(HttpContext httpContext, string? search, string? estateId, string? propertyId, string? HouseholdId, string? streetId, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    pageNumber = pageNumber == 0 ? 1 : pageNumber;
                    List<HouseholdModelView> data = await HouseholdRepo.GetHouseholds(search, estateId, propertyId, HouseholdId, streetId, pageSize, pageNumber);
                    int total = await HouseholdRepo.GetHouseholdCount(search, estateId, propertyId, HouseholdId, streetId);
                    PaginatedResponseModel<List<HouseholdModelView>> ret = new PaginatedResponseModel<List<HouseholdModelView>>()
                    {
                        Total = total,
                        Data = data
                    };
                    return Results.Ok(ret);
                }
                else return Results.Unauthorized();

            }
            catch (Exception ex)
            {
                Log.Error($"[Household][GetHouseholds] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(HouseholdModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> GetHouseholdById(HttpContext httpContext, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    HouseholdModelView ret = await HouseholdRepo.GetHouseholdById(id);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Household][GetHouseholdById] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(HouseholdModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateHousehold(HttpContext httpContext, HouseholdModelCreate model)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string householdId = Guid.NewGuid().ToString();
                    string ret = await HouseholdRepo.CreateHousehold(model, householdId);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Household][CreateHousehold] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        HouseholdModelView Household = await HouseholdRepo.GetHouseholdById(householdId);
                        return Results.Ok(Household);
                    }
                    else
                    {
                        Log.Error($"[Household][CreateHousehold] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Household][CreateHousehold] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(HouseholdModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> UpdateHousehold(HttpContext httpContext, HouseholdModelCreate model, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string ret = await HouseholdRepo.UpdateHousehold(model, id);
                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Household][UpdateHousehold] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    if (ret == Constants.StatusResponses.NOTFOUND)
                    {
                        return Results.NotFound("Not found");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        HouseholdModelView Household = await HouseholdRepo.GetHouseholdById(id);
                        return Results.Ok(Household);
                    }
                    else
                    {
                        Log.Error($"[Household][UpdateHousehold] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Household][UpdateHousehold] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }


        }


        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> DeleteHousehold(HttpContext httpContext, string id, string profileId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == profileId;
                if (isValidToken)
                {
                    string ret = await HouseholdRepo.DeactivateHousehold(id);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Household][DeleteHousehold] -> Failed -> HouseholdId={id}, profileId={profileId}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    if (ret == Constants.StatusResponses.NOTFOUND)
                    {
                        return Results.NotFound("Not found");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        return Results.Ok(ret);
                    }
                    else
                    {
                        Log.Error($"[Household][DeleteHousehold] -> Failed -> HouseholdId={id}, profileId={profileId}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Household][DeleteHousehold] -> Exception -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
