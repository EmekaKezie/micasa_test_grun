using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AuthModels;
using TheEstate.Models.CfgModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class CfgApi
    {
        public static void RegisterCfgApi(this WebApplication app)
        {
            app.MapGet("/cfg/usagecategory/get", GetUsageCategories);
            app.MapGet("/cfg/propertytype/get", GetPropertyTypes);
            app.MapGet("/cfg/householdclassification/get", GetHouseholdClassifications);
        }

        [Authorize]
        [ProducesResponseType(typeof(List<UsageCategoryModelView>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetUsageCategories(HttpContext httpContext, string? estateId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    List<UsageCategoryModelView> ret = await CfgRepo.GetUsageCategories(estateId);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Cfg][GetUsageCategories] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(List<PropertyTypeModelView>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetPropertyTypes(HttpContext httpContext, string? estateId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    List<PropertyTypeModelView> ret = await CfgRepo.GetPropertyTypes(estateId);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Cfg][GetPropertyTypes] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(List<HouseholdClassificationModelView>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetHouseholdClassifications(HttpContext httpContext, string? estateId, string? zoneId)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    List<HouseholdClassificationModelView> ret = await CfgRepo.GetHouseholdClassifications(estateId, zoneId);
                    if (ret == null) return Results.NotFound();
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Cfg][GetHouseholdClassifications] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
