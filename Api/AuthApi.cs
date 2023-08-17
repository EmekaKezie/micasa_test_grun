using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AuthModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class AuthApi
    {
        public static void RegisterAuthApi(this WebApplication app)
        {
            app.MapPost("/auth/createuser", CreateUser);
            app.MapPost("/auth/login", Login);
            //app.MapGet("getclaims", GetClaims);
            app.MapGet("/auth/otp/verify", VerifyOtp);
            app.MapGet("/auth/otp/resend", ResendOtp);
            app.MapGet("/auth/whatsapp/webhook", WhatsappWebhookTest);
        }


        [ProducesResponseType(typeof(AuthModel), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateUser(AuthModelLogin model)
        {
            try
            {
                string ret = await AuthRepo.CreateUser(model);
                if (ret == Constants.StatusResponses.INVALID_FORMAT) return Results.BadRequest("Username is not in a valid format");
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                else if (ret == Constants.StatusResponses.EXISTS)
                {
                    AuthModel auth = await AuthRepo.Login(model);
                    if (auth != null && !(auth.OnboadingStage!.Equals(Constants.OnboardingState.COMPLETE.ToString())))
                    {
                        return Results.Ok(auth);
                    }
                    else  return Results.Conflict("User already exists");
                }
                else
                {
                    AuthModel auth = await ProfileRepo.GetProfileById(ret);
                    if (auth != null)
                    {
                        return Results.Ok(auth);
                    }
                    else return Results.BadRequest("Something went wrong");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[User][CreateUser] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(AuthModelToken), StatusCodes.Status200OK)]
        public static async Task<IResult> Login(IConfiguration config, AuthModelLogin model)
        {
            try
            {
                AuthModel auth = await AuthRepo.Login(model);
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
                        Username =  auth.Username,
                        LoginMethod = auth.LoginMethod,
                        MobileNo = auth.MobileNo,
                        CreatedDate = auth.CreatedDate,
                        Email = auth.Email,
                        Firstname = auth.Firstname,
                        Lastname = auth.Lastname,
                        Status = auth.Status,
                        ImageUrl = auth.ImageUrl,
                        OnboadingStage = auth.OnboadingStage,
                        Residency = auth.Residency
                        
                    };
                    return Results.Ok(data);
                }
                else return Results.NotFound("Invalid Credential");
            }
            catch (Exception ex)
            {
                Log.Error($"[Auth][Login] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize(Policy = Constants.JWTClaimTypes.ResidentCategory)]
        public static IResult GetClaims(HttpContext httpContext)
        {
            try
            {
                bool validToken = Jwt.ValidateToken(httpContext, "MICASA1");
                if (validToken)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.Forbid();
                }
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(AuthModel), StatusCodes.Status200OK)]
        public static async Task<IResult> VerifyOtp(string profileId, string otp)
        {
            try
            {
                string ret = await AuthRepo.VerifyOtp(profileId, otp);
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                if (ret == Constants.StatusResponses.EXPIRED) return Results.BadRequest("Expired OTP");
                if (ret == Constants.StatusResponses.INVALID) return Results.BadRequest("Invalid OTP");
                else if (ret == Constants.StatusResponses.SUCCESS)
                {
                    AuthModel auth = await ProfileRepo.GetProfileById(profileId);
                    if (auth != null)
                    {
                        return Results.Ok(auth);
                    }
                    else return Results.BadRequest("Something went wrong");
                }
                else return Results.BadRequest("Failed");
            }
            catch (Exception ex)
            {
                Log.Error($"[Auth][VerifyOtp] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> ResendOtp(string profileId)
        {
            try
            {
                string ret = await AuthRepo.ResendOtp(profileId);
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                else if (ret == Constants.StatusResponses.SUCCESS) return Results.Ok(ret);
                else if (ret == Constants.StatusResponses.NOTFOUND) return Results.NotFound("User not found");
                else return Results.BadRequest("Failed");
            }
            catch (Exception ex)
            {
                Log.Error($"[Auth][VerifyOtp] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        public static async Task<IResult> WhatsappWebhookTest()
        {
            try
            {
                return Results.Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"[Auth][WhatsappWebhookTest] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }
        
    }
}
