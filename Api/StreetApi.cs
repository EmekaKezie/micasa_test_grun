using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AuthModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Models.StreetModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class StreetApi
    {
        public static void RegisterStreetApi(this WebApplication app)
        {
            app.MapGet("/street/get", GetStreets);
            app.MapGet("/street/get/{id}", GetStreetById);
            app.MapPost("/street/create", CreateStreet);
            app.MapPut("/street/update/{id}", UpdateStreet);
            app.MapDelete("/street/delete/{id}/{profileId}", DeleteStreet);
        }


        [ProducesResponseType(typeof(PaginatedResponseModel<List<StreetModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetStreets(HttpContext httpContext, string? search, string? streetType, string? zoneId, string? estateId, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    pageNumber = pageNumber == 0 ? 1 : pageNumber;
                    List<StreetModelView> data = await StreetRepo.GetStreets(search, streetType, zoneId, estateId, pageSize, pageNumber);
                    int total = await StreetRepo.GetStreetsCount(search, streetType, zoneId, estateId);
                    PaginatedResponseModel<List<StreetModelView>> ret = new PaginatedResponseModel<List<StreetModelView>>()
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
                Log.Error($"[Street][GetStreets] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(StreetModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> GetStreetById(HttpContext httpContext, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    StreetModelView ret = await StreetRepo.GetStreetById(id);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Street][GetStreetById] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(StreetModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateStreet(HttpContext httpContext, StreetModelCreate model)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string zoneId = Guid.NewGuid().ToString();
                    string ret = await StreetRepo.CreateStreet(model, zoneId);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Street][CreateStreet] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        StreetModelView street = await StreetRepo.GetStreetById(zoneId);
                        return Results.Ok(street);
                    }
                    else
                    {
                        Log.Error($"[Street][CreateStreet] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Street][CreateStreet] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(StreetModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> UpdateStreet(HttpContext httpContext, StreetModelCreate model, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string ret = await StreetRepo.UpdateStreet(model, id);
                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Street][UpdateStreet] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    if (ret == Constants.StatusResponses.NOTFOUND)
                    {
                        return Results.NotFound("Not found");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        StreetModelView zone = await StreetRepo.GetStreetById(id);
                        return Results.Ok(zone);
                    }
                    else
                    {
                        Log.Error($"[Street][UpdateStreet] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Zone][UpdateZone] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }


        }


        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> DeleteStreet(HttpContext httpContext, string id, string profileId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == profileId;
                if (isValidToken)
                {
                    string ret = await StreetRepo.DeactivateStreet(id);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Street][DeleteStreet] -> Failed -> streetId={id}, profileId={profileId}");
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
                        Log.Error($"[Street][DeleteStreet] -> Failed -> streetId={id}, profileId={profileId}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Street][DeleteStreet] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
