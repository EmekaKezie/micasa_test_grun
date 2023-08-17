using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AuthModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Models.ZoneModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class ZoneApi
    {
        public static void RegisterZoneApi(this WebApplication app)
        {
            app.MapGet("/zone/get", GetZones);
            app.MapGet("/zone/get/{id}", GetZoneById);
            app.MapPost("/zone/create", CreateZone);
            app.MapPut("/zone/update/{id}", UpdateZone);
            app.MapDelete("/zone/delete/{id}/{profileId}", DeleteZone);
        }


        [Authorize]
        [ProducesResponseType(typeof(PaginatedResponseModel<List<ZoneModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetZones(HttpContext httpContext, string? search, string? estateId, string? zoneType, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    pageNumber = pageNumber == 0 ? 1 : pageNumber;
                    List<ZoneModelView> data = await ZoneRepo.GetZones(search, estateId, zoneType, pageSize, pageNumber);
                    int total = await ZoneRepo.GetZonesCount(search, estateId, zoneType);
                    PaginatedResponseModel<List<ZoneModelView>> ret = new()
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
                Log.Error($"[Zone][GetZones] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(ZoneModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> GetZoneById(HttpContext httpContext, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    ZoneModelView ret = await ZoneRepo.GetZoneById(id);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Zone][GetZoneById] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(ZoneModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateZone(HttpContext httpContext, ZoneModelCreate model)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string zoneId = Guid.NewGuid().ToString();
                    string ret = await ZoneRepo.CreateZone(model, zoneId);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Zone][CreateZone] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    else if(ret == Constants.StatusResponses.ERROR)
                    {
                        Log.Error($"[Zone][CreateZone] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Zone Code must not be less than 3 characters");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        ZoneModelView zone = await ZoneRepo.GetZoneById(zoneId);
                        return Results.Ok(zone);
                    }
                    else
                    {
                        Log.Error($"[Zone][CreateZone] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Zone][CreateZone] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(ZoneModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> UpdateZone(HttpContext httpContext, ZoneModelCreate model, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string ret = await ZoneRepo.UpdateZone(model, id);
                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Zone][UpdateZone] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    if (ret == Constants.StatusResponses.NOTFOUND)
                    {
                        return Results.NotFound("Not found");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        ZoneModelView zone = await ZoneRepo.GetZoneById(id);
                        return Results.Ok(zone);
                    }
                    else
                    {
                        Log.Error($"[Zone][UpdateZone] -> Failed -> {JsonConvert.SerializeObject(model)}");
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
        public static async Task<IResult> DeleteZone(HttpContext httpContext, string id, string profileId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == profileId;
                if (isValidToken)
                {
                    string ret = await ZoneRepo.DeactivateZone(id);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Zone][DeleteZone] -> Failed -> zoneId={id}, profileId={profileId}");
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
                        Log.Error($"[Zone][DeleteZone] -> Failed -> zoneId={id}, profileId={profileId}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Zone][DeleteZone] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }

    }
}
