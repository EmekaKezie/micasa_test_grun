using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AppModels;
using TheEstate.Models.AuthModels;
using TheEstate.Models.ProfileModels;
using TheEstate.Models.ResidentModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class ProfileApi
    {
        public static void RegisterProfileApi(this WebApplication app)
        {
            app.MapPost("/profile/uploadphoto", UploadProfilePhoto).Accepts<IFormFile>("multipart/form-data");


            //app.MapPost("profile/upload", (HttpRequest request) =>
            //{
            //    if(!request.Form.Files.Any()) {
            //        return Results.BadRequest("Atleast, one file is requried");
            //    }
            //    else
            //    {
            //        var file = request.Form.Files[0];
            //        return Results.Ok();
            //    }
                
            //}).Accepts<IFormFile>("multipart/form-data").Produces(200);


            app.MapPost("/profile/setup", ProfileSetup);
            app.MapPost("/profile/addEstate", AddEstate);
            app.MapGet("/profile/addEstate/otp/verify", VerifyResidentOtp);

        }


        [ProducesResponseType(typeof(FileModelResponse), StatusCodes.Status200OK)]
        public static IResult UploadProfilePhoto(IWebHostEnvironment webHostEnvironment, HttpRequest file)
        {
            try
            {
                IFormFile uploadedFile = file.Form.Files[0];
                string webRoot = webHostEnvironment.WebRootPath;
                FileModelResponse ret = Utility.FileUploader(webRoot, Constants.FileStorageLocations.PROFILE_PHOTO_PATH, uploadedFile);

                if (ret != null) return Results.Ok(ret);
                else return Results.NoContent();
            }
            catch (Exception ex)
            {
                Log.Error($"[Profile][UploadProfilePhoto] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(AuthModelToken), StatusCodes.Status200OK)]
        public static async Task<IResult> ProfileSetup(ProfileModelSetup model, IConfiguration config)
        {
            try
            {
                //string webRoot = webHostEnvironment.WebRootPath;
                //FileModelResponse image = Utility.FileUploader(webRoot, Constants.FileStorageLocations.PROFILE_PHOTO_PATH, model.ImageBase64!);
                string ret = await ProfileRepo.ProfileSetup(model);
                if (ret == Constants.StatusResponses.SUCCESS)
                {
                    AuthModel auth = await ProfileRepo.GetProfileById(model.ProfileId);
                    if (auth != null)
                    {
                        ClaimsIdentityModel i = new()
                        {
                            Id = auth.Id,
                            Username = auth.Username,
                            Email = auth.Email,
                            MobileNo = auth.MobileNo,
                        };

                        string token = Jwt.GenerateJwt(i, config);

                        AuthModelToken data = new()
                        {
                            Token = token,
                            Id = auth.Id,
                            Username = auth.Username,
                            LoginMethod = auth.LoginMethod,
                            MobileNo = auth.MobileNo,
                            CreatedDate = auth.CreatedDate,
                            Email = auth.Email,
                            Firstname = auth.Firstname,
                            Lastname = auth.Lastname,
                            Status = auth.Status,
                            OnboadingStage = auth.OnboadingStage,
                            Residency = auth.Residency,
                            ImageUrl = auth.ImageUrl,
                        };
                        return Results.Ok(data);
                    }
                    else return Results.Ok("Redirect to Login");
                }
                else return Results.BadRequest("Failed. Please try again");
            }
            catch (Exception ex)
            {
                Log.Error($"[Profile][Setup] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(ResidentModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> AddEstate(AddEstateModel model)
        {
            try
            {
                string ret = await ProfileRepo.AddEstate(model);
                if (ret == Constants.StatusResponses.NOTFOUND) return Results.NotFound("Invalid Resident Code");
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                if (ret == Constants.StatusResponses.SUCCESS)
                {
                    ResidentModelView resident = await ResidentRepo.GetResidentByResidenCode(model.ResidentCode);
                    return Results.Ok(resident);
                }
                //if (ret == Constants.StatusResponses.SENTOTP) return Results.Ok("Sent OTP");
                else return Results.BadRequest(ret);
            }
            catch (Exception ex)
            {
                Log.Error($"[Profile][Setup] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(List<ResidentModelView>), StatusCodes.Status200OK)]
        public static async Task<IResult> VerifyResidentOtp(string residentCode, string profileId, string otp)
        {
            try
            {
                string ret = await ProfileRepo.VerifyResidentOtp(residentCode, otp);
                if (ret == Constants.StatusResponses.EXPIRED) return Results.NotFound("Expired OTP");
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                if (ret == Constants.StatusResponses.SUCCESS) {
                    List<ResidentModelView> resident = await ResidentRepo.GetResidentByProfileId(profileId);
                    return Results.Ok(resident); 
                }
                else return Results.BadRequest(ret);
            }
            catch (Exception ex)
            {
                Log.Error($"[Profile][VerifyResidentOtp] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
