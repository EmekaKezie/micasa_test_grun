using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AppModels;
using TheEstate.Models.AuthModels;
using TheEstate.Models.PropertyModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class PropertyApi
    {
        public static void RegisterPropertyApi(this WebApplication app)
        {
            app.MapGet("/property/get", GetProperties);
            app.MapGet("/property/get/{id}", GetPropertyById);
            app.MapPost("/property/create", CreateProperty);
            app.MapPut("/property/update/{id}", UpdateProperty);
            app.MapDelete("/property/delete/{id}/{profileId}", DeleteProperty);
            app.MapPost("/property/uploadphoto", UploadPropertyPhoto).Accepts<IFormFile>("multipart/form-data");
        }


        [Authorize]
        [ProducesResponseType(typeof(PaginatedResponseModel<List<PropertyModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetProperties(HttpContext httpContext, string? estateId, string? propertyType, string? propertyCategory, string? zoneId, string? streetId, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    pageNumber = pageNumber == 0 ? 1 : pageNumber;
                    List<PropertyModelView> data = await PropertyRepo.GetProperties(estateId, propertyType, propertyCategory, zoneId, streetId, pageSize, pageNumber);
                    int total = await PropertyRepo.GetPropertiesCount(estateId, propertyType, propertyCategory, zoneId, streetId);
                    PaginatedResponseModel<List<PropertyModelView>> ret = new PaginatedResponseModel<List<PropertyModelView>>()
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
                Log.Error($"[Property][GetProperties] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(PropertyModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> GetPropertyById(HttpContext httpContext, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    PropertyModelView ret = await PropertyRepo.GetPropertyById(id);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Property][GetPropertyById] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(PropertyModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateProperty(HttpContext httpContext, PropertyModelCreate model)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string PropertyId = Guid.NewGuid().ToString();
                    string ret = await PropertyRepo.CreateProperty(model, PropertyId);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Property][CreateProperty] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        PropertyModelView Property = await PropertyRepo.GetPropertyById(PropertyId);
                        return Results.Ok(Property);
                    }
                    else
                    {
                        Log.Error($"[Property][CreateProperty] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Property][CreateProperty] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(PropertyModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> UpdateProperty(HttpContext httpContext, PropertyModelCreate model, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string ret = await PropertyRepo.UpdateProperty(model, id);
                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Property][UpdateProperty] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    if (ret == Constants.StatusResponses.NOTFOUND)
                    {
                        return Results.NotFound("Not found");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        PropertyModelView Property = await PropertyRepo.GetPropertyById(id);
                        return Results.Ok(Property);
                    }
                    else
                    {
                        Log.Error($"[Property][UpdateProperty] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Property][UpdateProperty] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }


        }


        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> DeleteProperty(HttpContext httpContext, string id, string profileId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == profileId;
                if (isValidToken)
                {
                    string ret = await PropertyRepo.DeactivateProperty(id);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Property][DeleteProperty] -> Failed -> PropertyId={id}, profileId={profileId}");
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
                        Log.Error($"[Property][DeleteProperty] -> Failed -> PropertyId={id}, profileId={profileId}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Property][DeleteProperty] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(FileModelResponse), StatusCodes.Status200OK)]
        public static IResult UploadPropertyPhoto(IWebHostEnvironment webHostEnvironment, HttpRequest file)
        {
            try
            {
                IFormFile uploadedFile = file.Form.Files[0];
                string webRoot = webHostEnvironment.WebRootPath;
                FileModelResponse ret = Utility.FileUploader(webRoot, Constants.FileStorageLocations.PROPERTY_PHOTO_PATH, uploadedFile);

                if (ret != null) return Results.Ok(ret);
                else return Results.NoContent();
            }
            catch (Exception ex)
            {
                Log.Error($"[Profile][UploadEstatePhoto] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
