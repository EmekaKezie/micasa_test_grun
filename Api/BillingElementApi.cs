using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AuthModels;
using TheEstate.Models.BillingElementModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Models.ZoneModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class BillingElementApi
    {
        public static void RegisterBillingElementApi(this WebApplication app) {
            app.MapGet("/billingelement/get", GetBillingElement);
            app.MapGet("/billingelement/get/{id}", GetBillingElementById);
            //app.MapPost("/billingelement/create", CreateBillingElement);
        }

        [Authorize]
        [ProducesResponseType(typeof(PaginatedResponseModel<List<BillingElementModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetBillingElement(HttpContext httpContext, string? search, string? estateId, string? zoneId, string? householdClassification, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    pageNumber = pageNumber == 0 ? 1 : pageNumber;
                    List<BillingElementModelView> data = await BillingElementRepo.GetBillingElements(search, estateId, zoneId, householdClassification, pageSize, pageNumber);
                    int total = await BillingElementRepo.GetBillingElementsCount(search, estateId, zoneId, householdClassification);
                    PaginatedResponseModel<List<BillingElementModelView>> ret = new()
                    {
                        Data = data,
                        Total = total
                    };
                    return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[BillingElement][GetBillingElement] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(BillingElementModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> GetBillingElementById(HttpContext httpContext, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    BillingElementModelView ret = await BillingElementRepo.GetBillingElementById(id);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[BillingElement][GetBillingElementById] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(BillingElementModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateBillingElement(HttpContext httpContext, BillingElementModelCreate model)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string elementId = Guid.NewGuid().ToString();
                    string ret = await BillingElementRepo.CreateBillingElement(model, elementId);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[BillingElement][CreateBillingElement] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    else if (ret == Constants.StatusResponses.ERROR)
                    {
                        Log.Error($"[BillingElement][CreateBillingElement] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Zone Code must not be less than 3 characters");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        BillingElementModelView billingElement = await BillingElementRepo.GetBillingElementById(elementId);
                        return Results.Ok(billingElement);
                    }
                    else
                    {
                        Log.Error($"[BillingElement][CreateBillingElement] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[BillingElement][CreateBillingElement] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
