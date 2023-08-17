using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AppModels;
using TheEstate.Models.AuthModels;
using TheEstate.Models.EstateModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class EstateApi
    {
        public static void RegisterEstateApi(this WebApplication app)
        {
            app.MapGet("/estate/get", GetEstates);
            app.MapGet("/estate/get/{id}", GetEstateById);
            app.MapPost("/estate/create", CreateEstate);
            app.MapPut("/estate/update/{id}", UpdateEstate);
            app.MapDelete("/estate/delete/{id}/{profileId}", DeleteEstate);
            app.MapPost("/estate/uploadphoto", UploadEstatePhoto).Accepts<IFormFile>("multipart/form-data");
        }

        [Authorize]
        [ProducesResponseType(typeof(PaginatedResponseModel<List<EstateModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetEstates(HttpContext httpContext, string? search, string? status, string? code, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    pageNumber = pageNumber == 0 ? 1 : pageNumber;
                    List<EstateModelView> data = await EstateRepo.GetEstates(search, status, code, pageSize, pageNumber);
                    int total = await EstateRepo.GetEstatesCount(search, status, code);
                    PaginatedResponseModel<List<EstateModelView>> ret = new PaginatedResponseModel<List<EstateModelView>>()
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
                Log.Error($"[Estate][GetEstates] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(EstateModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> GetEstateById(HttpContext httpContext, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    EstateModelView ret = await EstateRepo.GetEstateById(id);

                    if (ret == null) return Results.NotFound("Not Found");
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();

            }
            catch (Exception ex)
            {
                Log.Error($"[Estate][GetEstateById] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(EstateModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateEstate(HttpContext httpContext, EstateModelCreate model)
        {
            Log.Information($"[Estate][CreateEstate] -> Info -> {JsonConvert.SerializeObject(model)}");
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string estateId = Guid.NewGuid().ToString();
                    string ret = await EstateRepo.CreateEstate(model, estateId);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Estate][CreateEstate] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        EstateModelView estate = await EstateRepo.GetEstateById(estateId);
                        return Results.Ok(estate);
                    }
                    else
                    {
                        Log.Error($"[Estate][CreateEstate] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Estate][CreateEstate] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(EstateModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> UpdateEstate(HttpContext httpContext, EstateModelCreate model, string id)
        {
            Log.Information($"[Estate][UpdateEstate] -> Info -> {JsonConvert.SerializeObject(model)}");
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string ret = await EstateRepo.UpdateEstate(model, id);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Estate][UpdateEstate] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    if (ret == Constants.StatusResponses.NOTFOUND)
                    {
                        return Results.NotFound("Not found");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        EstateModelView estate = await EstateRepo.GetEstateById(id);
                        return Results.Ok(estate);
                    }
                    else
                    {
                        Log.Error($"[Estate][UpdateEstate] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Estate][UpdateEstate] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> DeleteEstate(HttpContext httpContext, string id, string profileId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == profileId;
                if (isValidToken)
                {
                    string ret = await EstateRepo.DeactivateEstate(id);

                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Estate][DeleteEstate] -> Failed -> estateId={id}, profileId={profileId}");
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
                        Log.Error($"[Estate][DeleteEstate] -> Failed -> estateId={id}, profileId={profileId}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Estate][DeleteEstate] -> Exception -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(FileModelResponse), StatusCodes.Status200OK)]
        public static IResult UploadEstatePhoto(IWebHostEnvironment webHostEnvironment, HttpRequest file)
        {
            try
            {
                IFormFile uploadedFile = file.Form.Files[0];
                string webRoot = webHostEnvironment.WebRootPath;
                FileModelResponse ret = Utility.FileUploader(webRoot, Constants.FileStorageLocations.ESTATE_PHOTO_PATH, uploadedFile);

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
